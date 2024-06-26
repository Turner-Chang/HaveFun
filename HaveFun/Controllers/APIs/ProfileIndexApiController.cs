using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Text.Json;

namespace HaveFun.Controllers.APIs
{
    [Route("api/ProfileIndex/[action]")]
    [ApiController]
    public class ProfileIndexApiController : ControllerBase
    {
        HaveFunDbContext _context;

        public ProfileIndexApiController(HaveFunDbContext context)
        {
            _context = context;
        }

        // Post:api/ProfileIndex/GetUser/userId
        [HttpPost("{userId}")]
        public async Task<IActionResult> GetUser([FromRoute] int userId)
        {
            // 先將資料從DB中取出，並暫存到記憶體
            var userInfoData = await _context.UserInfos
                .Where(u => u.Id == userId)
                .ToListAsync();

            // 從記憶體中將資料寫入到指定物件
            var userInfoReturn = userInfoData.Select(u => new
            {
                name = u.Name,
                introduction = u.Introduction,
                profilePicture = CreatePictureUrl("GetPicture", "ProfileIndex", new { id = u.Id })
            });

            return Ok(userInfoReturn);
        }

        [HttpGet]
        public string CreatePictureUrl(string action, string controller, object routeValues)
        {
            // 使用 Url.Action 生成 URL
            string baseUrl = Url.Action(action, controller, routeValues, Request.Scheme);

            // Replace增加api路徑
            return baseUrl.Replace($"/{controller}/{action}", $"/api/{controller}/{action}");
        }

        // Get: api/ProfileIndex/GetProfilePicture
        [HttpGet("{id}")]
        public async Task<FileResult> GetPicture(int id)
        {
            UserInfo? user = await _context.UserInfos.FindAsync(id);
            string path = user.ProfilePicture;
            byte[] ImageContent = System.IO.File.ReadAllBytes(path);
            return File(ImageContent, "image/*");
        }

        //"{userId = 1 , loginId = 2}"
        //data = 反序列化
        //
        // Post: api/ProfileIndex/FollowUser
        [HttpPost]
        public async Task<ActionResult> GetFollowUser(int showUserId, int loginUserId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var followData = _context.FriendLists.FirstOrDefault(f => f.Clicked == loginUserId && f.BeenClicked == showUserId && f.state == 0);

            if (followData != null)
            {
                return Content("已關注");
            }
            else
            {
                return Content("關注我");
            }
        }


        [HttpPost]
        public async Task<ActionResult> SetFollowUser(int showUserId, int loginUserId)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var followData = _context.FriendLists.FirstOrDefault(f => f.Clicked == loginUserId && f.BeenClicked == showUserId && f.state == 0);

            if (followData == null)
            {
                var followUserData = new FriendList
                {
                    Clicked = loginUserId,
                    BeenClicked = showUserId,
                    state = 0  // 關注
                };
                _context.FriendLists.Add(followUserData);
                await _context.SaveChangesAsync();
                return Content("已關注");
            }
            else
            {
                _context.FriendLists.Remove(followData);
                await _context.SaveChangesAsync();
                return Content("取消關注");
            }
        }
    }
}
