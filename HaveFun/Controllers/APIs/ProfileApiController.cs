using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers.APIs
{
    [Route("api/Profile/[action]")]
    [ApiController]
    public class ProfileApiController : ControllerBase
    {
        HaveFunDbContext _context;

        public ProfileApiController(HaveFunDbContext context)
        {
            _context = context;
        }

        // GET: api/Profile/GetWhoLikeList
        [HttpGet]
        public async Task<IEnumerable<FriendList>> GetWhoLikeList()
        {
            return _context.FriendLists.Where(c => c.state == 0);

        }

        [HttpGet]
        // GET: api/Profile/GetWhoLike
        public async Task<string> GetWhoLike()
        {
            return "GetWhoLike呼叫成功";
        }
    }
}
