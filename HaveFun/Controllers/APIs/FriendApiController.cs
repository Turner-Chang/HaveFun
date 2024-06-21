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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFriend(int id)
        {
            var friends = await _dbContext.FriendLists
                .Where(x => x.Clicked == id)
                .Include(x => x.User2) // 加载用户信息
                .Select(x => new FriendDTO
                {
                    Id = x.User2.Id,
                    Name = x.User2.Name,
                    ProfilePicture = x.User2.ProfilePicture,
                    state = x.state
                })
                .ToListAsync();

            return Ok(friends);
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser([FromBody] int friendId)
        {
            var friend = await _dbContext.FriendLists.FirstOrDefaultAsync(x => x.User2.Id == friendId); // 使用朋友的Id来查找
            if (friend == null) return NotFound();

            friend.state = 3; // 将状态更改为封锁状态
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUser([FromBody] int friendId)
        {
            var friend = await _dbContext.FriendLists.FirstOrDefaultAsync(x => x.User2.Id == friendId); // 使用朋友的Id来查找
            if (friend == null) return NotFound();

            friend.state = 1; // 将状态更改为解锁状态
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
