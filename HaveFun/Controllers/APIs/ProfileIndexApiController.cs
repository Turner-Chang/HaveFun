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

            var IsFriend = _context.FriendLists.FirstOrDefault(
                f => f.Clicked == loginUserId && f.BeenClicked == showUserId && f.state == 1
                || f.Clicked == showUserId && f.BeenClicked == loginUserId && f.state == 1
            );

            if (IsFriend != null)
            {
                return Content("好友");
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

            // 檢查對方是否已關注
            var IsFocus = _context.FriendLists.FirstOrDefault(
                        f => f.Clicked == showUserId && f.BeenClicked == loginUserId && f.state == 0
            );
            
            if (IsFocus != null)
            {   
                // 對方已關注，兩人成為好友
                IsFocus.state = 1;
                _context.FriendLists.Update(IsFocus);
                await _context.SaveChangesAsync();

                var followUserData = new FriendList
                {
                    Clicked = loginUserId,
                    BeenClicked = showUserId,
                    state = 1
                };

                _context.FriendLists.Add(followUserData);
                await _context.SaveChangesAsync();
                return Content("好友");
            }

            // 檢查雙方是否為好友
            var friends = await _context.FriendLists
                .Where(f => (f.Clicked == loginUserId && f.BeenClicked == showUserId && f.state == 1)
                         || (f.Clicked == showUserId && f.BeenClicked == loginUserId && f.state == 1))
                .ToListAsync();

            if (friends != null)
            {
                foreach (var friend in friends)
                {
                    if (friend.Clicked == loginUserId)
                    {
                        friend.state = 0; // 修改狀態
                        _context.FriendLists.Update(friend); // 標記為更新
                    }
                    else
                    {
                        _context.FriendLists.Remove(friend); // 刪除
                    }
                }
                await _context.SaveChangesAsync();
            }

            // 對方未關注，加入關注
            var followData = _context.FriendLists.FirstOrDefault(f => f.Clicked == loginUserId && f.BeenClicked == showUserId && f.state == 0);

            if (followData == null)
            {
                // 關注
                var followUserData = new FriendList
                {   
                    Clicked = loginUserId,
                    BeenClicked = showUserId,
                    state = 0
                };
                _context.FriendLists.Add(followUserData);
                await _context.SaveChangesAsync();
                return Content("已關注");
            }
            else
            {   // 取消關注
                _context.FriendLists.Remove(followData);
                await _context.SaveChangesAsync();
                return Content("關注我");
            }
        }
    }
}
