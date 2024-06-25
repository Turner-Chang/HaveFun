using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
    public class ProfileController : Controller
    {
        [Authorize(AuthenticationSchemes = "Bearer,Cookies")]
        public IActionResult Index(string? userId)
        {
            var loginUser = Convert.ToInt32(Request.Cookies["userId"]);
            ViewBag.UserId = loginUser;

            if (!string.IsNullOrEmpty(userId))
            {
                ViewBag.ShowUserId = userId;
            }
            else
            {
                ViewBag.ShowUserId = loginUser;
            }
            return View();
        }
    }
}