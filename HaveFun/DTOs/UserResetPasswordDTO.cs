using System.ComponentModel.DataAnnotations;

namespace HaveFun.DTOs
{
    public class UserResetPasswordDTO
    {
        [Required(ErrorMessage = "帳號驗證錯誤")]
        public string EncryptToken { get; set; }

        // 密碼
        [Required(ErrorMessage = "密碼不可為空")]
        // 設定密碼一定要有一個大寫英文跟一個小寫英文，然後至少10字
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{10,20}$",
            ErrorMessage = "密碼必須包含至少一個大寫字母、一個小寫字母和一個數字，且長度在 10 到 20 個字符之間")]
        public string? Password { get; set; }
    }
}
