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
            var friendList = await _dbContext.FriendLists.Where(x => x.Clicked == id && x.state == 1).Select(x => x.User2).ToListAsync();

			return Ok(friendList);
        }

        [HttpPost("BlockUser")]
        public async Task<IActionResult> BlockUser([FromBody] int friendId)
        {
            var friend = await _dbContext.FriendLists.FindAsync(friendId);
            if (friend == null)
            {
                return NotFound("找不到好友");
            }

            friend.state = 3; // 假設 3 表示已封鎖
            _dbContext.FriendLists.Update(friend);
            await _dbContext.SaveChangesAsync();

            return Ok("封鎖成功");
        }

        [HttpPost("UnblockUser")]
        public async Task<IActionResult> UnblockUser([FromBody] int friendId)
        {
            var friend = await _dbContext.FriendLists.FindAsync(friendId);
            if (friend == null)
            {
                return NotFound("找不到好友");
            }

            friend.state = 0; // 假設 0 表示正常狀態
            _dbContext.FriendLists.Update(friend);
            await _dbContext.SaveChangesAsync();

            return Ok("解鎖成功");
        }
    }
}

