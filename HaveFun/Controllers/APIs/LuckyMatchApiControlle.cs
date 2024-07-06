using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            string currentUserId = loginId;
            return Ok(new { userId = currentUserId });
        }

        private const string DefaultProfilePicture = "/images/headshots/Noonefind.png";

        [HttpGet("GetRandomUser")]
        public IActionResult GetRandomUser(int currentUserId)
        {
            try
            {
                // 找出對當前用戶按讚 (state = 0) 但當前用戶沒有按讚 (state = 0) 的用戶
                var matchedUsers = _dbContext.FriendLists
                    .Where(f => f.BeenClicked == currentUserId && f.state == 0)
                    .Select(f => f.Clicked)
                    .ToList();

                if (!matchedUsers.Any())
                {
                    return Ok(new
                    {
                        Message = "等待按讚中",
                        ProfilePicture = DefaultProfilePicture
                    });
                }

                // 從匹配的用戶中隨機選擇一個
                var random = new Random();
                var randomUserId = matchedUsers[random.Next(matchedUsers.Count)];

                var user = _dbContext.UserInfos.Find(randomUserId);

                if (user == null)
                {
                    return Ok(new
                    {
                        Message = "等待按讚中",
                        ProfilePicture = DefaultProfilePicture
                    });
                }

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

        [HttpGet("GetWhoLikeList")]
        public async Task<IActionResult> GetWhoLikeList()
        {
            try
            {
                string loginId = Request.Cookies["userId"];

                var whoLikeListData = await _dbContext.FriendLists
                    .Where(f => f.state == 0 && f.BeenClicked.ToString() == loginId)
                    .Select(f => f.User1.Id)
                    .ToListAsync();

                var usersNotFriends = await _dbContext.UserInfos
                    .Where(u => u.Id != int.Parse(loginId) && !whoLikeListData.Contains(u.Id))
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

        [HttpPost("LikeUser")]
        public async Task<IActionResult> LikeUser([FromBody] LikeUserDTO likeUserData)
        {
            try
            {
                if (likeUserData == null || !likeUserData.UserId.HasValue || !likeUserData.FriendId.HasValue)
                {
                    return BadRequest("Invalid user data.");
                }

                int currentUserId = likeUserData.UserId.Value;
                int likedUserId = likeUserData.FriendId.Value;

                var likedUser = await _dbContext.UserInfos.FindAsync(likedUserId);
                if (likedUser == null)
                {
                    return NotFound("User not found");
                }

                var existingFriendship = await _dbContext.FriendLists
                    .FirstOrDefaultAsync(f => f.Clicked == currentUserId && f.BeenClicked == likedUserId);

                if (existingFriendship == null)
                {
                    var newFriendship = new FriendList
                    {
                        Clicked = currentUserId,
                        BeenClicked = likedUserId,
                        state = 1
                    };
                    _dbContext.FriendLists.Add(newFriendship);
                }
                else
                {
                    existingFriendship.state = 1;
                }

                var reverseFriendship = await _dbContext.FriendLists
                    .FirstOrDefaultAsync(f => f.Clicked == likedUserId && f.BeenClicked == currentUserId);

                if (reverseFriendship == null)
                {
                    var newReverseFriendship = new FriendList
                    {
                        Clicked = likedUserId,
                        BeenClicked = currentUserId,
                        state = 1
                    };
                    _dbContext.FriendLists.Add(newReverseFriendship);
                }
                else
                {
                    reverseFriendship.state = 1;
                }

                await _dbContext.SaveChangesAsync();

                // Trigger like event
                await TriggerLikeEvent(currentUserId, likedUserId);

                return Ok(new { message = "User liked successfully", isMatch = reverseFriendship?.state == 1 });
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
                var user = _dbContext.UserInfos.Find(userId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                user.Status = 2;
                _dbContext.SaveChanges();

                return Ok("User disliked successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string CreatePictureUrl(string action, string controller, object routeValues)
        {
            string baseUrl = Url.Action(action, controller, routeValues, Request.Scheme);
            return baseUrl.Replace($"/{controller}/{action}", $"/api/{controller}/{action}");
        }

        private async Task TriggerLikeEvent(int likerId, int likedUserId)
        {
            // Implement like event logic here
            // For example: send notifications, update statistics, etc.
            await Task.CompletedTask;
        }
    }

    public class LikeUserDTO
    {
        public int? UserId { get; set; }
        public int? FriendId { get; set; }
    }
}
