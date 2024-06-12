using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaveFun.Areas.ManagementSystem.Controllers
{
	[Route("api/Post/[action]")]
	[ApiController]
	public class Review_GetAndPut_PostController : ControllerBase
	{
		private HaveFunDbContext _context;

		public  Review_GetAndPut_PostController(HaveFunDbContext context) 
		{
			_context=context;

		}
		[HttpGet("{id}")]
		public async Task<ActionResult<Post>> GetById(int id) {
		var Post=	await _context.Posts.FindAsync(id);
			if(Post == null) { 
		return NotFound();
			}
			
			return Post;
		}
		[HttpPut]
		public async Task<string> PutState(int id,ChangePostStateDTO changePostStateDTO ) {
			Post post1= await _context.Posts.FindAsync(id);
			if (post1 == null) { return "貼文狀態修改失敗"; }
			if(changePostStateDTO.Id == post1.Id)
			{
			post1.Status =changePostStateDTO.Status;
			}
			_context.Entry(post1).State = EntityState.Modified;
			try {
				_context.SaveChanges();
			}
			catch(Exception ex) {
				return "貼文狀態修改失敗";
			}
			
			return "貼文狀態修改成功";
		}
	}
}
