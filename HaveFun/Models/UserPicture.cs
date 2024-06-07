using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
	public class UserPicture
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[ForeignKey("UserInfo")]
		public int UserId { get; set; }

		public virtual UserInfo UserInfo { get; set; }

		public string? Picture { get; set; }
	}
}
