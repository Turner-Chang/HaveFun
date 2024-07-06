using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaveFun.DTOs;
using HaveFun.Models;

namespace HaveFun.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class LuckyMatchApiController : ControllerBase
    {
        private readonly HaveFunDbContext _dbContext;

        public LuckyMatchApiController(HaveFunDbContext dbContext)
        {
            _dbContext = dbContext;
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
                    .Except(_dbContext.FriendLists
                        .Where(f => f.Clicked == currentUserId && f.state == 0)
                        .Select(f => f.BeenClicked))
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
                        BirthDay = u.BirthDay,
                        Gender = u.Gender,
                        ProfilePicture = u.ProfilePicture
                    })
                    .ToListAsync();

                var whoLikeList = usersNotFriends.Select(item => new WhoLikeListDTO
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Age = CalculateAge(item.BirthDay),
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

        private string CreatePictureUrl(string action, string controller, object routeValues)
        {
            string baseUrl = Url.Action(action, controller, routeValues, Request.Scheme);
            return baseUrl.Replace($"/{controller}/{action}", $"/api/{controller}/{action}");
        }

        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
