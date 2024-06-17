using System.Text.Json.Serialization;

namespace HaveFun.Areas.ManagementSystem.DTOs
{
	public class LabelManageDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CategoryId { get; set; }

		[JsonIgnore]
		public LabelCategoryManageDTO? LabelCategory { get; set; }
	}
}
