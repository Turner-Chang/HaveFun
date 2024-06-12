using HaveFun.Common;
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
		private readonly MembershipService _membershipService;
		public MatchApiController(HaveFunDbContext context, MembershipService membershipService)
		{
			_context = context;
			_membershipService = membershipService;
		}

		[HttpGet("{userId}")]
		public IEnumerable<MatchUserInfoDTO> GetNotMatchUser(int userId) 
		{
			DateTime today = DateTime.Today;

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
					Age = today.Year - u.BirthDay.Year,
					Pictures = u.Pictures.Select(p => new UserPicture{ 
						Id = p.Id,
						UserId = p.UserId,
						Picture = p.Picture,
					}).ToList(),
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

		[HttpGet("GetComplaintCategory")]
		public IEnumerable<ComplaintCategory> GetComplaintCategories()
		{
			return _context.ComplaintCategories.ToList();
		}

		[HttpPost("Like")]
		public string Like(MatchRequestDTO request)
		{

				FriendList fl = null;
				var interacted = _context.FriendLists.FirstOrDefault(u => u.Clicked == request.BeenClicked && u.BeenClicked == request.Clicked);
				if (interacted != null)
				{
					if (interacted.state == 0)
					{
						interacted.state = 1;
						_context.FriendLists.Update(interacted);
						fl = new FriendList
						{
							Clicked = request.Clicked,
							BeenClicked = request.BeenClicked,
							state = 1
						};
						_context.FriendLists.Add(fl);
						_context.SaveChanges();
						return "配對成功!";
					}
				}

				fl = new FriendList
				{
					Clicked = request.Clicked,
					BeenClicked = request.BeenClicked,
					state = 0
				};

				_context.FriendLists.Add(fl);
				_context.SaveChanges();
				return "";

			
		}

		[HttpPost("Dislike")]
		public string Dislike(MatchRequestDTO request)
		{
			FriendList fl = new FriendList
			{
				Clicked = request.Clicked,
				BeenClicked = request.BeenClicked,
				state = 2
			};
			_context.FriendLists.Add(fl);
			_context.SaveChanges();
			return "";
		}

		[HttpGet("CanUserSwipe/{userId}")]
		public ActionResult<bool> CanUserSwipe(int userId)
		{
			return _membershipService.canUserSwipe(userId);
		}
	}
}
