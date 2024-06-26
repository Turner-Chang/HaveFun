using System.ComponentModel.DataAnnotations;

namespace HaveFun.Areas.ManagementSystem.DTOs
{
    public class ManagementLoginDTO
    {
        [Required(ErrorMessage = "請輸入帳號")]
        public string? Account { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        public string? Password { get; set; }
    }
}
