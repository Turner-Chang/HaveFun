using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        
        public IActionResult ForgetPasswordSuccess()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }
        public IActionResult ChangePasswordSuccess()
        {
            return View();
        }
    }
}
