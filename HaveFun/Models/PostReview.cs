using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
	public class PostReview
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int PostReviewId { get; set; }

		[Required]
		[ForeignKey("Post")]
		public int PostId { get; set; }
		public virtual Post Post {  get; set; }

		[Required]
		[ForeignKey("User")]
		public int UserId {  get; set; }
		public virtual UserInfo User {  get; set; }

		[ForeignKey("CategoryId")]
		[Required]
		public int ReportItems {  get; set; }
		public virtual ComplaintCategory CategoryId {  get; set; }

		[MaxLength(50)]
		public string Reason { get; set; }

		[Required]
		public DateTime ReportTime {  get; set; }

		[Required]
		public int ProcessingStstus { get; set; } = 0;

	}
}
