using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace HaveFun.Service
{
    public class PostServices
    {
        private readonly HaveFunDbContext _context;

        public PostServices(HaveFunDbContext context)
        {
            this._context = context;
        }
        public async Task<List<PostsDTO>>? GetPostsList(string userId, string loginId)
        {
            //從資料庫取資料

            //整理資料

            ////return result;
            //List<string> FriendPostList = new List<string>();
            //if (userId == loginId)
            //{
            //    // 取出登入者FriendList
            //    var friendList = await _context.FriendLists
            //        .Where(f => f.Clicked.ToString() == userId && f.state == 1)
            //        .ToListAsync();

            //    foreach (var friend in friendList)
            //    {
            //        if (friend != null && friend.BeenClicked.ToString() != null)
            //        {
            //            FriendPostList.Add(friend.BeenClicked.ToString());
            //        }
            //    }
            //}
            //IList<string> test;
            //var posts = await _context.Posts
            //    .Where(p => p.UserId.ToString() == userId || FriendPostList.Contains(p.UserId.ToString()) && p.Status == 0)
            //    .OrderByDescending(p => p.Id)
            //    .Select(p => new
            //    {
            //        p.Id,
            //        p.UserId,
            //        UserName = p.User.Name,
            //        p.Contents,
            //        Time = p.Time.ToString("yyyy-MM-dd HH:mm:ss"),
            //        p.Pictures,
            //        p.Like,
            //        Replies = p.Comments
            //            .Where(c => c.ParentCommentId == null)
            //            .Select(c => new
            //            {
            //                c.Id,
            //                c.UserId,
            //                UserName = c.User.Name,
            //                c.PostId,
            //                c.ParentCommentId,
            //                c.Contents,
            //                Time = c.Time.ToString("yyyy-MM-dd HH:mm:ss"),
            //                NestedReplies = c.Replies.Select(nc => new
            //                {
            //                    nc.Id,
            //                    nc.UserId,
            //                    UserName = nc.User.Name,
            //                    nc.PostId,
            //                    nc.ParentCommentId,
            //                    nc.Contents,
            //                    Time = nc.Time.ToString("yyyy-MM-dd HH:mm:ss")
            //                }).ToList()
            //            }).ToList()
            //    })
            //    .ToListAsync();

            //var postDTOs = posts.Select(p => new PostsDTO
            //{
            //    Id = p.Id,
            //    UserId = p.UserId,
            //    UserName = p.UserName,
            //    Contents = p.Contents,
            //    Time = p.Time,
            //    PicturePath = string.IsNullOrEmpty(p.Pictures) ? "" : CreatePictureUrl("GetPostPicture", "Profile", new { id = p.Id }),
            //    Like = p.Like,
            //    FreindList = FriendPostList,
            //    Replies = p.Replies.Select(c => new CommentsDTO
            //    {
            //        Id = c.Id,
            //        UserId = c.UserId,
            //        UserName = c.UserName,
            //        PostId = c.PostId,
            //        ParentCommentId = c.ParentCommentId,
            //        Contents = c.Contents,
            //        Time = c.Time,
            //        NestedReplies = c.NestedReplies.Select(nc => new CommentsDTO
            //        {
            //            Id = nc.Id,
            //            UserId = nc.UserId,
            //            UserName = nc.UserName,
            //            PostId = nc.PostId,
            //            ParentCommentId = nc.ParentCommentId,
            //            Contents = nc.Contents,
            //            Time = nc.Time
            //        }).ToList()
            //    }).ToList()
            //}).ToList();

            return null;
        }
    }
}
