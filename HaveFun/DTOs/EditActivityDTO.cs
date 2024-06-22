using System.ComponentModel.DataAnnotations;

namespace HaveFun.DTOs
{
	public class EditActivityDTO
	{
		public int UserId { get; set; }
		public int ActivityId { get; set; }
		public int Type { get; set; }
		[MaxLength(50)]
		public string Title { get; set; }
		[MaxLength(1000)]
		public string Content { get; set; }
		[MaxLength(1000)]
		public string Notes { get; set; }
		[Range(0,int.MaxValue)]
		public int Amount { get; set; }
		[Range(1,int.MaxValue)]
		public int MaxParticipants { get; set; }
		[MaxLength(200)]
		public string Location { get; set; }
		public DateTime RegistrationTime { get; set; }
		public DateTime DeadlineTime { get; set; }
		public DateTime ActivityTime { get; set; }
		public IFormFile[]? Pictures { get; set; }

	}
}
