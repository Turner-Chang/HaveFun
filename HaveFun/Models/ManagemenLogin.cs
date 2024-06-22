using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
    public class ManagemenLogin
    {
        [Key] // 主鍵
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 設定自動編號
        public int Id { get; set; }

        [Required]
        public string Account { get; set; }

        [Required]
        [StringLength(maximumLength:20,MinimumLength =8)]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
