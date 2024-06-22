using HaveFun.Common;
using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;

namespace HaveFun.Controllers.APIs
{
    [Route("api/personalActivities/[action]")]
    [ApiController]

    public class PersonalActivitytiesController : ControllerBase
    {
        HaveFunDbContext _context;
        public PersonalActivitytiesController(HaveFunDbContext context, SaveImage saveImage)
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
                .OrderBy(activity => activity.ActivityTime)
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
                .Where(activity => activity.UserId == loginUserId && activity.ActivityTime > now)
                .Where(activity => activity.Status == 0)
                .Include(user => user.ActivityParticipants)
                .ThenInclude(member => member.User)
                .OrderBy(activity => activity.ActivityTime)
                .Select(data => new
                {
                    Id = data.Id,
                    HostId = data.User.Id,
                    Host = data.User.Name,
                    Title = data.Title,
                    Type = data.ActivityType.TypeName,
                    TypeId = data.ActivityType.Id,
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
					ActivityPicture = data.Picture,
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
        // 取消參加活動
        // DELETE: api/personalActivities/NotAttending
        [HttpDelete]
        public async Task<JsonResult> NotAttending(ActivityParticipantDTO user)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(ModelState);
            }
            try
            {
                var record = await _context.ActivityParticipantes.FirstOrDefaultAsync(record => record.ActivityId == user.ActivityId && record.UserId == user.UserId);
                _context.ActivityParticipantes.Remove(record);
                await _context.SaveChangesAsync();
                return new JsonResult("退出活動成功");

            }
            catch (DbException ex)
            {
                return new JsonResult($"資料庫錯誤：{ex.Message}");
            }
            catch (Exception ex)
            {
                return new JsonResult($"伺服器錯誤：{ex.Message}");
            }
        }
        //刪除活動(狀態改成下架)
		// PUT: api/personalActivities/DeleteActivity/5
		[HttpPut("{id}")]
        public async Task<JsonResult> DeleteActivity(int id)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult (new { state = "前端未輸入有效值" });
            }
            var record = await _context.Activities.FindAsync(id);
            if (record == null)
            {
                return new JsonResult(new { result = "此活動不存在" });
            }
            record.Status = 2;
            await _context.SaveChangesAsync();
            return new JsonResult(new { result = "成功刪除活動" });
		}
        //修改活動
        // PUT: api/personalActivities/EditActivity/5
        [HttpPut("{id}")]
        public async Task<JsonResult> EditActivity(int id, [FromForm]EditActivityDTO activity)
        {
            if (id != activity.ActivityId)
            {
                return new JsonResult(new { state = "修改活動失敗" });
            }
            var record = await _context.Activities.FindAsync(id);
            var recordChange = await _context.Activities.FindAsync(id);
			if (record == null)
            {
                return new JsonResult(new { result = "此活動不存在" });
            }
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { result = "前端未輸入有效值" });
            }
            record.Title = activity.Title;
            record.UserId = activity.UserId;
            record.Id = activity.ActivityId;
            record.Content = activity.Content;
            record.Notes = activity.Notes;
            record.RegistrationTime = activity.RegistrationTime;
            record.DeadlineTime = activity.DeadlineTime;
            record.ActivityTime = activity.ActivityTime;
            record.Amount = activity.Amount;
            record.MaxParticipants = activity.MaxParticipants;
            record.Location = activity.Location;
            
            
            //用BinaryReader讀取上傳的圖片，沒有則返回null
            //如果有上傳圖片
            

			if (activity.Pictures != null && activity.Pictures.Length > 0)
			{
				var picture = activity.Pictures[0];
				if (picture.Length > 0)
				{
					using (var binaryReader = new BinaryReader(picture.OpenReadStream()))
					{
						record.Picture = binaryReader.ReadBytes((int)picture.Length);
					}
				}
			}
            try
            {
                _context.Activities.Update(record);
				await _context.SaveChangesAsync();
				return new JsonResult(new { result = "成功修改活動" });
			}
			catch(Exception ex)
            {
				return new JsonResult($"伺服器錯誤：{ex.Message}");
			}
        }
		//前端發請求讀取活動資料表byte[]
		// GET: api/personalActivities/GetPicture/5
		[HttpGet("{id}")]
        public async Task<FileResult> GetPicture(int id)
        {
            string Filename = Path.Combine("wwwroot", "images", "NOimg.jpg");
            Activity activity = await _context.Activities.FindAsync(id);
            byte[] ImageContent = activity.Picture != null? activity.Picture : System.IO.File.ReadAllBytes(Filename);
            return File(ImageContent, "image/jpeg");
        }

    }
}
