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
        [HttpGet]
        //拿資料
        public async Task<ActionResult<IEnumerable<UserIfDTO>>> GetUserInfos()
        {
            var userInf = await _context.UserInfos
            .Select(u => new UserIfDTO
            {
                Name = u.Name,
                Address = u.Address,
                PhoneNumber = u.PhoneNumber,
                Gender = u.Gender,
                BirthDay = u.BirthDay,
                Introduction = u.Introduction
            })
            .ToListAsync();

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

            };

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
