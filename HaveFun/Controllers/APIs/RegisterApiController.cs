using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

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
        public bool HasAccount(string account)
        {
            try
            {
                UserInfo user = _dbContext.UserInfos
                        .Where(user => user.Account == account)
                        .FirstOrDefault();
                if (user == null)
                {
                    return true;
                }
                return false;
            }
            catch (DbException)
            {
                return false;
            }
        }
    }
}
