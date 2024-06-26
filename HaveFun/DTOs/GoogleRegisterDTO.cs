using System.ComponentModel.DataAnnotations;

namespace HaveFun.DTOs
{
    public class GoogleRegisterDTO
    {
        // 帳號
        [Required(ErrorMessage = "帳號不可為空")]
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
        public string? Account { get; set; }

        // 密碼
        [Required(ErrorMessage = "密碼不可為空")]
        // 設定密碼一定要有一個大寫英文跟一個小寫英文，然後至少10字
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{10,20}$",
            ErrorMessage = "密碼必須包含至少一個大寫字母、一個小寫字母和一個數字，且長度在 10 到 20 個字符之間")]
        public string? Password { get; set; }

        // 暱稱
        [Required(ErrorMessage = "請輸入暱稱")]
        [StringLength(maximumLength: 50,
           ErrorMessage = "暱稱長度不能超過50字")]
        public string? Name { get; set; }

        // 地址
        [StringLength(maximumLength: 100, ErrorMessage = "地址長度不可超過100字")]
        public string? Address { get; set; }

        // 電話
        [StringLength(maximumLength: 20, MinimumLength = 10,
           ErrorMessage = "電話號碼應該介於 10 到 20 個字元之間")]
        [RegularExpression("^[\\d\\+\\-\\(\\) ]+$", ErrorMessage = "電話號碼格式不正確")]
        public string? PhoneNumber { get; set; }

        // 性別
        [Required(ErrorMessage = "請輸入性別")]
        [Range(0, 1, ErrorMessage = "請輸入正確的性別")]
        public int? Gender { get; set; }

        // 生日
        [Required(ErrorMessage = "請輸入出生年月日")]
        public DateTime? BirthDay { get; set; }

        // 圖片
        public byte[]? ProfilePicture { get; set; }

        // 圖片名
        public string? ProfilePictureName { get; set; }
    }
}
