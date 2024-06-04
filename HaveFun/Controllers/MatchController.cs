using HaveFun.Models;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
	[Route("/Match/{action=Index}")]
	public class MatchController : Controller
	{
		private readonly HaveFunDbContext _context;

		public MatchController(HaveFunDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			string[] imageUrlArr = new string[] { "~/images/pic1.jpg", "~/images/pic2.jpg", "~/images/pic3.jpg" };
			ViewBag.ImageUrlArr = imageUrlArr;
			return View();
		}
	}
}
