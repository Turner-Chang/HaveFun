using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers.APIs
{
    [Route("api/Posts/[action]")]
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

        //新增貼文
        // POST: api/PostsApi
        [HttpPost]
        public async Task<string> CreatePost(PostDTO postDTO)
        {
            Post p = new Post
            {
                UserId = postDTO.UserId,
                Contents = postDTO.Contents,
                Time = DateTime.UtcNow,
                Status = 0
            };
            if (postDTO.Pictures != null)
            {
                using (BinaryReader br = new BinaryReader(postDTO.Pictures.OpenReadStream()))
                {
                    // p.Pictures = br.ReadBytes((int)postDTO.Pictures.Length);
                    //byte[]最後要轉成存放位置的string @@
                }
            }
            _context.Posts.Add(p);
            await _context.SaveChangesAsync();
            return "新增貼文成功";
        }
    }
}
