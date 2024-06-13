using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IEnumerable<WhoLikeListDTO>> GetWhoLikeList()
        {
            var whoLikeList = await _context.FriendLists
                .Where(fl => fl.state == 0)
                .Include(fl => fl.User1)
                .Select(fl => new WhoLikeListDTO
                {
                    Name = fl.User1.Name,
                    Age = CalculateAge(fl.User1.BirthDay),
                    Gender = fl.User1.Gender == 1 ? "male" : "female",
                    ProfilePicture = fl.User1.ProfilePicture
                })
                .ToListAsync();

            return whoLikeList;
        }

        private static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        //GET: api/Profile/GetPostsList
        [HttpGet]
        public async Task<IEnumerable<PostsDTO>> GetPostsList()
        {
            var posts = await _context.Posts
                .Where(p => p.Status == 0)
                .OrderByDescending(p => p.Id) // Id由大到小排序
                .Select(p => new PostsDTO
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    UserName = p.User.Name,
                    Contents = p.Contents,
                    Time = p.Time,
                    Pictures = p.Pictures,
                    Replies = p.Comments
                        .Where(c => c.ParentCommentId == null)
                        .Select(c => new CommentsDTO
                        {
                            Id = c.Id,
                            UserId = c.UserId,
                            UserName=c.User.Name,
                            PostId = c.PostId,
                            ParentCommentId = c.ParentCommentId,
                            Content = c.Contents,
                            Time = c.Time,
                            NestedReplies = c.Replies.Select(nc => new CommentsDTO
                            {
                                Id = nc.Id,
                                UserId = nc.UserId,
                                UserName = nc.User.Name,
                                PostId = nc.PostId,
                                ParentCommentId = nc.ParentCommentId,
                                Content = nc.Contents,
                                Time = nc.Time
                            }).ToList()
                        }).ToList()
                })
                .ToListAsync();

            return posts;
        }

        //POST: api/Profile/AddPost
        [HttpPost]
        public async Task<ActionResult<PostsDTO>> AddPost(PostsDTO postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (postDto == null)
            {
                return BadRequest("Post data is null");
            }

            if (string.IsNullOrEmpty(postDto.Contents))
            {
                return BadRequest("Post content is empty");
            }

            if (postDto.UserId <= 0)
            {
                return BadRequest("Invalid User ID");
            }

            var post = new Post
            {
                UserId = postDto.UserId,
                Contents = postDto.Contents,
                Time = DateTime.Now,
                Pictures = postDto.Pictures,
                Status = 0
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            postDto.Id = post.Id;

            return CreatedAtAction(nameof(GetPostsList), new { id = post.Id }, postDto);
        }

        // POST: api/Profile/AddComment
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentsDTO commentDto)
        {
            if (commentDto == null)
            {
                return BadRequest("Comment data is null");
            }

            if (string.IsNullOrEmpty(commentDto.Content))
            {
                return BadRequest("Comment content is empty");
            }

            if (commentDto.PostId <= 0)
            {
                return BadRequest("Invalid Post ID");
            }

            var comment = new Comment
            {
                UserId = commentDto.UserId,
                PostId = commentDto.PostId,
                ParentCommentId = commentDto.ParentCommentId,
                Contents = commentDto.Content,
                Time = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var createdComment = new Comment
            {
                UserId = comment.UserId,
                PostId = comment.PostId,
                ParentCommentId = comment.ParentCommentId,
                Contents = comment.Contents,
                Time = comment.Time
            };

            return CreatedAtAction(nameof(AddComment), new { id = createdComment.Id }, createdComment);
        }

        [HttpGet]
        // 測試用
        // GET: api/Profile/GetWhoLike
        public async Task<object[]> GetWhoLike()
        {
            var WhoLikeList = new[]{
            new {
                    Name = "Michele Storm_TEST1",
                    Age = 18,
                    Gender = "male",
                    ProfilePicture = "assets/images/member/profile/profile.jpg"
                },

            new {
                    Name = "Michele Storm_TEST2",
                    Age = 20,
                    Gender = "male",
                    ProfilePicture = "assets/images/member/profile/profile.jpg"
                }
            };
            return WhoLikeList;
        }


    }


}
