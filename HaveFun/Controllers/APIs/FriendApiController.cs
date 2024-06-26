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
            var friendList = await _dbContext.FriendLists
                .Where(x => x.Clicked == id && x.state == 1)
                .Select(x => x.User2)
                .Select(u => new FriendDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    ProfilePicture = u.ProfilePicture,
                    IsBlocked = false, // 未封鎖
                    state = 1 // 正常狀態
                }).ToListAsync();

            return Ok(friendList);
        }

        // 取得黑名單
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlacklist(int id)
        {
            var blacklist = await _dbContext.FriendLists
                .Where(x => (x.Clicked == id || x.BeenClicked == id) && x.state == 2)
                .Select(x => x.Clicked == id ? x.User2 : x.User1)
                .Select(u => new FriendDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    ProfilePicture = u.ProfilePicture,
                    IsBlocked = true, // 已封鎖
                    state = 2 // 已封鎖狀態
                }).ToListAsync();

            return Ok(blacklist);
        }
        
        // 封鎖用戶
        [HttpPost]
        public async Task<IActionResult> BlockUser(data data)
        {
            var friendRelation = await _dbContext.FriendLists
                .FirstOrDefaultAsync(x =>
                    (x.Clicked == data.userId && x.BeenClicked == data.friendId) ||
                    (x.Clicked == data.friendId && x.BeenClicked == data.userId));

            if (friendRelation == null)
            {
                return NotFound("找不到好友關係");
            }

            friendRelation.state = 2; // 已封鎖
            _dbContext.FriendLists.Update(friendRelation);
            await _dbContext.SaveChangesAsync();

            return Ok("封鎖成功");
        }

        // 解除封鎖用戶
        [HttpPost]
        public async Task<IActionResult> UnblockUser(data data)
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
