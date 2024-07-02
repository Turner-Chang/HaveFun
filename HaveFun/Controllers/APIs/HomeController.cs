using HaveFun.Common;
using HaveFun.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HaveFun.Controllers.APIs
{
	[Route("api/Home/[action]")]
	[ApiController]
	public class HomeController : ControllerBase
	{
		HaveFunDbContext _context;
		public HomeController(HaveFunDbContext context)
		{
			_context = context;
		}
		//請求現在最多人參加的活動
		// GET: api/Home/GetPopularActivity/ClientNowTime
		[HttpGet("{ClientNow}")]
		public async Task<JsonResult> GetPopularActivity(string ClientNow)
		{
			var now = DateTime.Parse(ClientNow);
			var popularActivity = await _context.Activities
				.Where(activity => activity.ActivityTime > now && activity.Status == 0)
				.Include(activity => activity.ActivityParticipants)
				.OrderByDescending(activity => activity.ActivityParticipants.Count())
				.Select(data => new
				{
					Id = data.Id,
					UserName = data.User.Name,
					UserId = data.User.Id,
					Title = data.Title,
					Time = data.ActivityTime.ToString("yyyy-MM-dd")
				}).Take(3).ToListAsync();
			return new JsonResult(popularActivity);
		}
		//請求公告
		// GET: api/Home/GetAnnouncement/ClientNowTime
		[HttpGet("{ClientNow}")]
		public async Task<JsonResult> GetAnnouncement(string ClientNow)
		{
			var now = DateTime.Parse(ClientNow);
			var announcement = await _context.Announcements
				.Where(data => data.StartTime < now && data.EndTime > now)
				.OrderByDescending (data => data.StartTime)
				.Select(announcement => new
				{
					Title = announcement.Title,
					Content = announcement.Content,
					StartTime = announcement.StartTime.ToString("yyyy-MM-dd HH:mm"),
				}).ToListAsync();
			return new JsonResult(announcement);
		}
	}

}
