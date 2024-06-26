using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
	public class MemberLevelController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
