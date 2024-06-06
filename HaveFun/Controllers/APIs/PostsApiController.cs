﻿using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IQueryable<Post>> GetPosts()
        {
            return _context.Posts;
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
