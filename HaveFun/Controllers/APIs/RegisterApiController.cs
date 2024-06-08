using HaveFun.Common;
using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Mvc;
using Mono.TextTemplating;
using System.Data.Common;

namespace HaveFun.Controllers.APIs
{
    [Route("api/Register/[action]")]
    [ApiController]
    public class RegisterApiController : ControllerBase
    {
        // 注入
        HaveFunDbContext _dbContext;
        SaveImage _saveImage;
        PasswordSecure _passwordSecure;
        SendEmail _sendEmail;

        public RegisterApiController(HaveFunDbContext dbContext, SaveImage saveImage, PasswordSecure passwordSecure, SendEmail sendEmail)
        {
            _dbContext = dbContext;
            _saveImage = saveImage;
            _passwordSecure = passwordSecure;
            _sendEmail = sendEmail;
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
        public async Task<JsonResult> AddUser(UserRegisterDTO userRegisterDTO)
        {

            // 資料驗證沒過處理
            if (!ModelState.IsValid)
            {
                //var errors = ModelState.Where(state => state.Value.Errors.Count > 0)
                //    .ToDictionary(
                //        err => err.Key,
                //        err => err.Value.Errors.Select(msg => msg.ErrorMessage).ToArray()
                //    );
                //return new JsonResult(new { 
                //    success = false,
                //    errors
                //});
            }

            // 資料驗證過的處理

            // 密碼加密
            byte[] salt = _passwordSecure.CreateSalt();
            string strSalt = Convert.ToBase64String(salt);
            string password = userRegisterDTO.Password;
            string hashPassword = _passwordSecure.HashPassword(password, salt);
            Console.WriteLine(strSalt);
            Console.WriteLine(hashPassword);

            //圖片處理

            string fullPath = string.Empty;
            if (userRegisterDTO.ProfilePicture != null)
            {
                // 把大頭照丟到wwwtoot的images的headshots資料夾內
                string imgPath = "../HaveFun/wwwroot/images/headshots";

                // 檔案名為帳號+檔案名
                string imgName = userRegisterDTO.Account + userRegisterDTO.ProfilePicture.FileName;

                _saveImage.Path = imgPath;
                _saveImage.Name = imgName;
                _saveImage.Picture = userRegisterDTO.ProfilePicture;
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

            // 把資料放到資料庫中
            int userId = 0;
            try
            { 
                UserInfo userInfo = new UserInfo();
                userInfo.Account = userRegisterDTO.Account;
                userInfo.Password = hashPassword;
                userInfo.Name = userRegisterDTO.Name;
                userInfo.Address = userRegisterDTO.Address;
                userInfo.PhoneNumber = userRegisterDTO.PhoneNumber;
                userInfo.Gender = (int)userRegisterDTO.Gender;
                userInfo.BirthDay = (DateTime)userRegisterDTO.BirthDay;
                userInfo.ProfilePicture = fullPath;
                userInfo.PasswordSalt = strSalt;

                _dbContext.UserInfos.Add(userInfo);
                await _dbContext.SaveChangesAsync();
                userId = userInfo.Id;
            }
            catch (DbException)
            {
                return new JsonResult(
                    new
                    {
                        success = false
                    }
                );
            }
            catch (Exception)
            {
                return new JsonResult(
                    new
                    {
                        success = false
                    }
                );
            }

            return new JsonResult(
                new
                {
                    success = true,
                    id = userId,
                }
            );
        }

        // 傳送Email的Api
        [HttpGet("{id}")]
        public async Task<JsonResult> SendCheckEmail(int id)
        {
            // 寄信的內容
            string mailBody = @$"<!DOCTYPE html>
                                <html lang=""en"">
                                <head>
                                    <meta charset=""UTF-8"">
                                    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                                    <title>確認您的電子郵件</title>
                                </head>
                                <body>
                                    <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;"">
                                        <h2 style=""text-align: center; color: #007bff;"">確認您的電子郵件</h2>
                                        <p>親愛的用戶，</p>
                                        <p>感謝您註冊我們的服務！為了完成您的註冊，請點擊下方的鏈接進行確認：</p>
                                        <p style=""text-align: center;"">
                                            <a href=""https://localhost:7152/Register/Verification?id={id}"" style=""display: inline-block; padding: 10px 20px; background-color: #007bff; color: #fff; text-decoration: none;"">確認郵件</a>
                                        </p>
                                        <p>如果上面的鏈接無法點擊，請將以下地址複製到您的瀏覽器地址欄中進行訪問：</p>
                                        <p style=""text-align: center;"">https://localhost:7152/Register/Verification/id</p>
                                        <p>如果您並未註冊，請忽略此郵件。</p>
                                        <p>祝您使用愉快！</p>
                                        <p>此致，敬禮</p>
                                        <p>您的服務團隊</p>
                                    </div>
                                </body>
                                </html>
                                ";

            try
            {
                // 獲取寄信的人的email
                UserInfo? user = await _dbContext.UserInfos.FindAsync(id);
                if (user == null)
                {
                    return new JsonResult(new
                    {
                        success = false
                    });
                }

                // 寄信
                string emailTo = user.Account;
                _sendEmail.emailTo = emailTo;
                _sendEmail.body = mailBody;
                _sendEmail.subject = "HaveFun:完成註冊的確認郵件";
                _sendEmail.Send();

                return new JsonResult(new
                {
                    success = true
                });
            }
            catch (Exception)
            {

                return new JsonResult(new
                {
                    success = false
                });
            }
        }
    }
}
