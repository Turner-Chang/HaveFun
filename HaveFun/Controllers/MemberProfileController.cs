using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
	public class MemberProfileController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
