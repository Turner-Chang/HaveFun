using HaveFun.Common;
using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace HaveFun.Controllers.APIs
{
    [Route("api/login/[action]")]
    [ApiController]
    public class LoginApiController : ControllerBase
    {
        HaveFunDbContext _dbContext;
        Jwt _jwt;
        PasswordSecure _passwordSecure;
        public LoginApiController(HaveFunDbContext dbContext, Jwt jwt, PasswordSecure passwordSecure)
        {
            _dbContext = dbContext;
            _jwt = jwt;
            _passwordSecure = passwordSecure;
        }

        [HttpPost]
        public async Task<JsonResult> Login(UserLoginDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
            }

            UserInfo user = await _dbContext.UserInfos.Where(user => user.Account == userDTO.Account).FirstAsync();
            if (user == null)
            {
                return new JsonResult(new
                {
                    success = false,
                });
            }
            else
            {
                string salt = user.PasswordSalt;
                string password = _passwordSecure.HashPassword(userDTO.Password, Convert.FromBase64String(salt));
                if (password == user.Password)
                {
                    string jwtToken = _jwt.GenerateJWTToken(user);
                    Response.Cookies.Append("secret", jwtToken, new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(1),
                        HttpOnly = true,
                        IsEssential = true
                    });
                    Response.Cookies.Append("userId", Convert.ToString(user.Id), new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(1),
                        HttpOnly = true,
                        IsEssential = true
                    });
                    return new JsonResult(new
                    {
                        success = true,
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        success = false,
                    });
                }

            }
        }
    }
}
