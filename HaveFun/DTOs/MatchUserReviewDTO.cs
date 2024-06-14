namespace HaveFun.DTOs
{
	public class MatchUserReviewDTO
	{
		public DateTime ReportTime { get; set; }
		public int ReportUserId { get; set; }
		public int BeReportedUserId { get; set; }
		public int ComplaintCategoryId { get; set; }
	}
}
