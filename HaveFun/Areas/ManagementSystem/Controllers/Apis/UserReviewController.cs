using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HaveFun.Areas.ManagementSystem.Controllers.Apis
{
    [Route("api/userReview/[action]")]
    [ApiController]
    public class UserReviewController : ControllerBase
    {
        private HaveFunDbContext _context;
        public UserReviewController(HaveFunDbContext haveFunDbContext)
        {
            _context = haveFunDbContext;
        }
        [HttpGet]
        public async Task<IEnumerable<UserReview>> GetAll()
        {
            return _context.UserReviews;

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReview>> GetById(int id) { 
        var review1 = await _context.UserReviews.FindAsync(id);
        if(review1 == null)
            {
                return NotFound();
            }
        return review1;
        }
    }
}
