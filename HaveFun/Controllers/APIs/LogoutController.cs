using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers.APIs
{
	[Route("api/logout/[action]")]
	[ApiController]
	public class LogoutApiController : ControllerBase
	{
		public LogoutApiController()
		{
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			// Sign out from Cookie Authentication
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			// Remove JWT token cookie if it exists
			if (Request.Cookies.ContainsKey("JwtSecret"))
			{
				Response.Cookies.Delete("JwtSecret");
			}

			// Remove user ID cookie if it exists
			if (Request.Cookies.ContainsKey("userId"))
			{
				Response.Cookies.Delete("userId");
			}

			return new JsonResult(new
			{
				success = true,
			});
		}
	}
}