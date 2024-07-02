using HaveFun.DTOs;
using HaveFun.Models;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HaveFun.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class LuckyMatchApiController : ControllerBase
    {
        private readonly HaveFunDbContext _dbContext;

        public LuckyMatchApiController(HaveFunDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Authorize(AuthenticationSchemes = "Bearer,Cookies")]
        [HttpGet("GetCurrentUserId")]
        public IActionResult GetCurrentUserId()
        {
            string loginId = Request.Cookies["userId"];
            // 这里需要根据实际情况获取当前用户的 ID
            string currentUserId = loginId; // 假设当前用户ID为1
            return Ok(new { userId = currentUserId });
        }

        
        [HttpGet("GetRandomUser")]
        public IActionResult GetRandomUser(int currentUserId)
        {
            try
            {
                var interactedUser = _dbContext.FriendLists
             .Where(f1 => f1.BeenClicked == currentUserId && f1.state == 0)
             .Select(f1 => f1.Clicked)
             .ToList();

                var user = _dbContext.UserInfos
                    .Where(u => u.Id != currentUserId && interactedUser.Contains(u.Id))
                    .OrderBy(u => Guid.NewGuid())
                    .FirstOrDefault();

                if (user == null)
                {
                    return NotFound("No random user found");
                }

                // 使用 CreatePictureUrl 方法生成图片 URL
                var randomUser = new
                {
                    Id = user.Id,
                    Name = user.Name,
                    Gender = user.Gender,
                    BirthDay = user.BirthDay.ToShortDateString(),
                    ProfilePicture = CreatePictureUrl("GetPicture", "Profile", new { id = user.Id })
                };

                return Ok(randomUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public string CreatePictureUrl(string action, string controller, object routeValues)
        {
            // 使用 Url.Action 生成 URL
            string baseUrl = Url.Action(action, controller, routeValues, Request.Scheme);

            // Replace增加api路徑
            return baseUrl.Replace($"/{controller}/{action}", $"/api/{controller}/{action}");
        }

        [HttpGet("GetWhoLikeList")]
        public async Task<IActionResult> GetWhoLikeList()
        {
            try
            {
                string loginId = Request.Cookies["userId"];

                var whoLikeListData = await _dbContext.FriendLists
                    .Where(f => f.state == 0 && f.BeenClicked.ToString() == loginId)
                    .Select(f => f.User1.Id) // Selecting IDs of users who liked the current user
                    .ToListAsync();

                // Fetch users who are not already friends
                var usersNotFriends = await _dbContext.UserInfos
                    .Where(u => u.Id != int.Parse(loginId) && !whoLikeListData.Contains(u.Id)) // Exclude current user and existing friends
                    .Select(u => new
                    {
                        Id = u.Id,
                        Name = u.Name,
                        BirthDay = u.BirthDay.ToShortDateString(),
                        Gender = u.Gender,
                        ProfilePicture = u.ProfilePicture
                    })
                    .ToListAsync();

                var whoLikeList = usersNotFriends.Select(item => new WhoLikeListDTO
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Gender = item.Gender == 1 ? "male" : "female",
                    ProfilePicture = CreatePictureUrl("GetPicture", "Profile", new { id = item.Id })
                });

                return Ok(whoLikeList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        //private string CreatePictureUrl(string action, string controller, object routeValues)
        //{
        //    // 根据实际情况生成图片URL的方法
        //    // 这里只是一个示例，实际应用中需要根据具体的路由和参数生成图片URL
        //    return Url.Action(action, controller, routeValues);
        //}

        [HttpPost("LikeUser")]
        public IActionResult LikeUser([FromBody] data data)
        {
            try
            {
                // 检查 userData 是否为 null
                if (data == null || !data.userId.HasValue || !data.friendId.HasValue)
                {
                    return BadRequest("Invalid user data.");
                }

                int currentUserId = data.userId.Value;
                int likedUserId = data.friendId.Value;

                // 查找被喜欢的用户
                var likedUser = _dbContext.UserInfos.Find(likedUserId);
                if (likedUser == null)
                {
                    return NotFound("User not found");
                }

                // 更新好友清单中的状态为喜欢（1）
                var existingFriendship = _dbContext.FriendLists
                    .FirstOrDefault(f => f.Clicked == currentUserId && f.BeenClicked == likedUserId);

                if (existingFriendship == null)
                {
                    // 如果没有现有的好友记录，创建新记录
                    var newFriendship = new FriendList
                    {
                        Clicked = currentUserId,
                        BeenClicked = likedUserId,
                        state = 1 // 状态为喜欢
                    };
                    _dbContext.FriendLists.Add(newFriendship);
                }
                else
                {
                    // 如果存在好友记录，更新状态
                    existingFriendship.state = 1;
                }

                // 更新反向的好友记录
                var reverseFriendship = _dbContext.FriendLists
                    .FirstOrDefault(f => f.Clicked == likedUserId && f.BeenClicked == currentUserId);

                if (reverseFriendship == null)
                {
                    // 如果没有反向的好友记录，创建新记录
                    var newReverseFriendship = new FriendList
                    {
                        Clicked = likedUserId,
                        BeenClicked = currentUserId,
                        state = 1 // 状态为喜欢
                    };
                    _dbContext.FriendLists.Add(newReverseFriendship);
                }
                else
                {
                    // 如果存在反向好友记录，更新状态
                    reverseFriendship.state = 1;
                }

                // 保存更改
                _dbContext.SaveChanges();

                return Ok("User liked successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("DislikeUser")]
        public IActionResult DislikeUser([FromBody] int userId)
        {
            try
            {
                // 在这里处理不喜欢用户的逻辑，例如更新数据库中的状态为 3
                var user = _dbContext.UserInfos.Find(userId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                // 更新用户状态为不喜欢 (状态码 3)
                user.Status = 3; // 假设状态码 3 表示不喜欢
                _dbContext.SaveChanges();

                return Ok("User disliked successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
