using HaveFun.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers.APIs
{
    [Route("api/login/[action]")]
    [ApiController]
    public class LoginApiController : ControllerBase
    {
        public async Task<JsonResult> Login(UserLoginDTO user)
        {
            if (!ModelState.IsValid)
            {
            }
        }
    }
}
