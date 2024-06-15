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
            var friendList = await _dbContext.FriendLists.ToListAsync();
            return Ok(friendList);
        }
    }
}
