using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
	
	public class LoginController : Controller
    {
        [HttpGet]
		
		public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
		
		public IActionResult ForgetPassword()
        {
            return View();
        }
    }
}
