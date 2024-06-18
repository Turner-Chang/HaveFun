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
        // GET: api/personalActivities/GetActivities/5
        [HttpGet("{loginUserId}")]
        public async Task<JsonResult> GetActivities(int loginUserId)
        {
            var result = await _context.Activities
                .Where(activity => 
                activity.UserId == loginUserId || 
                activity.ActivityParticipants.Any(member => member.UserId == loginUserId))
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
            return new JsonResult(result);
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
