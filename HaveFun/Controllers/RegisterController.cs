using HaveFun.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult SendMail() { 

			return View();
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
