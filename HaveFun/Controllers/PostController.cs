using HaveFun.Models;
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

        public IActionResult Index()
        {
            return View();
        }
    }
}
