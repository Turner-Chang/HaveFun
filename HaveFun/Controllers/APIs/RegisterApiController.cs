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

        public RegisterApiController(HaveFunDbContext dbContext, SaveImage saveImage, PasswordSecure passwordSecure)
        {
            _dbContext = dbContext;
            _saveImage = saveImage;
            _passwordSecure = passwordSecure;
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
                    success = true
                }
            );
        }
    }
}
