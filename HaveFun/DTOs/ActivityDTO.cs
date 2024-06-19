using HaveFun.Areas.ManagementSystem.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.DTOs
{
	public class ActivityDTO
	{
		public int Id { get; set; }

		public MatchUserInfoDTO User { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string Notes { get; set; }
		public int Amount { get; set; } 
		public int MaxParticipants { get; set; }
		public string Location { get; set; } 
		public DateTime InitiationTime { get; set; }
		public DateTime RegistrationTime { get; set; }
		public DateTime DeadlineTime { get; set; }
		public DateTime ActivityTime { get; set; }

		public int PastDay { get; set; }
		public int Status { get; set; }
		public byte[] Picture { get; set; }
		public string ActivityTypeName { get; set; } 
		public List<MatchUserInfoDTO> Participants { get; set; } 
	}
}
