using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
	public class Announcement
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(20)]
		public string Title { get; set; }

		[MaxLength(50)]
		public string Content { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }
	}
}
