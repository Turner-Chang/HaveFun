using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
	public class Label
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		[ForeignKey("LabelCategory")]
		public int CategoryId { get; set; }
		
		public virtual LabelCategory? LabelCategory { get; set; }
		public virtual ICollection<MemberLabel> MemberLabels { get; set; }
	}
}
