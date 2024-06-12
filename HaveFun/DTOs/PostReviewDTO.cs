using HaveFun.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HaveFun.DTOs
{
    public class PostReviewDTO
    {
		public int PostReviewId { get; set; }
		public int UserId { get; set; }

		public int PostId { get; set; }
		public int ReportItems { get; set; }
		public string Reason { get; set; }
		public int ProcessingStstus { get; set; } = 0;




	}
}