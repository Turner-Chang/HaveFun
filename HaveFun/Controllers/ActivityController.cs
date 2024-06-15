using HaveFun.Models;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
	public class ActivityController : Controller
	{
		private readonly HaveFunDbContext _context;

		public ActivityController(HaveFunDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			ViewBag.UserId = 2;
			return View();
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Activity activity)
		{
			return RedirectToAction("Create");
		}
	}
}
