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

        public FriendApiController(HaveFunDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // 取得好友列表
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFriend(int id)
        {
            var toFriendList = _dbContext.FriendLists.Where(x => x.BeenClicked == id);

            var friendList = await _dbContext.FriendLists
                .Where(x =>
                    (x.Clicked == id && x.state == 1)
                )
                .Select(x => new
                {
                    Id = x.Clicked == id ? x.User2.Id : x.User1.Id,
                    Name = x.Clicked == id ? x.User2.Name : x.User1.Name,
                    ProfilePicture = x.Clicked == id ? x.User2.ProfilePicture : x.User1.ProfilePicture,
                    IsBlocked = false, // 未封鎖
                    state = 1 // 正常狀態
                })
                .Distinct()
                .ToListAsync();

            var friendDTOList = friendList
                .GroupBy(f => f.Id)
                .Select(g => g.First())
                .Select(f => new FriendDTO
                {
                    Id = f.Id,
                    Name = f.Name,
                    ProfilePicture = f.ProfilePicture,
                    IsBlocked = f.IsBlocked,
                    state = f.state
                })
                .ToList();

            return Ok(friendDTOList);
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
                    IsBlocked = true, // 已封鎖
                    state = 3 // 已封鎖狀態
                })
                .Distinct()
                .ToListAsync();

            var blacklistDTOList = blacklist
                .GroupBy(b => b.Id)
                .Select(g => g.First())
                .Select(b => new FriendDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    ProfilePicture = b.ProfilePicture,
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
                    (x.Clicked == data.userId && x.BeenClicked == data.friendId) ||
                    (x.Clicked == data.friendId && x.BeenClicked == data.userId));

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
                    (x.Clicked == data.userId && x.BeenClicked == data.friendId) ||
                    (x.Clicked == data.friendId && x.BeenClicked == data.userId));

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