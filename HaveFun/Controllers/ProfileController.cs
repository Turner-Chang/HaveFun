using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
    public class ProfileController : Controller
    {
        [Authorize(AuthenticationSchemes = "Bearer,Cookies")]
		[HttpGet("Profile/{id?}")]
		public IActionResult Index(int? id)
        {
			int loginUser;
			if (id.HasValue && id > 0)
			{
				// Use the userId parameter if it's valid
				loginUser = id.Value;
			}
			else
			{
				var cookieUserId = Request.Cookies["userId"];
				loginUser = !string.IsNullOrEmpty(cookieUserId) ? Convert.ToInt32(cookieUserId) : 0;
			}
			ViewBag.UserId = loginUser;
			return View();
		}

		//public IActionResult Post()
		//{

		//	return PartialView("_Post");
		//}

		//public IActionResult UserInfo()
		//{
		//	return PartialView("_UserInfo");
		//}

		//public IActionResult WhoLikes()
		//{
		//	return PartialView("_WhoLikes");
		//}

		//      public IActionResult FriendList()
		//      {
		//          return PartialView("_FriendList");
		//      }

		//public IActionResult ActivitysManagement()
		//{
		//	return PartialView("_ActivitysManagement");
		//}
	}
}