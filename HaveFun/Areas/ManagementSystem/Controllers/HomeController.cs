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
    }
}
