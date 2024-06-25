using HaveFun.Common;
using HaveFun.DTOs;
using HaveFun.Models;
using Humanizer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Security.Claims;
using System.Text;
using NuGet.Common;
using Google.Apis.Auth;

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
        GoogleOAuth _googleOAuth;
        public LoginApiController(HaveFunDbContext dbContext, Jwt jwt, PasswordSecure passwordSecure, SendEmail sendEmail, DESSecure desSecure, GoogleOAuth googleOAuth)
        {
            _dbContext = dbContext;
            _jwt = jwt;
            _passwordSecure = passwordSecure;
            _sendEmail = sendEmail;
            _desSecure = desSecure;
            _googleOAuth = googleOAuth;
        }

        [HttpPost]
        public async Task<JsonResult> Login(UserLoginDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
            }

            UserInfo? user = await _dbContext.UserInfos.Where(user => user.Account == userDTO.Account).FirstOrDefaultAsync();
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
                    // 這邊設定Cookie驗證
                    var claims = new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString())
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, "Login");
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    //這邊設定JWT驗證
                    string jwtToken = _jwt.GenerateJWTToken(user);
                    Response.Cookies.Append("JwtSecret", jwtToken, new CookieOptions
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
                    if(user.AccountStatus == 0)
                    {
                        string? link = Url.Action("SendMail", "Register", new { id = user.Id }, protocol: HttpContext.Request.Scheme);
                        return new JsonResult(new
                        {
                            success = true,
                            emailLink = link,
                            id = user.Id
                        });
                    }
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
                    string decryptToken = $"{user.Id}|{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}";
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
                    HttpContext.Session.SetString(encryptToken, user.Id.ToString());
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }

        }

        [HttpPost]
        public async Task<bool> CheckToken([FromBody] string token)
        {
            try
            {
                if (token == null)
                {
                    return false;
                }

                string[] encryptToken = _desSecure.Decrypt(token).Split('|');
                int userId = Convert.ToInt32(encryptToken[0]); 
                DateTime tokenDate = Convert.ToDateTime(encryptToken[1]);
                TimeSpan time = DateTime.Now - tokenDate;
                string sessionToken = HttpContext.Session.GetString(token);
                // 驗證session的值是否正確
                if (sessionToken != userId.ToString() || string.IsNullOrEmpty(sessionToken))
                {
                    HttpContext.Session.Remove(token);
                    return false;
                }

                // 驗證時間是否超過30分鐘
                if (time > TimeSpan.FromMinutes(30))
                {
                    HttpContext.Session.Remove(token);
                    return false;
                }
                UserInfo? user = await _dbContext.UserInfos.FindAsync(userId);
                if (user == null)
                {
                    HttpContext.Session.Remove(token);
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                HttpContext.Session.Remove(token);
                return false;
            }

        }

        [HttpPost]
        public async Task<string> ResetPassword(UserResetPasswordDTO resetPasswordDTO)
        {
            string token = resetPasswordDTO.EncryptToken;
            string? sessionToken = HttpContext.Session.GetString(token);
            if (string.IsNullOrEmpty(sessionToken))
            {
                return "驗證錯誤，請重新重置密碼";
            }

            if (!ModelState.IsValid)
            {
                
            }

            try
            {
                // 獲取token並解密
                string[] encryptToken = _desSecure.Decrypt(resetPasswordDTO.EncryptToken).Split('|');
                
                // 獲取userId跟token產生時間
                int userId = Convert.ToInt32(encryptToken[0]);
                DateTime tokenDate = Convert.ToDateTime(encryptToken[1]);

                if(sessionToken != userId.ToString())
                {
                    HttpContext.Session.Remove(token);
                    return "驗證錯誤，請重新重置密碼";
                }

                //比對時間，如果超過30分鐘，就傳錯誤
                TimeSpan time = DateTime.Now - tokenDate;
                if (time > TimeSpan.FromMinutes(30))
                {
                    HttpContext.Session.Remove(token);
                    return "已超過時間限制，請重新重置密碼";
                }

                // 更改密碼
                UserInfo? user = await _dbContext.UserInfos.FindAsync(userId);
                if (user == null)
                {
                    HttpContext.Session.Remove(token);
                    return "找不到使用者，請確認帳號是否正確";
                }
                byte[] salt = _passwordSecure.CreateSalt();
                user.PasswordSalt = salt;
                user.Password = _passwordSecure.HashPassword(resetPasswordDTO.Password, salt);
                await _dbContext.SaveChangesAsync();
                HttpContext.Session.Remove(token);
                return "true";
            }
            catch (Exception)
            {
                HttpContext.Session.Remove(token);
                return "驗證錯誤，請重新重置密碼"; 
            }
        }
    }
}
