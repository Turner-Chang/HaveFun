using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
	public class ComplaintCategory
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ComplaintCategoryId { get; set; }

		[Required]
		[MaxLength(10)]
		public string ComplaintCategoryName { get; set; }

		public virtual ICollection<UserReview> UserReviews { get; set; }
	}
}
