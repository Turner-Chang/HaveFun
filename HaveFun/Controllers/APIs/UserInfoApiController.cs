﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaveFun.Models;
using HaveFun.Common;
using HaveFun.DTOs;

namespace HaveFun.Controllers.APIs
{
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
            var userInf = await _context.UserInfos
            .Where(u => u.Id == id) // 使用 Where 來過濾資料
            .Select(u => new UserIfDTO
            {
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


        // GET: api/UserInfoApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfo>> GetUserInfo(int id)
        {
            var userInfo = await _context.UserInfos.FindAsync(id);

            if (userInfo == null)
            {
                return NotFound();
            }

            return userInfo;
        }
        //GET: api/UserInfo/GetPicture
        [HttpGet("GetPicture/{id}")]
        public async Task<FileResult> GetPicture(int id)
        {
            UserInfo? user = await _context.UserInfos.FindAsync(id);
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
            var userInfo = new UserInfo
            {
                Name = userInfoDTO.Name,
                Address = userInfoDTO.Address,
                PhoneNumber = userInfoDTO.PhoneNumber,
                Gender = userInfoDTO.Gender.Value,
                BirthDay = userInfoDTO.BirthDay.Value,
                Introduction = userInfoDTO.Introduction,
                Password = userInfoDTO.Password,
            };
            if (userInfoDTO.ProfilePicture != null)
            {
                // 把大頭照丟到wwwtoot的images的headshots資料夾內
                string imgPath = "../HaveFun/wwwroot/images/headshots";

                // 檔案名為帳號+檔案名
                string imgName = userInfo.Account + userInfoDTO.ProfilePicture.FileName;

                _saveImage.Path = imgPath;
                _saveImage.Name = imgName;
                _saveImage.Picture = userInfoDTO.ProfilePicture;
                string fullPath = string.Empty;
                bool isSave = _saveImage.Save(out fullPath);
                if (isSave == false)
                {
                    return new JsonResult(
                        new
                        {
                            success = false,
                            profilePictureError = "圖片存取失敗"
                        }
                    );
                }
            }

            _context.UserInfos.Add(userInfo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "用戶資料以保存成功" });
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