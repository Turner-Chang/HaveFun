using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Areas.ManagementSystem.Controllers
{
    [Area("ManagementSystem")]
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
