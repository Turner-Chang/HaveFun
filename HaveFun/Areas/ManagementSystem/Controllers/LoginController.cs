using HaveFun.Areas.ManagementSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using HaveFun.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace HaveFun.Areas.ManagementSystem.Controllers
{
    [Area("ManagementSystem")]
    public class LoginController : Controller
    {
        HaveFunDbContext _dbContext;
        public LoginController(HaveFunDbContext haveFunDbContext)
        {
            _dbContext = haveFunDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Index(ManagementLoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                ManagemenLogin? user = await _dbContext.ManagemenLogins.FirstOrDefaultAsync(user => user.Account == loginDTO.Account);
                if(user == null || user.Password != loginDTO.Password)
                {
                    ModelState.AddModelError("checkError", "帳號或密碼錯誤");
                    ViewData["error"] = "帳號密碼錯誤";
                    return View(loginDTO);
                }
                HttpContext.Session.SetString("Login", user.Id.ToString());
                return RedirectToAction("Index", "Home", new { area = "ManagementSystem" });
            }
            return View(loginDTO);
        }
    }
}
