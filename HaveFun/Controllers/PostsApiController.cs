using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsApiController : ControllerBase
    {
        private readonly HaveFunDbContext _context;

        public PostsApiController(HaveFunDbContext context)
        {
            _context = context;
        }
        //顯示貼文
        // GET : api/PostsApi
        [HttpGet]
        public async Task<IEnumerable<Post>> GetPosts()
        {
            return _context.Posts;
        }
        
        ////新增貼文
        //[HttpPost]
        //public async Task<string> CreatePost(PostDTO postDTO)
        //{
        //    Post p = new Post();
        //    {
        //        Id = postDTO.Id,
        //        Content = postDTO.Content,
        //        Time = DateTime.Now,
        //        status = postDTO.Status
        //    }
        //}
    }
}
