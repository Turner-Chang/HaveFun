using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
    public class Transaction
    {
        [Required] //設定主鍵
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //設定自動編號
        public int Id { get; set; }
        [Required]
        [ForeignKey("UserInfo")] // 外鍵
        public int UserId { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        [Required]
		[Column(TypeName = "decimal(18,4)")]
		public decimal Amount { get; set; }
        [Required]
        public int Product { get; set; }
        [Required]
        public int Method { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Status { get; set; }
        public int Type { get; set; }
    }
}
