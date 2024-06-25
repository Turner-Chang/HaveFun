using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Areas.ManagementSystem.Controllers
{
	[Area("ManagementSystem")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult login()
		{
			return View();
		}
		public IActionResult Management()
		{
			return View();
		}
		public IActionResult PostManagement()
		{
			return View();
		}
		public IActionResult UserManagement()
		{
			return View();
		}
		public IActionResult LabelManage()
		{
			return View();
		}
		public IActionResult ActivityManagement()
		{
			return View();
		}
		public IActionResult RefundReview()
		{
			return View();
		}
		public IActionResult Chart()
		{

			return View();
		}
	}

}
