using HaveFun.DTOs;
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
                UserInfo? user = _dbContext.UserInfos
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

        // 把前端資料處理並傳到資料庫
        [HttpPost]
        public JsonResult AddUser(UserRegisterDTO userRegisterDTO)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(
                new
                {
                    success = false
                }
            );
            }
            Console.WriteLine(userRegisterDTO.Account);
            Console.WriteLine(userRegisterDTO.Password);
            Console.WriteLine(userRegisterDTO.Name);
            Console.WriteLine(userRegisterDTO.Address);
            Console.WriteLine(userRegisterDTO.Gender);
            Console.WriteLine(userRegisterDTO.BirthDay);
            Console.WriteLine(userRegisterDTO.PhoneNumber);
            Console.WriteLine(userRegisterDTO.ProfilePicture == null);
            return new JsonResult(
                new
                {
                    success = true
                }
            );
        }
    }
}
