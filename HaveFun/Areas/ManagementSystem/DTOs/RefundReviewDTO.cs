using HaveFun.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace HaveFun.Areas.ManagementSystem.DTOs
{
	public class RefundReviewDTO
	{
		public int Id { get; set; }

		[ForeignKey("UserInfo")] // 外鍵
		public int UserId { get; set; }
		public virtual UserInfo UserInfo { get; set; }

		[Column(TypeName = "decimal(18,4)")]
		public decimal Amount { get; set; }

		public int Product { get; set; }

		public DateTime Date { get; set; }
		public int Status { get; set; }
        public int Method { get; set; }
    }
}
