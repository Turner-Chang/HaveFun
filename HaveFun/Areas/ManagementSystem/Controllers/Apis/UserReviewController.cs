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
        [HttpPost]
        public async Task<string> Create(UserReviewDTO userReviewDTO ) {
            UserReview userReview1 = new UserReview { 
            
            ComplaintCategoryId = userReviewDTO.complaintCategoryId,
            BeReportedUserId=userReviewDTO.beReportedUserId,
            ReportUserId=userReviewDTO.reportUserId,
            ReportTime=userReviewDTO.reportTime,
            
            };
            _context.UserReviews.Add(userReview1);
            await _context.SaveChangesAsync();
            return "新增成功";
        } 
    }
}
