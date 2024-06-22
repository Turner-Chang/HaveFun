using HaveFun.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HaveFun.Controllers
{
	public class MatchController : Controller
	{
		private readonly HaveFunDbContext _context;

		private int _userId;

		public MatchController(HaveFunDbContext context)
		{
			_context = context;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);

			//檢查Cookie 是否存在並嘗試獲取其值
			if (Request.Cookies.TryGetValue("userId", out string userIdString) && int.TryParse(userIdString, out int userId))
			{
				_userId = userId;
			}
			else
			{
				_userId = -1; //默認值或其他處理
			}
		}
		[Authorize(AuthenticationSchemes = "Bearer,Cookies")]
	//	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public IActionResult Index()
		{
			ViewBag.UserId = _userId;
			return View();
		}
	}
}
