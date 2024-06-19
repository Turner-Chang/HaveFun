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

namespace HaveFun.Controllers.APIs
{
	[Authorize(AuthenticationSchemes = "Bearer,Cookies")]
	[Route("api/UserInfo/[action]")]
    [ApiController]
    public class UserInfoApiController : ControllerBase
    {
         SaveImage _saveImage;
         HaveFunDbContext _context;

        public UserInfoApiController(HaveFunDbContext context, SaveImage saveImage)
        {
            _context = context;
            _saveImage = saveImage;
        }

        // GET: api/UserInfo/GetUserinfos
        [HttpGet("{id}")]
        [ActionName("GetUserinfos")]
        //拿資料
        public async Task<UserIfDTO> GetUserInfos(int id)
        {
			string userId = Request.Cookies["userId"];
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
                Introduction = u.Introduction,
                Password = u.Password, // 注意: 密碼通常不應該被傳送到客戶端
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
                    Introduction = u.Introduction,
                    Password = u.Password
                })
                .ToListAsync();
            return userInfos;
        }

        //GET: api/UserInfo/GetPicture/2
        [HttpGet("{id}")]
        public async Task<FileResult> GetPicture(int id)
        {
            UserInfo? user = await _context.UserInfos.FindAsync(id);
            if (user == null) { return null; }
            string path = user.ProfilePicture;
            byte[] ImageContent = System.IO.File.ReadAllBytes(path);
            return File(ImageContent, "image/*");
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
            UserInfo? user = await _context.UserInfos.FindAsync(userInfoDTO.Id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            user.Name = userInfoDTO.Name;
            user.Address = userInfoDTO.Address;
            user.PhoneNumber = userInfoDTO.PhoneNumber;
            user.Password = userInfoDTO.Password;
            user.Gender = userInfoDTO.Gender;
            user.BirthDay = userInfoDTO.BirthDay;
            user.Introduction = userInfoDTO.Introduction;
            if (userInfoDTO.ProfilePicture != null)
            {
                // 把大頭照丟到wwwtoot的images的headshots資料夾內
                string imgPath = "../HaveFun/wwwroot/images/headshots";
                // 檔案名為帳號+檔案名
                string imgName = user.Account+userInfoDTO.ProfilePicture.FileName; // Use unique name
                string fullPath = Path.Combine(imgPath, imgName);

                // Save image to disk
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await userInfoDTO.ProfilePicture.CopyToAsync(stream);
                }

                user.ProfilePicture = fullPath; // Save path in the database
            }
            _context.UserInfos.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "用戶資料已保存成功" });
        }

        //_saveImage.Path = imgPath;
        //_saveImage.Name = imgName;
        //_saveImage.Picture = userInfoDTO.ProfilePicture;
        //string fullPath = string.Empty;
        //bool isSave = _saveImage.Save(out fullPath);
        //if (isSave == false)
        //{
        //    return new JsonResult(
        //        new
        //        {
        //            success = false,
        //            profilePictureError = "圖片存取失敗"
        //        }
        //    );
        //}
    

            
    

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
