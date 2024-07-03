using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaveFun.Controllers.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FriendApiController : ControllerBase
    {
        private readonly HaveFunDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private const string DefaultProfilePicture = "/images/headshots/noheadphoto.png";


        public FriendApiController(HaveFunDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        // 取得好友列表
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFriend(int id)
        {
            var friendList = await _dbContext.FriendLists
                .Where(x => (x.Clicked == id && x.state == 1 ))
                .Select(x => new
                {
                    Id = x.Clicked == id ? x.User2.Id : x.User1.Id,
                    Name = x.Clicked == id ? x.User2.Name : x.User1.Name,
                    ProfilePicture = x.Clicked == id ? x.User2.ProfilePicture : x.User1.ProfilePicture,
                    IsBlocked = false,
                    state = 1
                })
                .Distinct()
                .ToListAsync();

            var friendDTOList = friendList
                .GroupBy(f => f.Id)
                .Select(g => g.First())
                .Select(f => new 
                {
                    Id = f.Id,
                    Name = f.Name,
                    ProfilePicture= f.ProfilePicture,
                    IsBlocked = f.IsBlocked,
                    state = f.state
                })
                .ToList();

            var friendReturn = new List<FriendDTO>();
            foreach(var item in friendDTOList)
            {
                friendReturn.Add(new FriendDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    ProfilePicture = string.IsNullOrEmpty(item.ProfilePicture) ? DefaultProfilePicture:CreatePictureUrl("GetPicture","Profile",new { Id = item.Id }),
                    IsBlocked = item.IsBlocked,
                    state = item.state
                });
            };

            return Ok(friendReturn);
        }

        [HttpGet]
        public string CreatePictureUrl(string action, string controller, object routeValues)
        {
            // 使用 Url.Action 生成 URL
            string baseUrl = Url.Action(action, controller, routeValues, Request.Scheme);

            // Replace增加api路徑
            return baseUrl.Replace($"/{controller}/{action}", $"/api/{controller}/{action}");
        }

        // 取得黑名單
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlacklist(int id)
        {
            var blacklist = await _dbContext.FriendLists
                .Where(x => x.Clicked == id && x.state == 3)
                .Select(x => new
                {
                    Id = x.Clicked == id ? x.User2.Id : x.User1.Id,
                    Name = x.Clicked == id ? x.User2.Name : x.User1.Name,
                    ProfilePicture = x.Clicked == id ? x.User2.ProfilePicture : x.User1.ProfilePicture,
                    IsBlocked = true,
                    state = 3
                })
                .Distinct()
                .ToListAsync();

            var baseUrl = _configuration["BaseUrl"];

            var blacklistDTOList = blacklist
                .GroupBy(b => b.Id)
                .Select(g => g.First())
                .Select(b => new FriendDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    ProfilePicture = string.IsNullOrEmpty(b.ProfilePicture)? DefaultProfilePicture : CreatePictureUrl("GetPicture", "Profile", new { Id = b.Id }),
                    IsBlocked = b.IsBlocked,
                    state = b.state
                })
                .ToList();

            return Ok(blacklistDTOList);
        }

        // 封鎖用戶
        [HttpPost]
        public async Task<IActionResult> BlockUser([FromBody] data data)
        {
            var friendRelation = await _dbContext.FriendLists
                .FirstOrDefaultAsync(x =>
                    x.Clicked == data.userId && x.BeenClicked == data.friendId);

            if (friendRelation == null)
            {
                return NotFound("找不到好友關係");
            }

            friendRelation.state = 3; // 已封鎖
            _dbContext.FriendLists.Update(friendRelation);
            await _dbContext.SaveChangesAsync();

            return Ok("封鎖成功");
        }

        // 解除封鎖用戶
        [HttpPost]
        public async Task<IActionResult> UnblockUser([FromBody] data data)
        {
            var friendRelation = await _dbContext.FriendLists
                .FirstOrDefaultAsync(x =>
                    x.Clicked == data.userId && x.BeenClicked == data.friendId);

            if (friendRelation == null)
            {
                return NotFound("找不到好友關係");
            }

            friendRelation.state = 1; // 正常狀態
            _dbContext.FriendLists.Update(friendRelation);
            await _dbContext.SaveChangesAsync();

            return Ok("解除封鎖成功");
        }

    }
}