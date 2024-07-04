using HaveFun.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HaveFun.Controllers
{
    public class LuckyMatchController : Controller
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

