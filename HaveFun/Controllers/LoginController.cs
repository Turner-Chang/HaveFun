using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
