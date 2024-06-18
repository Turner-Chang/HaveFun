using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers.APIs
{
    [Route("api/layout/[action]")]
    [ApiController]
    public class LayoutController : ControllerBase
    {
        [Authorize(AuthenticationSchemes = "Bearer,Cookies")]
        [HttpGet]
        public string getLoginId (){
            string userId = Request.Cookies["userId"];
            if (userId == null) { return ""; }
            return userId;
        }
        [HttpGet]
        public string LogOut() {

            return "";
        }
    }
}
