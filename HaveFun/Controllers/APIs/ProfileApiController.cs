﻿using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace HaveFun.Controllers.APIs
{
    [Route("api/Profile/[action]")]
    [ApiController]
    public class ProfileApiController : ControllerBase
    {
        HaveFunDbContext _context;
        private readonly IHostEnvironment hostEnvironment;

        //private readonly PostServices postServices;

        public ProfileApiController(HaveFunDbContext context, IHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;
        }

        // Post: api/Profile/GetUserInfor/loginUserId
        [HttpPost("{loginUserId}")]
        public async Task<IEnumerable<UserInfo>> GetUserInfor([FromRoute] int loginUserId)
        {
            var userInfo = await _context.UserInfos
                .Where(u => u.Id == loginUserId)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.ProfilePicture,
                    u.Level
                })
                .ToListAsync();

            var userInfoReturn = userInfo.Select(u => new UserInfo
            {
                Id = u.Id,
                Name = u.Name,
                ProfilePicture = CreatePictureUrl("GetPicture", "Profile", new { id = u.Id }),
                Level = u.Level
            });

            return userInfoReturn;
        }

        // GET: api/Profile/GetWhoLikeList
        [HttpGet]
        public async Task<IEnumerable<WhoLikeListDTO>> GetWhoLikeList()
        {
            string loginId = Request.Cookies["userId"];
            var whoLikeListData = await _context.FriendLists
                .Where(f => f.state == 0 && f.BeenClicked.ToString() == loginId)
                .Select(f => new
                {
                    f.User1.Id,
                    f.User1.Name,
                    f.User1.BirthDay,
                    f.User1.Gender,
                    f.User1.ProfilePicture
                })
                .ToListAsync();

            var whoLikeList = new List<WhoLikeListDTO>();
            foreach (var item in whoLikeListData)
            {
                whoLikeList.Add(new WhoLikeListDTO
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Age = CalculateAge(item.BirthDay),
                    Gender = item.Gender == 1 ? "male" : "female",
                    ProfilePicture = CreatePictureUrl("GetPicture", "Profile", new { id = item.Id })
                });
            }

            return whoLikeList;
        }

        [HttpPost]
        public async Task<ActionResult> setLikeUser(int loginUserId, int likeUserId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 找出原始 Clicked to BeenClicked 
            var LikeUser = _context.FriendLists.FirstOrDefault(f => f.Clicked == likeUserId && f.BeenClicked == loginUserId && f.state == 0);

            if (LikeUser != null)
            {
                LikeUser.state = 1; // 好友
                _context.FriendLists.Update(LikeUser);
                await _context.SaveChangesAsync();
            }

            // 寫入 BeenClicked to Clicked
            var LikeUserData = new FriendList
            {
                Clicked = loginUserId,
                BeenClicked = likeUserId,
                state = 1  // 好友
            };
            _context.FriendLists.Add(LikeUserData);
            await _context.SaveChangesAsync();

            return Ok("資料新增成功");
        }

        [HttpGet]
        public string CreatePictureUrl(string action, string controller, object routeValues)
        {
            // 使用 Url.Action 生成 URL
            string baseUrl = Url.Action(action, controller, routeValues, Request.Scheme);

            // Replace增加api路徑
            return baseUrl.Replace($"/{controller}/{action}", $"/api/{controller}/{action}");
        }

        // Get: api/Profile/GetPicture
        [HttpGet("{id}")]
        public async Task<FileResult> GetPicture([FromRoute] int id)
        {
            string path = "";
            UserInfo? user = await _context.UserInfos.FindAsync(id);
            if (string.IsNullOrEmpty(user.ProfilePicture))
            {
                path = "wwwroot\\images\\headshots\\NoHeadphoto.png";
            }
            else
            {
                path = user.ProfilePicture;
            }

            byte[] ImageContent = System.IO.File.ReadAllBytes(path);
            return File(ImageContent, "image/*");
        }

        // 計算年齡
        private static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        //GET: api/Profile/GetPostsList
        [Authorize(AuthenticationSchemes = "Bearer,Cookies")]
        [HttpGet("{userId}")]
        public async Task<IEnumerable<PostsDTO>> GetPostsList([FromRoute] string userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 5, [FromQuery] bool queryFriend = false)
        {
            string loginId = Request.Cookies["userId"]; // 取得loginId

            //todo 先判斷是否是自己，是則撈出朋友id
            // userId = loginId => loginId + friendListId
            // userId != loginId => userId

            //從資料庫取資料

            //整理資料

            //return result;
            List<string> FriendPostList = new List<string>();
            List<string> FriendBlackList = new List<string>();

            if (userId == loginId)
            {
                // 取出登入者FriendList
                var friendList = await _context.FriendLists
                    .Where(f => f.Clicked.ToString() == loginId && f.state == 1)
                    .ToListAsync();

                foreach (var friend in friendList)
                {
                    if (friend != null && friend.BeenClicked.ToString() != null)
                    {
                        FriendPostList.Add(friend.BeenClicked.ToString());
                    }
                }

                // 取出登入者FriendList-被封鎖名單
                var friendBlackList = await _context.FriendLists
                    .Where(f => f.BeenClicked.ToString() == loginId && f.state == 3)
                    .ToListAsync();

                foreach (var friend in friendBlackList)
                {
                    if (friend != null && friend.Clicked.ToString() != null)
                    {
                        FriendBlackList.Add(friend.Clicked.ToString());
                    }
                }

                // 從好友清單排除，被封鎖的User
                FriendPostList.RemoveAll(friend => friend != null && FriendBlackList.Contains(friend));

                if (FriendBlackList.Contains(userId))
                {
                    return [];
                }
            }
            else
            {
                // 取出登入者FriendList-被封鎖名單
                var friendBlackList = await _context.FriendLists
                    .Where(f => f.BeenClicked.ToString() == loginId && f.state == 3)
                    .ToListAsync();

                foreach (var friend in friendBlackList)
                {
                    if (friend != null && friend.Clicked.ToString() != null)
                    {
                        FriendBlackList.Add(friend.Clicked.ToString());
                    }
                }

                // 從好友清單排除，被封鎖的User
                FriendPostList.RemoveAll(friend => friend != null && FriendBlackList.Contains(friend));

                if (FriendBlackList.Contains(userId))
                {
                    return [];
                }
            }

            var query = _context.Posts.AsQueryable();
            if (queryFriend)
            {
                // 只搜尋朋友的貼文
                query = query.Where(p => FriendPostList.Contains(p.UserId.ToString()) && p.Status == 0);
            }
            else
            {
                // 只搜尋使用者自己的貼文
                query = query.Where(p => p.UserId.ToString() == userId && p.Status == 0);
            }

            var posts = await query
                .Where(p => (p.UserId.ToString() == userId || FriendPostList.Contains(p.UserId.ToString())) && p.Status == 0)
                .OrderByDescending(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new
                {
                    p.Id,
                    p.UserId,
                    UserName = p.User.Name,
                    UserPicture = p.User.ProfilePicture,
                    p.Contents,
                    Time = p.Time.ToString("yyyy-MM-dd HH:mm:ss"),
                    p.Pictures,
                    p.Like,
                    Likes = p.Like.Select(l => new LikeDTO
                    {
                        UserId = l.User != null ? l.User.Id : 0,
                        UserName = l.User != null ? l.User.Name : "",
                        UserPicture = l.User != null ? l.User.ProfilePicture : ""
                    }).ToList(),
                    Replies = p.Comments
                        .Where(c => c.ParentCommentId == null)
                        .Select(c => new
                        {
                            c.Id,
                            c.UserId,
                            UserName = c.User.Name,
                            UserPicture = c.User.ProfilePicture,
                            c.PostId,
                            c.ParentCommentId,
                            c.Contents,
                            Time = c.Time.ToString("yyyy-MM-dd HH:mm:ss"),
                            NestedReplies = c.Replies.Select(nc => new
                            {
                                nc.Id,
                                nc.UserId,
                                UserName = nc.User.Name,
                                UserPicture = nc.User.ProfilePicture,
                                nc.PostId,
                                nc.ParentCommentId,
                                nc.Contents,
                                Time = nc.Time.ToString("yyyy-MM-dd HH:mm:ss")
                            }).ToList()
                        }).ToList()
                }).ToListAsync();

            Console.WriteLine(posts);

            var postDTOs = posts.Select(p => new PostsDTO
            {
                Id = p.Id,
                UserId = p.UserId,
                UserName = p.UserName,
                UserPicture = CreatePictureUrl("GetPicture", "Profile", new { id = p.UserId }),
                Contents = p.Contents,
                Time = p.Time,
                PicturePath = string.IsNullOrEmpty(p.Pictures) ? "" : CreatePictureUrl("GetPostPicture", "Profile", new { id = p.Id }),
                Like = p.Like,
                LikeUserList = p.Likes.Select(l => new LikeDTO
                {
                    UserId = l.UserId,
                    UserName = l.UserName,
                    UserPicture = CreatePictureUrl("GetPicture", "Profile", new { id = l.UserId })
                }).ToList(),
                LikeCount = p.Likes.Count(),
                FreindList = FriendPostList,
                Replies = p.Replies.Select(c => new CommentsDTO
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.UserName,
                    UserPicture = CreatePictureUrl("GetPicture", "Profile", new { id = c.UserId }),
                    PostId = c.PostId,
                    ParentCommentId = c.ParentCommentId,
                    Contents = c.Contents,
                    Time = c.Time,
                    NestedReplies = c.NestedReplies.Select(nc => new CommentsDTO
                    {
                        Id = nc.Id,
                        UserId = nc.UserId,
                        UserName = nc.UserName,
                        UserPicture = CreatePictureUrl("GetPicture", "Profile", new { id = nc.UserId }),
                        PostId = nc.PostId,
                        ParentCommentId = nc.ParentCommentId,
                        Contents = nc.Contents,
                        Time = nc.Time
                    }).ToList()
                }).ToList()
            }).ToList();

            //var postDTOs = await postServices.GetPostsList(userId, loginId);

            return postDTOs;
        }

        // Get: api/Profile/GetPostPicture
        [HttpGet("{id}")]
        public async Task<FileResult> GetPostPicture([FromRoute] int id)
        {
            Post? post = await _context.Posts.FindAsync(id);
            string path = post.Pictures;
            byte[] ImageContent = System.IO.File.ReadAllBytes(path);
            return File(ImageContent, "image/*");
        }

        //POST: api/Profile/AddPost
        [HttpPost]
        public async Task<ActionResult<PostsDTO>> AddPost([FromForm] PostsDTO postDto)
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

            // 查詢會員資訊
            var userInfo = await _context.UserInfos
                   .FirstOrDefaultAsync(u => u.Id == postDto.UserId);

            if (userInfo == null)
            {
                return BadRequest("User not found");
            }

            // 保存圖片到指定目錄
            string picturePath = "";
            if (postDto.Pictures != null)
            {
                string Current = DateTime.Now.ToString("yyyyMMddHHmmss");
                string imgPath = "wwwroot\\images\\postImgs";
                string fileName = $"PostPic_{postDto.UserId}_{Current}_{postDto.Pictures.FileName}";

                var savePath = System.IO.Path.Combine(hostEnvironment.ContentRootPath, imgPath, fileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await postDto.Pictures.CopyToAsync(stream);
                }

                picturePath = $"{imgPath}/{fileName}";
            }

            var post = new Post
            {
                UserId = postDto.UserId,
                Contents = postDto.Contents,
                Time = DateTime.Now,
                Pictures = picturePath,
                Status = 0
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            postDto.Id = post.Id;
            postDto.UserName = userInfo.Name;
            postDto.UserPicture = CreatePictureUrl("GetPicture", "Profile", new { id = userInfo.Id });
            postDto.Time = post.Time.ToString("yyyy-MM-dd HH:mm:ss");
            postDto.PicturePath = string.IsNullOrEmpty(picturePath) ? "" : CreatePictureUrl("GetPostPicture", "Profile", new { id = post.Id });

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

            if (string.IsNullOrEmpty(commentDto.Contents))
            {
                return BadRequest("Comment content is empty");
            }

            if (commentDto.PostId <= 0)
            {
                return BadRequest("Invalid Post ID");
            }

            // 查詢會員資訊
            var userInfo = await _context.UserInfos
                   .FirstOrDefaultAsync(u => u.Id == commentDto.UserId);

            if (userInfo == null)
            {
                return BadRequest("User not found");
            }

            var comment = new Comment
            {
                UserId = commentDto.UserId,
                PostId = commentDto.PostId,
                ParentCommentId = commentDto.ParentCommentId,
                Contents = commentDto.Contents,
                Time = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            commentDto.Id = comment.Id;
            commentDto.UserName = userInfo.Name;
            commentDto.UserPicture = CreatePictureUrl("GetPicture", "Profile", new { id = userInfo.Id });
            commentDto.Time = comment.Time.ToString("yyyy-MM-dd HH:mm:ss");

            return CreatedAtAction(nameof(AddComment), new { id = comment.Id }, commentDto);
        }

        //新增貼文按讚
        // POST: api/Profile/AddLike
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
                    var response = new
                    {
                        State = "CancelLike",
                        UserId = clcickLike.UserId,
                        UserPicture = CreatePictureUrl("GetPicture", "Profile", new { id = clcickLike.UserId })
                    };
                    return new JsonResult(response);
                    //return new JsonResult("CancelLike");
                }
                else
                {
                    Like like = new Like
                    {
                        PostId = clcickLike.PostId,
                        UserId = clcickLike.UserId,
                    };
                    _context.Likes.Add(like);
                    await _context.SaveChangesAsync();
                    var response = new
                    {
                        State = "Like",
                        UserId = clcickLike.UserId,
                        UserPicture = CreatePictureUrl("GetPicture", "Profile", new { id = clcickLike.UserId })
                    };
                    return new JsonResult(response);

                    //return new JsonResult("Like");
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

        //查詢檢舉項目
        // GET : api/Profile/GetComplaintCategory
        [HttpGet]
        public async Task<JsonResult> GetComplaintCategory()
        {
            var result = await _context.ComplaintCategories.ToListAsync();
            return new JsonResult(result);
        }

        //檢舉貼文
        // POST: api/Profile/RatPostReview
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

        // 刪除貼文(改為下架狀態)
        // POST: api/Profile/Unpost/id
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

        [HttpGet("{id}")]
        public async Task<ActionResult<UserPicture>> GetUserPictures(int id)
        {
            try
            {
                var userPictures = await _context.UserPictures.Where(up => up.UserId == id).ToListAsync();

                if (userPictures == null || userPictures.Count == 0)
                {
                    return NotFound($"No pictures found for user with ID {id}");
                }

                return Ok(userPictures);
            }
            catch (Exception ex)
            {
                // Log the exception details here if necessary
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserPicture(int id)
        {
            try
            {
                var userPicture = await _context.UserPictures.FindAsync(id);
                if (userPicture == null)
                {
                    return NotFound($"Picture with ID {id} not found.");
                }

                _context.UserPictures.Remove(userPicture);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception details here if necessary
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }


}
