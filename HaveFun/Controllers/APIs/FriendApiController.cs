using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers.APIs
{
    [Route("api/Friend/[action]")]
    [ApiController]
    public class FriendApiController : ControllerBase
    {
        HaveFunDbContext _dbContext;
        public FriendApiController(HaveFunDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{id}")]
        public async Task<ICollection<FriendList>> GetFriend(int id)
        {
            var friendList =  _dbContext.FriendLists.Where(fl => fl.Clicked == id).ToList();
            return friendList;
        }
    }
}
