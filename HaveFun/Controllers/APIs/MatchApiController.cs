using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HaveFun.Controllers.APIs
{
	[Route("api/[controller]")]
	[ApiController]
	public class MatchApiController : ControllerBase
	{
		private readonly HaveFunDbContext _context;
		public MatchApiController(HaveFunDbContext context)
		{
			_context = context;
		}

		[HttpGet("{userId}")]
		public IEnumerable<MatchUserInfoDTO> GetNotMatchUser(int userId) 
		{
			var interactedUser = _context.FriendLists
				.Where(f1 => f1.Clicked == userId)
				.Select(f1 => f1.BeenClicked);

			var usersNotInteractedWith = _context.UserInfos
				.Where(u => u.Id != userId && !interactedUser.Contains(u.Id))
				.Select(u => new MatchUserInfoDTO
				{
					Id = u.Id,
					Name = u.Name,
					Address = u.Address,
					Gender = u.Gender,
					BirthDay = u.BirthDay,
					ProfilePicture = u.ProfilePicture,
					Introduction = u.Introduction,
					Level = u.Level,
					Labels = u.MemberLabels.Select(m1 => new MatchLabelDTO
					{
						Id = m1.Label.Id,
						Name = m1.Label.Name,
						Category = new MatchLabelCategoryDTO
						{
							Id = m1.Label.LabelCategory.Id,
							Name = m1.Label.LabelCategory.Name
						}
					}).ToList()
				}).ToList() ;

			return usersNotInteractedWith;
		}
	}
}
