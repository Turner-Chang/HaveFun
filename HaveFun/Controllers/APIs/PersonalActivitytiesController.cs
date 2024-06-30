using HaveFun.Common;
using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
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
        // GET: api/personalActivities/GetCommingupActivities/5/1
        [HttpGet("{page}/{pagePerCount}")]
        public async Task<JsonResult> GetCommingupActivities(int page, int pagePerCount)
        {
            if (!ModelState.IsValid) return new JsonResult(new { state = "前端未輸入有效值" });
            try
            {
				var commingupActivities = await _context.Activities
					.AsNoTracking()
                    .Include(user => user.ActivityParticipants)
                    .ThenInclude(member => member.User)
                    .Where(activity => activity.Status == 0 && activity.ActivityTime > DateTime.Now &&
                    (activity.UserId == User.GetUserId() ||
					activity.ActivityParticipants.Any(member => member.UserId == User.GetUserId()) 
					))
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
							Id = user.User.Id
						}).ToList()
					})
					.ToListAsync();
                if(commingupActivities != null)
                {
					return new JsonResult(new { 
						data = commingupActivities.Skip(3 * (page - 1)).Take(pagePerCount),
						totalCount = commingupActivities.Count,
                    });
				}
                else
                {
                    return new JsonResult("資料庫無該會員資料");
                }
			}
			catch (Exception ex)
			{
				return new JsonResult($"伺服器錯誤：{ex.Message}");
			}
		}
        //請求登入會員主辦的活動
        // GET: api/personalActivities/GetHostActivities/5/1
        [HttpGet("{page}/{pagePerCount}")]
        public async Task<JsonResult> GetHostActivities(int page, int pagePerCount)
        {
			if (!ModelState.IsValid) return new JsonResult(new { state = "前端未輸入有效值" });
            try
            {
				var hostActivities = await _context.Activities
                    .Include(user => user.ActivityParticipants)
                    .ThenInclude(member => member.User)
                    .Where(activity => activity.Status == 0 && activity.UserId == User.GetUserId() && activity.ActivityTime > DateTime.Now)
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
							Id = user.User.Id
						}).ToList()
					}).ToListAsync();
				
					return new JsonResult(new
                    {
                        data = hostActivities.Skip(3 * (page - 1)).Take(pagePerCount),
                        totalCount = hostActivities.Count
                    });
			}
			catch (Exception ex)
			{
				return new JsonResult($"伺服器錯誤：{ex.Message}");
			}
		}
        // 請求已結束的活動
        // GET: api/personalActivities/GetPastActivities/5/1
        [HttpGet("{page}/{pagePerCount}")]
        public async Task<JsonResult> GetPastActivities(int page, int pagePerCount)
        {
			if (!ModelState.IsValid) return new JsonResult(new { state = "前端未輸入有效值" });

            try
            {
				var pastActivities = await _context.Activities
					.AsNoTracking()
                    .Include(x => x.ActivityParticipants)
                    .ThenInclude(member => member.User)
                    .Where(act => act.Status == 0 && act.ActivityTime < DateTime.Now &&
						(act.UserId == User.GetUserId() || 
							act.ActivityParticipants.Any(member => member.UserId == User.GetUserId())))					
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
							Id = user.User.Id
						}).ToList()
					}).ToListAsync();

                return new JsonResult(new 
				{
					data = pastActivities.Skip((page - 1) * pagePerCount).Take(pagePerCount),
					totalCount = pastActivities.Count,
                });
            }
			catch (Exception ex)
			{
				return new JsonResult($"伺服器錯誤：{ex.Message}");
			}
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
		//前端發請求讀取活動資料表byte[]圖片
		// GET: api/personalActivities/GetPicture/5
		[HttpGet("{id}")]
        public async Task<FileResult> GetPicture(int id)
        {
            string Filename = Path.Combine("wwwroot", "images", "NOimg.jpg");
            var activity = await _context.Activities.FindAsync(id);
            byte[] ImageContent = activity.Picture != null? activity.Picture : System.IO.File.ReadAllBytes(Filename);
            return File(ImageContent, "image/jpeg");
        }
		//讀取使用者頭像
		// GET: api/personalActivities/GetUserProfile/5
		[HttpGet("{id}")]
        public async Task<FileResult> GetUserProfile(int id)
        {
            var user = await _context.UserInfos.FindAsync(id);
            string profilePath = user.ProfilePicture;
            byte[] ImageContent = System.IO.File.ReadAllBytes(profilePath);
            return File(ImageContent, "image/jpge");
		}
    }
}
