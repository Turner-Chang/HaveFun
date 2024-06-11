using HaveFun.Models;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
	public class MatchController : Controller
	{
		private readonly HaveFunDbContext _context;

		public MatchController(HaveFunDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			ViewBag.UserId = 1;
			return View();
		}
	}
}
