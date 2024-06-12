using System.ComponentModel.DataAnnotations;

namespace HaveFun.DTOs
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "帳號不可為空")]
        [EmailAddress(ErrorMessage = "帳號或密碼錯誤")]
        public string? Account { get; set; }

        [Required(ErrorMessage = "密碼不可為空")]
        // 設定密碼一定要有一個大寫英文跟一個小寫英文，然後至少10字
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{10,20}$",
    ErrorMessage = "帳號或密碼錯誤")]
        public string? Password { get; set; }
    }
}
