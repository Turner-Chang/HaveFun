using HaveFun.Common;
using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data.Common;
using System.Text.Json.Nodes;

namespace HaveFun.Controllers.APIs
{

    [Route("api/Post/[action]")]
 
    [ApiController]
    public class PostsApiController : ControllerBase
    {
        HaveFunDbContext _context;
        SaveImage _saveImage;

        public PostsApiController(HaveFunDbContext context, SaveImage saveImage)
        {
            _context = context;
            _saveImage = saveImage;
        }
        //顯示貼文
        // GET : api/Post/GetPosts
        [HttpGet]
        public async Task<IEnumerable<Post>> GetPosts()
        {
            return _context.Posts;
        }
        //顯示貼文+回覆相關資料
        // GET : api/Post/GetPostsRel
        [HttpGet]
        public async Task<JsonResult> GetPostsRel()
        {
            var result = await _context.Posts
                .Where(p => p.Status == 0)
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.Like)
                .OrderByDescending(p => p.Time)
                .Select(r => new 
                { UserName = r.User.Name,
                  PostId = r.Id,
                  ProfilePicture = r.User.ProfilePicture,
                  Contents = r.Contents,
                  Time = r.Time.ToString("yyyy-MM-dd HH:mm:ss"),
                  Picture = r.Pictures,
                  Status = r.Status,
                  Like = r.Like,
                  Comment = r.Comments.OrderBy(c => c.Time).Select(c => new
                  {
                      CommentId = c.Id,
                      ParentCommentId = c.ParentCommentId,
                      PostId = c.PostId,
                      UserId = c.User.Id,
                      UserName = c.User.Name,
                      ProfilePicture = c.User.ProfilePicture,
                      Contents = c.Contents,
                      Time = c.Time.ToString("yyyy-MM-dd HH:mm:ss")
                  }).ToList()
                }).ToListAsync();
            return new JsonResult(result);
        }
        // 取得登入者資料
        // GET : api/Post/GetLoginUser/5
        [HttpGet("{id}")]
        public async Task<JsonResult>GetLoginUser(int id)
        {
            var userExist = await _context.UserInfos.AnyAsync(user =>  user.Id == id);
            if (!userExist)
            {
                return new JsonResult(null);
            }
            var userInfo = await _context.UserInfos
                .Where(user => user.Id == id)
                .Select(user => new
                {
                    UserName = user.Name,
                    UserId = user.Id,
                    ProfilePicture = user.ProfilePicture
                }).FirstOrDefaultAsync();
            return new JsonResult(userInfo);
        }
        //新增貼文
        //POST: api/Post/CreatePost
        [HttpPost]
        public async Task<ActionResult> CreatePost(PostDTO postDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string FullPath = string.Empty;
            if(postDTO.Pictures != null)
            {
                string Current = DateTime.Now.ToString("yyyyMMddHHmmss");
                string imgPath = "../HaveFun/wwwroot/images/postImgs";
                string imgName = postDTO.UserId + Current + postDTO.Pictures.FileName;
                _saveImage.Path = imgPath;
                _saveImage.Name = imgName;
                _saveImage.Picture = postDTO.Pictures;
                bool isSave = _saveImage.Save(out FullPath);
                if (isSave == false)
                {
                    return Content("圖片存取失敗");
                }
            }
            try
            {
                Post post = new Post
                {
                    Id = 0,
                    UserId = postDTO.UserId,
                    Contents = postDTO.Contents,
                    Time = DateTime.Now,
                    Pictures = FullPath,
                    Status = 0
                };
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
            }
            catch (DbException ex)
            {
                return Content($"資料庫錯誤：{ex.Message}");
            }
            catch (Exception ex)
            {
                return Content($"伺服器錯誤：{ex.Message}");
            }
            return Content("新增貼文成功");
        }

        //查詢檢舉項目
        // GET : api/Post/GetComplaintCategory
        [HttpGet]
        public async Task<JsonResult> GetComplaintCategory()
        {
            var result = await _context.ComplaintCategories.ToListAsync();
            return new JsonResult(result);
        }
        //檢舉貼文
        // POST: api/Post/RatPostReview
        [HttpPost]
        public async Task<JsonResult> RatPostReview(PostReviewDTO ratPost)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(ModelState);
            }
            try 
            {
                bool exist = await _context.PostReviews.AnyAsync(record => record.UserId == ratPost.UserId && record.PostId == ratPost.PostId);
                if (exist)
                {
                    return new JsonResult("此用戶已經檢舉過此貼文");
                }
                PostReview post = new PostReview
                {
                    PostReviewId = 0,
                    PostId = ratPost.PostId,
                    UserId = ratPost.UserId,
                    ReportItems = ratPost.ReportItems,
                    Reason = ratPost.Reason,
                    ReportTime = DateTime.Now,
                    ProcessingStstus = ratPost.ProcessingStstus
                };
                _context.PostReviews.Add(post);
                await _context.SaveChangesAsync();
                return new JsonResult("新增檢舉貼文成功");
            }
            catch (DbException ex)
            {
                return new JsonResult($"資料庫錯誤：{ex.Message}");
            }
            catch (Exception ex)
            {
                return new JsonResult($"伺服器錯誤：{ex.Message}");
            }
        }
        //刪除貼文(目前未使用，從資料庫整筆貼文刪掉)
        // DELETE: api/Post/DeletePost/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound("查無此貼文");
            }
            try
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return StatusCode(500,"刪除貼文失敗");
            }
            return NoContent();
        }
        //刪除貼文 狀態改成下架
        // PUT: api/Post/Unpost/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Unpost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound("查無此貼文");
            }
            post.Status = 1;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        //新增貼文按讚
        // POST: api/Post/AddLike
        [HttpPost]
        public async Task<JsonResult> AddLike(LikeDTO clcickLike)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(ModelState);
            }
            try
            {
                var record = await _context.Likes.FirstOrDefaultAsync(record => record.PostId == clcickLike.PostId && record.UserId == clcickLike.UserId);
                if (record != null)
                {
                    _context.Likes.Remove(record);
                    await _context.SaveChangesAsync();
                    return new JsonResult("取消Like");
                }
                else
                {
                    Like like = new Like
                    {
                        Id = 0,
                        PostId = clcickLike.PostId,
                        UserId = clcickLike.UserId,
                    };
                    _context.Likes.Add(like);
                    await _context.SaveChangesAsync();
                    return new JsonResult("Like成立");
                }
            }
            catch (DbException ex)
            {
                return new JsonResult($"資料庫錯誤：{ex.Message}");
            }
            catch (Exception ex)
            {
                return new JsonResult($"伺服器錯誤：{ex.Message}");
            }
        }
    }
}
