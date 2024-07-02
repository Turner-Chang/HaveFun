using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HaveFun.Areas.ManagementSystem.Controllers.Apis
{
	[Route("api/Announcement/[action]")]
	[ApiController]
	public class AnnouncementApiController : ControllerBase
	{
		public HaveFunDbContext _context;
		public AnnouncementApiController(HaveFunDbContext haveFunDbContext) {
			_context = haveFunDbContext;
		}
		[HttpGet]
		public async Task<IEnumerable<Announcement>>GetAll()
		{
			  return _context.Announcements;
		}
		[HttpGet("id")]
		public async Task<ActionResult<Announcement>> Get(int id) {
		var ann =await _context.Announcements.FindAsync(id);
			if (ann==null) { return NotFound(); }
			return ann;
		}


		[HttpPost]
		public async Task<string> Create(Announcement announcement) {
			if (announcement.Title.IsNullOrEmpty() ||
				announcement.Content.IsNullOrEmpty() ||
				announcement.StartTime==null ||
				announcement.EndTime==null)	
			{
				return "新增失敗";
			}

			Announcement announcement1 = new Announcement {
				Title = announcement.Title,
				Content = announcement.Content,
				StartTime = announcement.StartTime,
				EndTime = announcement.EndTime,
			};
			_context.Announcements.Add(announcement1);
			await _context.SaveChangesAsync();
			return "新增成功";
		}

		[HttpPut]
		public async Task<string> Edit(int id ,Announcement announcement) {
			if (id!=announcement.Id) { return "修改失敗"; }
			var ann1 = await _context.Announcements.FindAsync(id);
			if (ann1 != null) {
			ann1.StartTime = announcement.StartTime;
			ann1.EndTime = announcement.EndTime;
			ann1.Content = announcement.Content;
			ann1.Title= announcement.Title;
			}
			try { 
			_context.Entry(ann1).State=EntityState.Modified;
			await _context.SaveChangesAsync();
			}catch (Exception ex)
			{
				return "修改失敗";
			}

			return "修改成功";
		}
		[HttpDelete]
		public async Task<string> Delete( int id) {

			var ann= await _context.Announcements.FindAsync( id);
			if(ann != null)
			{
				_context.Announcements.Remove(ann);
			}

			try { 
			_context.SaveChangesAsync();
			
			}catch (Exception ex) {

				return "刪除失敗";
			}

			return "刪除成功";
		}
	}
	
}
