using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Areas.ManagementSystem.Controllers
{
	[Route("api/Post/[action]")]
	[ApiController]
	public class Review_Get_PostController : ControllerBase
	{
		private HaveFunDbContext _context;

		public  Review_Get_PostController(HaveFunDbContext context) 
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
	}
}
