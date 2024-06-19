using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HaveFun.Controllers.APIs
{
    [Route("api/personalActivities/[action]")]
    [ApiController]
    
    public class PersonalActivitytiesController : ControllerBase
    {
        HaveFunDbContext _context;
        public PersonalActivitytiesController(HaveFunDbContext context)
        {
            _context = context;
        }
        // 請求即將發生的活動
        // GET: api/personalActivities/GetCommingupActivities/5
        [HttpGet("{loginUserId}")]
        public async Task<JsonResult> GetCommingupActivities(int loginUserId)
        {
            var now = DateTime.Now;
            var commingupActivities = await _context.Activities
                .Where(activity => 
                (activity.UserId == loginUserId || 
                activity.ActivityParticipants.Any(member => member.UserId == loginUserId)) &&
                activity.ActivityTime > now)
                .Where(activity => activity.Status == 0)
                .Include(user => user.ActivityParticipants)
                .ThenInclude(member => member.User)
                .OrderByDescending(activity => activity.ActivityTime)
                .Select(data => new
                {
                    Id = data.Id,
                    HostId = data.User.Id,
                    Host = data.User.Name,
                    Title = data.Title,
                    Type = data.ActivityType.TypeName,
                    Content = data.Content,
                    Notes = data.Notes,
                    Amount = data.Amount,
                    Max = data.MaxParticipants,
                    Location = data.Location,
                    RegistrationTime = data.RegistrationTime.ToString("yyyy-MM-dd HH:mm"),
                    DeadlineTime = data.DeadlineTime.ToString("yyyy-MM-dd HH:mm"),
                    ActivityTime = data.ActivityTime.ToString("yyyy-MM-dd HH:mm"),
                    Participant = data.ActivityParticipants.Select(user => new
                    {
                        Name = user.User.Name,
                        ProfilePicture = user.User.ProfilePicture
                    }).ToList()
                }).ToListAsync();
            return new JsonResult(commingupActivities);
        }
        //請求登入會員主辦的活動
        // GET: api/personalActivities/GetHostActivities/5
        [HttpGet("{loginUserId}")]
        public async Task<JsonResult> GetHostActivities(int loginUserId)
        {
            var now = DateTime.Now;
            var hostActivities = await _context.Activities
                .Where(activity =>activity.UserId == loginUserId  && activity.ActivityTime > now)
                .Where(activity => activity.Status == 0)
                .Include(user => user.ActivityParticipants)
                .ThenInclude(member => member.User)
                .OrderByDescending(activity => activity.ActivityTime)
                .Select(data => new
                {
                    Id = data.Id,
                    HostId = data.User.Id,
                    Host = data.User.Name,
                    Title = data.Title,
                    Type = data.ActivityType.TypeName,
                    Content = data.Content,
                    Notes = data.Notes,
                    Amount = data.Amount,
                    Max = data.MaxParticipants,
                    Location = data.Location,
                    RegistrationTime = data.RegistrationTime.ToString("yyyy-MM-dd HH:mm"),
                    DeadlineTime = data.DeadlineTime.ToString("yyyy-MM-dd HH:mm"),
                    ActivityTime = data.ActivityTime.ToString("yyyy-MM-dd HH:mm"),
                    Participant = data.ActivityParticipants.Select(user => new
                    {
                        Name = user.User.Name,
                        ProfilePicture = user.User.ProfilePicture
                    }).ToList()
                }).ToListAsync();
            return new JsonResult(hostActivities);
        }
        // 請求已結束的活動
        // GET: api/personalActivities/GetPastActivities/5
        [HttpGet("{loginUserId}")]
        public async Task<JsonResult> GetPastActivities(int loginUserId)
        {
            var now = DateTime.Now;
            var pastActivities = await _context.Activities
                .Where(activity =>
                (activity.UserId == loginUserId ||
                activity.ActivityParticipants.Any(member => member.UserId == loginUserId)) &&
                activity.ActivityTime < now)
                .Where(activity => activity.Status == 0)
                .Include(user => user.ActivityParticipants)
                .ThenInclude(member => member.User)
                .OrderByDescending(activity => activity.ActivityTime)
                .Select(data => new
                {
                    Id = data.Id,
                    HostId = data.User.Id,
                    Host = data.User.Name,
                    Title = data.Title,
                    Type = data.ActivityType.TypeName,
                    Content = data.Content,
                    Notes = data.Notes,
                    Amount = data.Amount,
                    Max = data.MaxParticipants,
                    Location = data.Location,
                    RegistrationTime = data.RegistrationTime.ToString("yyyy-MM-dd HH:mm"),
                    DeadlineTime = data.DeadlineTime.ToString("yyyy-MM-dd HH:mm"),
                    ActivityTime = data.ActivityTime.ToString("yyyy-MM-dd HH:mm"),
                    Participant = data.ActivityParticipants.Select(user => new
                    {
                        Name = user.User.Name,
                        ProfilePicture = user.User.ProfilePicture
                    }).ToList()
                }).ToListAsync();
            return new JsonResult(pastActivities);
        }
        //查詢檢舉項目
        // GET : api/personalActivities/GetActivityType
        [HttpGet]
		public async Task<JsonResult> GetActivityType()
		{
			var result = await _context.ActivityTypes.ToListAsync();
			return new JsonResult(result);
		}
	}
}
