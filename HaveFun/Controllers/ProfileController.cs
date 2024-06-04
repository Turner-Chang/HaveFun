using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
	public class ProfileController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
        public IActionResult Post()
        {

            return PartialView("_Post");
        }


        public IActionResult WhoLikes() { 
		
			return PartialView("_WhoLikes");
		}
	}
}
