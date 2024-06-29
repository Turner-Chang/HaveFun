using Google.Apis.Auth;
using HaveFun.Common;
using HaveFun.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using HaveFun.DTOs;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace HaveFun.Controllers
{
    public class RegisterController : Controller
    {
        HaveFunDbContext _dbContext;
        GoogleOAuth _googleOAuth;
        PasswordSecure _passwordSecure;
        Jwt _jwt;
        SaveImage _saveImage;

        public RegisterController(HaveFunDbContext dbContext, GoogleOAuth googleOAuth, PasswordSecure passwordSecure, Jwt jwt, SaveImage saveImage)
        {
            _dbContext = dbContext;
            _googleOAuth = googleOAuth;
            _passwordSecure = passwordSecure;
            _jwt = jwt;
            _saveImage = saveImage;
        }


        // 註冊頁面
        // Index: Register
        public IActionResult Index()
        {
            return View();
        }

        //Register/SendMail
        // 註冊後的寄信頁面
        [HttpGet("Register/SendMail/{userId}")]
        public async Task<IActionResult> SendMail(int userId) {
            try
            {
                UserInfo? userInfo = await _dbContext.UserInfos.FirstOrDefaultAsync(user => user.Id == userId);
                if (userInfo == null)
                {
                    return RedirectToAction("Index");
                }
                if (userInfo.AccountStatus == 1)
                {
                    return Redirect("/Login/Index");
                }
                return View();
            }
            catch (Exception)
            {

                return Redirect("/Login/Index");
            }
        }

        // Register/Verification/id
        // 驗證信箱完成頁面
        [HttpGet]
        [Route("Register/Verification/{id}")]
        public async Task<IActionResult> Verification(int id)
        {
            try
            {
                UserInfo? user = await _dbContext.UserInfos.FindAsync(id);
                if (user != null)
                {
                    user.AccountStatus = 1;
                    await _dbContext.SaveChangesAsync();
                    return View();
                } else
                {
                    return View("VerificationFailed");
                }

            }
            catch (Exception)
            {

                return View("VerificationFailed");
            }
        }

        public IActionResult VerificationSuccess()
        {
            return View();
        }

        public IActionResult VerificationFailed()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GoogleRegister()
        {
            try
            {
                string? formCredential = Request.Form["credential"]; //回傳憑證
                string? formToken = Request.Form["g_csrf_token"]; //回傳令牌
                string? cookiesToken = Request.Cookies["g_csrf_token"]; //Cookie 令牌

                GoogleJsonWebSignature.Payload? payload = _googleOAuth.VerifyGoogleToken(formCredential, formToken, cookiesToken).Result;
                if (payload == null)
                {
                    return Redirect("/Login");
                }
                else
                {
                    UserInfo? user = _dbContext.UserInfos.FirstOrDefault(user => user.Account == payload.Email);
                    if (user == null)
                    {
                        ViewData["Account"] = payload.Email;
                        ViewData["Name"] = payload.Name;
                        ViewData["Password"] = "Aa123456789";
                        return View();
                    }
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
                }
                return Redirect("/Profile");
            }
            catch (Exception)
            {
                return Redirect("/Login");
            }
        }

        [HttpPost("GoogleRegisterUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GoogleRegister(UserRegisterDTO user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string fullPath = string.Empty;
                    if (user.ProfilePicture != null)
                    {
                        // 把大頭照丟到wwwtoot的images的headshots資料夾內
                        string imgPath = "../HaveFun/wwwroot/images/headshots";

                        // 檔案名為帳號+檔案名
                        string imgName = user.Account + user.ProfilePicture.FileName;

                        _saveImage.Path = imgPath;
                        _saveImage.Name = imgName;
                        _saveImage.Picture = user.ProfilePicture;
                        bool isSave = _saveImage.Save(out fullPath);
                        if (isSave == false)
                        {
                            return new JsonResult(
                                new
                                {
                                    success = false,
                                    profilePictureError = "圖片存取失敗"
                                }
                            );
                        }
                    }
                    byte[] salt = _passwordSecure.CreateSalt();
                    UserInfo userInfo = new UserInfo
                    {
                        Account = user.Account,
                        Password = _passwordSecure.HashPassword(Convert.ToBase64String(salt), salt),
                        Name = user.Name,
                        Address = user.Address,
                        PhoneNumber = user.PhoneNumber,
                        Gender = (int)user.Gender,
                        BirthDay = (DateTime)user.BirthDay,
                        ProfilePicture = fullPath,
                        PasswordSalt = salt
                    };
                    _dbContext.UserInfos.Add(userInfo);
                    await _dbContext.SaveChangesAsync();
                    // 這邊設定Cookie驗證
                    var claims = new Claim[]
                    {
                        new Claim(ClaimTypes.Name, userInfo.Id.ToString())
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, "Login");
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    //這邊設定JWT驗證
                    string jwtToken = _jwt.GenerateJWTToken(userInfo);
                    Response.Cookies.Append("JwtSecret", jwtToken, new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(1),
                        HttpOnly = true,
                        IsEssential = true
                    });

                    Response.Cookies.Append("userId", Convert.ToString(userInfo.Id), new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(1),
                        HttpOnly = true,
                        IsEssential = true
                    });
                    return Redirect("/Profile");
                }
                ViewData["Account"] = user.Account;
                ViewData["Name"] = user.Name;
                ViewData["Password"] = "Aa123456789";
                return View();
            }
            catch (Exception)
            {
                return Redirect("/Login");
            }
        }
    }
}
