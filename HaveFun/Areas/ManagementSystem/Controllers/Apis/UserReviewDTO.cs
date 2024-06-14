using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HaveFun.Areas.ManagementSystem.Controllers.Apis
{
	public class UserReviewDTO
	{
		public int Id { get; set; }
		public DateTime reportTime { get; set; }
		

		public int complaintCategoryId { get; set; }

		public int reportUserId { get; set; }
		
		public int beReportedUserId { get; set; }


	}
}


		