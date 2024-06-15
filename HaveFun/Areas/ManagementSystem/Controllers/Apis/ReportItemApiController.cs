using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace HaveFun.Areas.ManagementSystem.Controllers.Apis
{
	[Route("api/[action]")]
	[ApiController]
	public class ReportItemApiController : ControllerBase
	{ private HaveFunDbContext _context;
		public ReportItemApiController(HaveFunDbContext context) {
			_context = context; }

		[HttpGet]
		public async Task<IEnumerable<ComplaintCategory>>Getall(){

			 return  _context.ComplaintCategories;
		}

	}
}
