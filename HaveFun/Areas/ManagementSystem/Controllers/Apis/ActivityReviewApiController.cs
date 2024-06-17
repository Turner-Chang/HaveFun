using HaveFun.Areas.ManagementSystem.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Areas.ManagementSystem.Controllers.Apis
{
    [Route("api/ActivityReview/[action]")]
	[ApiController]
	public class ActivityReviewApiController : ControllerBase
	{
		private HaveFunDbContext _context;
		public ActivityReviewApiController(HaveFunDbContext haveFunDbContext)
		{
			_context = haveFunDbContext;
		}

		[HttpGet]
		public async Task<IEnumerable<ActivityReview>> GetAll()
		{

			return _context.ActivityReviews;
		}
		[HttpGet("{id}")]
		public async Task<ActivityReview> GetReviewById(int id)
		{
			var actyivity1 = await _context.ActivityReviews.FindAsync(id);
			if (actyivity1 == null)
			{
				return null;
			}
			return actyivity1;
		}
		[HttpGet]
		public async Task<Activity> GetActById(int id) {
		var act1= await _context.Activities.FindAsync(id);
		if(act1 == null) { return null; }
			return act1;

		}
		[HttpPut]
		public async Task<string> ChangeActivityState(int id, ChangeActivityStateDTO activityDTO) { return null; }














		//測試用API
		[HttpPost]
		public async Task<string> CreateReview(ActivityReviewDTO review)
		{
			ActivityReview ActivityReview1 = new ActivityReview
			{
				ActivityId = review.ActivityId,
				ProcessingStstus = review.ProcessingStstus,


				ReportItems = review.ReportItems,
				ReportReason = review.ReportReason,
				ReportTime = review.ReportTime,
				UserId = review.UserId,
			};
			_context.ActivityReviews.Add(ActivityReview1);

			await _context.SaveChangesAsync();
			return "新增成功";
		}
		//測試用API
		[HttpPost]
		public async Task<string> CreateAct(TestActivityDTO activity) {
			Activity activity1 = new Activity { 
			Id = activity.Id,
			Content = activity.Content,
			Location = activity.Location,
			
			DeadlineTime = activity.DeadlineTime,
			InitiationTime = activity.InitiationTime,
			ActivityTime = activity.ActivityTime,
			Status = activity.Status,
			Title = activity.Title,
			UserId= activity.UserId,
			Notes = activity.Notes,
			RegistrationTime = activity.RegistrationTime,
			Amount = activity.Amount,
			MaxParticipants = activity.MaxParticipants,
			Type = activity.Type,
			Picture = activity.Picture,
			
			};
			_context.Activities.Add(activity1);
			await _context.SaveChangesAsync();
			return "新增成功";
		}
	}
}
