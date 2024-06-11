using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaveFun.Areas.ManagementSystem.Controllers
{
	[Route("api/PostReview/[action]")]
	[ApiController]
	public class PostReviewApiController : ControllerBase
	{
		private  HaveFunDbContext _context;

		public PostReviewApiController(HaveFunDbContext context)
		{
			_context = context;
		}

		[HttpGet]

		public async Task<IEnumerable<PostReview>> GetAll()
		{
			return _context.PostReviews;
		}
		[HttpGet("{Id}")]
		public async Task<ActionResult<PostReview>> GetById(int Id)
		{
			var PostReview = await _context.PostReviews.FirstOrDefaultAsync(p => p.PostReviewId == Id);
			if (PostReview == null)
			{ return NotFound(); }
			return PostReview;
		}

		[HttpPut("{Id}")]
		public async Task<string> Put(int Id, PostReviewDTO postReviewDTO)
		{
			if (Id != postReviewDTO.PostReviewId)
			{ return "更新失敗"; }
			PostReview postReview1 = await _context.PostReviews.FirstOrDefaultAsync(p => p.PostReviewId == Id);
			postReview1.ProcessingStstus = postReviewDTO.ProcessingStstus;

			_context.Entry(postReview1).State = EntityState.Modified;
			try { await _context.SaveChangesAsync(); }
			catch (Exception e) { return "更新失敗"; }
			return "更新成功";
		}
		//public async Task<string> Delete(int Id)
		//{
		//	PostReview review = await _context.PostReviews.FindAsync(Id);
		//	if (review != null)
		//	{
		//		_context.PostReviews.Remove(review);
		//	}
		//	try
		//	{
		//		await _context.SaveChangesAsync();
		//	}
		//	catch (Exception e) { return "刪除失敗"; }
		//	return "刪除成功";
		//}

		[HttpPost]
		public async Task<string> Create(PostReviewDTO reviewDTO)
		{
			PostReview review = new PostReview
			{
				PostId = reviewDTO.PostId,
				UserId = reviewDTO.UserId,
				PostReviewId = reviewDTO.PostReviewId,
				ProcessingStstus = reviewDTO.ProcessingStstus,
				ReportItems = reviewDTO.ReportItems,
				Reason = reviewDTO.Reason,
				ReportTime = reviewDTO.ReportTime,
			};
			_context.PostReviews.Add(review);
			await _context.SaveChangesAsync();
			return "新增成功";
		}
	}
}
