using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers.APIs
{
    [Route("api/Register/[action]")]
    [ApiController]
    public class RegisterApiController : ControllerBase
    {
        HaveFunDbContext _dbContext;

        public RegisterApiController(HaveFunDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // 驗證帳號是否重複
        [HttpGet("{account}")]
        public bool GetHasAccount(string account)
        {
            _dbContext
            return true;
        }
    }
}
