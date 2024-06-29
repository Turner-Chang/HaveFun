using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Areas.ManagementSystem.Controllers.Apis
{
    [Route("api/Login/[action]")]
    [ApiController]
    public class SessionIdapiController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            var Id= HttpContext.Session.GetString("Login");
            if (Id != null)
                return Id;
            else return "-1";
        }
		[HttpPost]
		public string Logout()
		{
			HttpContext.Session.Remove("Login");
			return "OK";
		}
	}
}