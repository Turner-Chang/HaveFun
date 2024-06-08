using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
	public class RegisterController : Controller
	{
        // 註冊頁面
        // Index: Register
        public IActionResult Index()
		{
			return View();
		}

        //Register/SendMail
        // 註冊後的寄信頁面
        public IActionResult SendMail() { 

			return View();
		}

        //Register/Verification
        // 驗證信箱完成頁面
        public IActionResult Verification()
		{
			return View();
		}
	}
}
