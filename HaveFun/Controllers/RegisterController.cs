using HaveFun.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaveFun.Controllers
{
	public class RegisterController : Controller
	{
		HaveFunDbContext _dbContext;
		public RegisterController(HaveFunDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        // 註冊頁面
        // Index: Register
        public IActionResult Index()
		{
			return View();
		}

		//Register/SendMail
		// 註冊後的寄信頁面
		[HttpGet("Register/SendMail/{userId}")]
        public async Task<IActionResult> SendMail(int userId) {
			try
			{
				UserInfo? userInfo = await _dbContext.UserInfos.FirstOrDefaultAsync(user => user.Id == userId);
				if (userInfo == null)
				{
					return RedirectToAction("Index");
				}
				if (userInfo.AccountStatus == 1)
				{
					return Redirect("/Login/Index");
				}
				return View();
			}
			catch (Exception)
			{

				return Redirect("/Login/Index");
            }
		}

		// Register/Verification/id
		// 驗證信箱完成頁面
		[HttpGet]
		[Route("Register/Verification/{id}")]
        public async Task<IActionResult> Verification(int id)
		{
			try
			{
				UserInfo? user = await _dbContext.UserInfos.FindAsync(id);
				if (user != null)
				{
					user.AccountStatus = 1;
					await _dbContext.SaveChangesAsync();
                    return View();
                }else
				{
                    return View("VerificationFailed");
                }
				
			}
			catch (Exception)
			{

                return View("VerificationFailed");
            }
		}

		public IActionResult VerificationSuccess()
		{
			return View();
		}

        public IActionResult VerificationFailed()
        {
            return View();
        }
    }
}
