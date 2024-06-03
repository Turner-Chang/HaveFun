using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
