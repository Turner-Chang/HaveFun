using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaveFun.Models;
using HaveFun.Common;
using HaveFun.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace HaveFun.Controllers.APIs
{
	//[Authorize(AuthenticationSchemes = "Bearer,Cookies")]
	[Route("api/UserInfo/[action]")]
    [ApiController]
    public class UserInfoApiController : ControllerBase
    {
		
		
		SaveImage _saveImage;
        HaveFunDbContext _context;
		IWebHostEnvironment _environment;

		public UserInfoApiController(HaveFunDbContext context, SaveImage saveImage, IWebHostEnvironment environment)
        {
            _context = context;
            _saveImage = saveImage;
			_environment = environment;
		}
		

		// GET: api/UserInfo/GetUserinfos
		[HttpGet("{id}")]
        [ActionName("GetUserinfos")]
        //拿資料
        public async Task<UserIfDTO> GetUserInfos(int id)
        {
			//string userId = Request.Cookies["userId"];
			var userInf = await _context.UserInfos
            .Where(u => u.Id == id) // 使用 Where 來過濾資料
            .Select(u => new UserIfDTO
            {
                Id = u.Id,
                Name = u.Name,
                Address = u.Address,
                PhoneNumber = u.PhoneNumber,
                Gender = u.Gender,
                BirthDay = u.BirthDay,
                Introduction = u.Introduction               
            })
            .FirstOrDefaultAsync(); // 使用 FirstOrDefaultAsync 來獲取單個資料

            return userInf;
        }
        //GetUserInfo 找全部的user 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserIfDTO>>> GetUserInfo()
        {
            var userInfos = await _context.UserInfos
                .Select(u => new UserIfDTO
                {
                    Id = u.Id, // Include the Id property
                    Name = u.Name,
                    Address = u.Address,
                    PhoneNumber = u.PhoneNumber,
                    Gender = u.Gender,
                    BirthDay = u.BirthDay,
                    Introduction = u.Introduction                    
                })
                .ToListAsync();
            return userInfos;
        }

        //GET: api/UserInfo/GetPicture/2
        [HttpGet("{id}")]
        public async Task<FileResult> GetPicture(int id)
        {
            UserInfo? user = await _context.UserInfos.FindAsync(id);
            if (user.ProfilePicture != null)
            {
                string? path = user.ProfilePicture;
                byte[] ImageContent = System.IO.File.ReadAllBytes(path);
                return File(ImageContent, "image/*");
            }
            return null;
        }

		// Get: api/Profile/GetAlbumPictures/{userId}
		[HttpGet("{userId}")]
		public async Task<ActionResult<IEnumerable<string>>> GetAlbumPictures(int userId)
		{
			var pictures = await _context.UserPictures
				.Where(up => up.UserId == userId)
				.Select(up => up.Picture)
				.ToListAsync();

			return Ok(pictures);
		}


		// PUT: api/UserInfoApi/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
        public async Task<IActionResult> PutUserInfo(int id, UserInfo userInfo)
        {
            if (id != userInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(userInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserInfoApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserInfo>> PostUserInfo(UserInfo userInfo)
        {
            _context.UserInfos.Add(userInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserInfo", new { id = userInfo.Id }, userInfo);
        }

        //POST: api/UserInfo/SaveData        
        [HttpPost]
        [ActionName("SaveData")]
        public async Task<IActionResult> SaveData([FromForm]UserIfDTO userInfoDTO)
        {
            if (!ModelState.IsValid)  /*檢查狀態*/
            {
                return BadRequest(ModelState);
            }
			// 檢查電話號碼格式
			if (!string.IsNullOrEmpty(userInfoDTO.PhoneNumber) && !Regex.IsMatch(userInfoDTO.PhoneNumber, @"^[0-9]+$"))
			{
				ModelState.AddModelError(nameof(userInfoDTO.PhoneNumber), "電話號碼格式不正確");
				return BadRequest(ModelState);
			}

			UserInfo? user = await _context.UserInfos.FindAsync(userInfoDTO.Id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            user.Name = userInfoDTO.Name;
            user.Address = userInfoDTO.Address;                       
            user.Gender = userInfoDTO.Gender;
            user.BirthDay = userInfoDTO.BirthDay;
            user.Introduction = userInfoDTO.Introduction;

			// 檢查是否有新的電話號碼提供
			if (!string.IsNullOrWhiteSpace(userInfoDTO.PhoneNumber))
			{
				user.PhoneNumber = userInfoDTO.PhoneNumber;
			}
			else
			{
				user.PhoneNumber = null; // 或者設置為空字符串，根據您的資料庫設計
			}

			if (userInfoDTO.ProfilePicture != null)
            {
                // 把大頭照丟到wwwtoot的images的headshots資料夾內
                string imgPath = "wwwroot\\images\\headshots";
                // 檔案名為帳號+檔案名
                string imgName = user.Account+userInfoDTO.ProfilePicture.FileName; // Use unique name
                // string fullPath = Path.Combine(imgPath, imgName);
                string fullPath = string.Empty;
                _saveImage.Name = imgName;
                _saveImage.Path = imgPath;
                _saveImage.Picture = userInfoDTO.ProfilePicture;
                _saveImage.Save(out fullPath);

				user.ProfilePicture = fullPath; // Save path in the database
            }
            _context.UserInfos.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "用戶資料已保存成功" });
        }

		//保存個人相簿圖片
		// POST: api/UserInfo/UploadAlbumPicture
		[HttpPost]
		public async Task<IActionResult> UploadAlbumPicture([FromForm] int userId, [FromForm] List<IFormFile> albumPictures)
		{
			if (albumPictures == null || !albumPictures.Any())
			{
				return BadRequest("沒有上傳圖片");
			}

			var user = await _context.UserInfos.FindAsync(userId);
			if (user == null)
			{
				return NotFound("User not found.");
			}

			string albumPath = Path.Combine(_environment.WebRootPath, "images", "album");
			Directory.CreateDirectory(albumPath);

			foreach (var file in albumPictures)
			{
				if (file.Length > 0)
				{
					string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
					string filePath = Path.Combine(albumPath, fileName);

					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await file.CopyToAsync(stream);
					}

					var userPicture = new UserPicture
					{
						UserId = user.Id,
						Picture = $"/images/album/{fileName}"
					};

					_context.UserPictures.Add(userPicture);
				}
			}

			await _context.SaveChangesAsync();

			return Ok(new { message = "圖片已成功上傳到相簿" });
		}


        // DELETE: api/UserInfo/DeletePicture/{pictureId}
        [HttpDelete("{pictureId}")]
        public async Task<IActionResult> DeletePicture(int pictureId)
        {
            var picture = await _context.UserPictures.FindAsync(pictureId);
            if (picture == null)
            {
                return NotFound("Picture not found");
            }

            if (System.IO.File.Exists(Path.Combine(_environment.WebRootPath, picture.Picture.TrimStart('/'))))
            {
                System.IO.File.Delete(Path.Combine(_environment.WebRootPath, picture.Picture.TrimStart('/')));
            }

            _context.UserPictures.Remove(picture);
            await _context.SaveChangesAsync();

            return Ok(new { message = "圖片已刪除" });
        }

			// DELETE: api/UserInfoApi/5
			[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserInfo(int id)
        {
            var userInfo = await _context.UserInfos.FindAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }

            _context.UserInfos.Remove(userInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserInfoExists(int id)
        {
            return _context.UserInfos.Any(e => e.Id == id);
        }
    }
}
