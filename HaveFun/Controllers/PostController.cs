using HaveFun.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
    public class PostController : Controller
    {
        HaveFunDbContext _context;
        public PostController(HaveFunDbContext context)
        {
            _context = context;
        }
        [Authorize(AuthenticationSchemes = "Bearer,Cookies")]

        public IActionResult Index()
        {
            var loginUser = Convert.ToInt32(Request.Cookies["userId"]);
            ViewBag.UserId = loginUser;
            return View();
        }
    }
}
