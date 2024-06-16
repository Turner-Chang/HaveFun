using HaveFun.Areas.ManagementSystem.DTOs;
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
        [HttpPut]
        public async Task<string> ChangeReviewState(
            int id,UserReviewStateDTO userReviewState) {
            var review1=await _context.UserReviews.FindAsync(id);
            if (review1 == null)
            {
                return "修改失敗";
            }
            if (userReviewState.Id == review1.Id) {
                review1.status = userReviewState.status;
            }
				_context.Entry(review1).State = EntityState.Modified;
                try { 
                _context.SaveChanges();
                }
                catch(Exception ex) 
                { return "修改失敗"; }
			return "修改成功";
		}
        
    }
}
