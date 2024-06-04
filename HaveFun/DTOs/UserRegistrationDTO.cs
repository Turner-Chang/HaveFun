using System.ComponentModel.DataAnnotations;

namespace HaveFun.DTOs
{
    public class UserRegistrationDTO
    {
        [StringLength(maximumLength: 100, MinimumLength = 15)]
        [Required]
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
        public string Account { get; set; }

        [Required]
        [StringLength(maximumLength: 255, MinimumLength = 10)]
        // 設定密碼一定要有一個大寫英文跟一個小寫英文，然後至少10字
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{10,20}$", 
            ErrorMessage = "密碼必須包含至少一個大寫字母、一個小寫字母和一個數字，且長度在 10 到 20 個字符之間")]
        public string Password { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 1,
           ErrorMessage = "姓名長度必須介於 1 到 50 個字之間")]
        public string Name { get; set; }

        [StringLength(maximumLength: 100, ErrorMessage = "地址長度不可超過100字")]
        public string Address { get; set; }

        [StringLength(maximumLength: 20, MinimumLength = 10,
           ErrorMessage = "電話號碼應該介於 10 到 20 個字元之間")]
        public string PhoneNumber { get; set; }

        [Required]
        public int Gender { get; set; }

        
        public IFormFile ProfilePicture { get; set; }


    }
}
