namespace HaveFun.DTOs
{
	public class EditActivityDTO
	{
		public int UserId { get; set; }
		public int ActivityId { get; set; }
		public int Type { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string Notes { get; set; }
		public int Amount { get; set; }
		public int MaxParticipants { get; set; }
		public string Location { get; set; }
		public DateTime RegistrationTime { get; set; }
		public DateTime DeadlineTime { get; set; }
		public DateTime ActivityTime { get; set; }
		public IFormFile[]? Pictures { get; set; }

	}
}
