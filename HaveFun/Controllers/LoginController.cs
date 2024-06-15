using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
    [Route("/Login/{action}")]
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }
    }
}
