using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

        public class Member
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string Gender { get; set; }
            public string Image { get; set; }
            public bool Online { get; set; }
        }


        [HttpGet]
        // GET: api/Profile/GetWhoLike
        public async Task<string> GetWhoLike()
        {
            var members = new List<Member>{
            new Member
            {
                Name = "Michele Storm",
                Age = 32,
                Gender = "male",
                Image = "assets/images/member/profile/profile.jpg",
                Online = true
            },
        };
            //return "GetWhoLike呼叫成功";
            var json = JsonSerializer.Serialize(members);
            return json;
        }
    }
}
