using HaveFun.Areas.ManagementSystem.DTOs;
using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;

namespace HaveFun.Controllers.APIs
{
	[Route("api/[controller]")]
	[ApiController]
	public class ActivityApiController : ControllerBase
	{
		private readonly HaveFunDbContext _context;

		public ActivityApiController(HaveFunDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public ActionResult<IEnumerable<ActivityDTO>> GetActivities()
		{
			string fileName = Path.Combine("StaticFiles", "images", "NOimg.jpg");
			DateTime today = DateTime.Today;
			var activities = _context.Activities
			.Include(a => a.ActivityType) // 包含活動類型
			.Include(a => a.ActivityParticipants) // 包含活動參與者
			.ThenInclude(ap => ap.User) // 包含參與者的用戶信息
			.Include(a => a.User)
			.Select(a => new ActivityDTO
			{
				Id = a.Id,
				Title = a.Title,
				User = new MatchUserInfoDTO
				{
					Id = a.User.Id,
					Name = a.User.Name,
					ProfilePicture = a.User.ProfilePicture
				},
				Content = a.Content,
				Notes = a.Notes,
				Amount = a.Amount,
				MaxParticipants = a.MaxParticipants,
				Location = a.Location,
				InitiationTime = a.InitiationTime,
				RegistrationTime = a.RegistrationTime,
				DeadlineTime = a.DeadlineTime,
				ActivityTime = a.ActivityTime,
				PastDay = today.Day - a.InitiationTime.Day,
				Status = a.Status,
				ActivityTypeName = a.ActivityType.TypeName, // 活動類型名稱
				Participants = a.ActivityParticipants.Select(ap => new MatchUserInfoDTO
				{
					Id = ap.User.Id,
					Name = ap.User.Name,
					ProfilePicture = ap.User.ProfilePicture
				}).ToList() // 將參與者轉換為 UserInfoDto 列表
			}).ToList(); // 將結果轉換為列表

			// 返回 OK 結果與活動數據
			return Ok(activities);
		}

		//GET: api/Categories/GetPicture/5
		[HttpGet("GetPicture/{id}")]
		public async Task<FileResult> GetPicture(int id)
		{
			string fileName = Path.Combine("StaticFiles", "images", "NOimg.jpg");
			Activity? a = await _context.Activities.FindAsync(id);
			byte[] imageContent = a?.Picture != null ? a.Picture : System.IO.File.ReadAllBytes(fileName);
			return File(imageContent, "image/jpg");
		}

		[HttpGet("GetActivityType")]
		public ActionResult<ActivityType> GetActivityType()
		{
			var activityTypes = _context.ActivityTypes;
			return Ok(activityTypes);
		}

		[HttpPost("SignUp")]
		public async Task<IActionResult> SignUp(ActivitySignUpDTO request) 
		{
			var activity = await _context.Activities
			.Include(a => a.ActivityParticipants)
			.FirstOrDefaultAsync(a => a.Id == request.ActivityId);

			if (activity == null)
			{
				return NotFound("活动不存在" );
			}

			var isUserParticipating = activity.ActivityParticipants.Any(ap => ap.UserId == request.UserId);
			if (isUserParticipating)
			{
				return BadRequest("用户已经报名参加此活动");
			}

			if (activity.ActivityParticipants.Count >= activity.MaxParticipants)
			{
				return BadRequest("活动已经满员");
			}

			var activityParticipant = new ActivityParticipant
			{
				UserId = request.UserId,
				ActivityId = request.ActivityId,
			};

			_context.ActivityParticipantes.Add(activityParticipant);
			await _context.SaveChangesAsync();

			return Ok("报名成功");
		}
	}
}
