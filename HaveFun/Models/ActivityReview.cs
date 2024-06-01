using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
	public class ActivityReview
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ActivityReviewId { get; set; }

		[Required]
		[ForeignKey("ActId")]
		public int ActivityId {  get; set; }
		public virtual Activity ActId {  get; set; }

		[Required]
		[ForeignKey("User")]
		public int UserId { get; set; }
		public virtual UserInfo User { get; set; }

		[Required]
		[ForeignKey("CategoryId")]
		public int ReportItems {  get; set; }
		public virtual ComplaintCategory CategoryId { get; set; }


		[MaxLength(50)]
		public string ReportReason { get; set; }

		[Required]
		public DateTime ReportTime {  get; set; }

		[Required]
		public int ProcessingStstus { get; set; } = 0;


	}
}
