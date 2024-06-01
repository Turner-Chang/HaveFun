using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
	public class MemberLabel
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
	
		public int UserId { get; set; }
		
		public int LabelId { get; set; }

		[ForeignKey("UserId")]
		public virtual UserInfo UserInfo { get; set; }

		[Required]
		[ForeignKey("LabelId")]
		public virtual Label Label { get; set; }
	}
}
