using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

namespace HaveFun.Controllers.APIs
{

    [Route("api/Post/[action]")]
 
    [ApiController]
    public class PostsApiController : ControllerBase
    {
        private readonly HaveFunDbContext _context;

        public PostsApiController(HaveFunDbContext context)
        {
            _context = context;
        }
        //顯示貼文
        // GET : api/Post/GetPosts
        [HttpGet]
        public async Task<IEnumerable<Post>> GetPosts()
        {
            return _context.Posts;
        }
        public async Task<IEnumerable<object>> GetPostsRel()
        {
            var result = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Select(r => new 
                { UserName = r.User.Name,
                  Contents = r.Contents,
                  Time = r.Time.ToString("yyyy-MM-dd HH:mm:ss"),
                  Picture = r.Pictures,
                  Status = r.Status,
                  Comment = r.Comments.Select(c => new
                  {
                      CommentId = c.Id,
                      ParentCommentId = c.ParentCommentId,
                      PostId = c.PostId,
                      UserId = c.User.Id,
                      UserName = c.User.Name,
                      Contents = c.Contents,
                      Time = c.Time.ToString("yyyy-MM-dd HH:mm:ss")
                  })
                })
                .ToList();
            return result;
        }
        //新增貼文 //未完成
        // POST: api/Post/CreatePost
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
