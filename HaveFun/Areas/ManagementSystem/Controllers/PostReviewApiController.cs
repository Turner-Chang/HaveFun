using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaveFun.Areas.ManagementSystem.Controllers
{
    [Route("api/PostReview/[action]")]
    [ApiController]
    public class PostReviewApiController : ControllerBase
    {
        private readonly HaveFunDbContext _context;

        public PostReviewApiController(HaveFunDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IEnumerable<PostReview>> GetPostReviews() {
            return _context.PostReviews;
        }
        [HttpGet("{Id}")]
    public async Task<ActionResult<PostReview>> GetPostReviewByPostReviewId(int Id)
    {
            var PostReview = await _context.PostReviews.FirstOrDefaultAsync(p => p.PostReviewId == Id);
            if(PostReview == null) 
            { return NotFound(); }
        return PostReview;
    }
}
}
