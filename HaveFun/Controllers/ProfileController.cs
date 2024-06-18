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

		public IActionResult UserInfo()
		{
			return PartialView("_UserInfo");
		}

		public IActionResult WhoLikes()
		{
			return PartialView("_WhoLikes");
		}

        public IActionResult FriendList()
        {
            return PartialView("_FriendList");
        }
		
		public IActionResult ActivitysManagement()
		{
			return PartialView("_ActivitysManagement");
		}
    }
}
