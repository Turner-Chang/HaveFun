using HaveFun.Common;
using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mono.TextTemplating;
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
            //測試用
            //Console.WriteLine(userRegisterDTO.Account);
            //Console.WriteLine(userRegisterDTO.Password);
            //Console.WriteLine(userRegisterDTO.Name);
            //Console.WriteLine(userRegisterDTO.Address);
            //Console.WriteLine(userRegisterDTO.Gender);
            //Console.WriteLine(userRegisterDTO.BirthDay);
            //Console.WriteLine(userRegisterDTO.PhoneNumber);
            //Console.WriteLine(userRegisterDTO.ProfilePicture == null);

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


            //圖片處理
            if(userRegisterDTO.ProfilePicture != null)
            {
                // 把大頭照丟到wwwtoot的images的headshots資料夾內
                string imgPath = "../HaveFun/wwwroot/images/headshots";

                // 檔案名為帳號+檔案名
                string imgName = userRegisterDTO.Account + userRegisterDTO.ProfilePicture.FileName;


                SaveImage saveImage = new SaveImage(imgPath, imgName, userRegisterDTO.ProfilePicture);

                bool isSave = saveImage.Save();
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
           

            return new JsonResult(
                new
                {
                    success = true
                }
            );
        }
    }
}
