using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFriend(int id)
        {
            var friendList = await _dbContext.FriendLists.Where(x => x.Clicked == id && x.state == 1).Select(x => x.Clicked == id ? x.User2 : x.User1).Select(u => new FriendDTO
            {
                UserId = id.ToString(),
                Id = u.Id.ToString(),
                Name = u.Name,
                ProfilePicture = u.ProfilePicture,
                IsBlocked = false,
                state = 1
            }).ToListAsync();

			return Ok(friendList);
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser(int userId, int friendId)
        {
            var friendRelation = await _dbContext.FriendLists
                .FirstOrDefaultAsync(x =>
                    (x.Clicked == userId && x.BeenClicked == friendId) ||
                    (x.Clicked == friendId && x.BeenClicked == userId));

            if (friendRelation == null)
            {
                return NotFound("找不到好友關係");
            }

            friendRelation.state = 2; //  2表示已封鎖
            _dbContext.FriendLists.Update(friendRelation);
            await _dbContext.SaveChangesAsync();

            return Ok("封鎖成功");
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUser(int userId, int friendId)
        {
            var friendRelation = await _dbContext.FriendLists
                .FirstOrDefaultAsync(x =>
                    (x.Clicked == userId && x.BeenClicked == friendId) ||
                    (x.Clicked == friendId && x.BeenClicked == userId));

            if (friendRelation == null)
            {
                return NotFound("找不到好友關係");
            }

            friendRelation.state = 1; // 假設 1 表示正常狀態
            _dbContext.FriendLists.Update(friendRelation);
            await _dbContext.SaveChangesAsync();

            return Ok("解除封鎖成功");
        }

      
    }
}

