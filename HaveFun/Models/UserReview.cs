using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
	public class UserReview
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public DateTime reportTime { get; set; }

		[ForeignKey("ComplaintCategory")]
		public int complaintCategoryId { get; set; }
		public virtual ComplaintCategory ComplaintCategory { get; set; }

		[ForeignKey("User1")]
		public int reportUserId { get; set; }
		public virtual UserInfo User1 { get; set; }

		[ForeignKey("User2")]
		public int beReportedUserId { get; set; }
		public virtual UserInfo User2 { get; set; }
	}
}
