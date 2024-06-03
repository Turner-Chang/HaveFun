using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
	public class RegisterController : Controller
	{
		// 註冊頁面
		public IActionResult Index()
		{
			return View();
		}

		// 註冊後的寄信頁面
		public IActionResult SendMail() { 
			return View();
		}

		// 驗證信箱完成頁面
		public IActionResult Verification()
		{
			return View();
		}
	}
}
