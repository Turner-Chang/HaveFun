using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace HaveFun.Areas.ManagementSystem.DTOs
{
	public class LabelCategoryManageDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }

		[JsonIgnore]
		public ICollection<LabelManageDTO>? Labels { get; set; }
	}
}
