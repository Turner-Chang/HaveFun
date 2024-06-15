using HaveFun.Common;
using HaveFun.DTOs;
using HaveFun.Models;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
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
        SendEmail _sendEmail;
        DESSecure _desSecure;
        public LoginApiController(HaveFunDbContext dbContext, Jwt jwt, PasswordSecure passwordSecure, SendEmail sendEmail, DESSecure desSecure)
        {
            _dbContext = dbContext;
            _jwt = jwt;
            _passwordSecure = passwordSecure;
            _sendEmail = sendEmail;
            _desSecure = desSecure;
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
                byte[] salt = user.PasswordSalt;
                string password = _passwordSecure.HashPassword(userDTO.Password, salt);
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

        [HttpGet("{email}")]
        public async Task<bool> SendPasswordEmail(string email)
        {
            UserInfo? user = await _dbContext.UserInfos
                .FirstOrDefaultAsync(user => user.Account == email);
            if (user == null)
            {
                return false;
            }
            else
            {
                try
                {
                    // 設定唯一性的token跟更改密碼的連結
                    string decryptToken = $"{user.Id}/{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}";
                    string encryptToken = _desSecure.Encrypt(decryptToken);
                    string? link = Url.Action("ChangePassword", "Login", new { token = encryptToken }, protocol: HttpContext.Request.Scheme);

                    _sendEmail.emailTo = email;
                    _sendEmail.subject = "HaveFun，重置您的帳戶密碼";
                    _sendEmail.body = $@"
                                    <html>
                                    <head>
                                        <style>
                                            .container {{
                                                font-family: Arial, sans-serif;
                                                margin: 20px;
                                                padding: 20px;
                                                border: 1px solid #ddd;
                                                border-radius: 5px;
                                                max-width: 600px;
                                            }}
                                            .button {{
                                                display: inline-block;
                                                padding: 10px 20px;
                                                margin: 20px 0;
                                                font-size: 16px;
                                                color: #fff;
                                                background-color: #007bff;
                                                text-decoration: none;
                                                border-radius: 5px;
                                            }}
                                            .footer {{
                                                margin-top: 20px;
                                                font-size: 12px;
                                                color: #777;
                                            }}
                                        </style>
                                    </head>
                                    <body>
                                        <div class='container'>
                                            <h2>密碼重置通知</h2>
                                            <p>親愛的用戶，</p>
                                            <p>我們收到您重置密碼的請求。如果是您本人操作，請點擊以下按鈕以重置您的密碼：</p>
                                            <a href='{link}' class='button'>重置密碼</a>
                                            <p>如果您並未請求重置密碼，請忽略此郵件，您的帳戶信息將保持不變。</p>
                                            <div class='footer'>
                                                <p>謝謝您的支持，<br>您的公司名稱</p>
                                                <p>這是一封自動生成的郵件，請勿直接回覆。</p>
                                            </div>
                                        </div>
                                    </body>
                                    </html>
                                    ";
                    _sendEmail.Send();
                    
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }

        }
        //[HttpGet]
        //public string ResetPassword(UserResetPasswordDTO resetPasswordDTO)
        //{

        //    return "";
        //}
    }
}
