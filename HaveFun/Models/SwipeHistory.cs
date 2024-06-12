using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
	public class SwipeHistory
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[ForeignKey("UserInfo")]
		public int UserId { get; set; }

		public DateTime SwipeDate { get; set; }

		public virtual UserInfo UserInfo { get; set; }
	}
}
