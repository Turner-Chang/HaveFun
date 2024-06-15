using HaveFun.Areas.ManagementSystem.DTOs;
using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaveFun.Areas.ManagementSystem.Controllers.Apis
{
	[Route("api/UserInfo/[action]")]
	[ApiController]
	public class UserReview_UserInfo_Controller : ControllerBase
	{
		private HaveFunDbContext _context;
		public UserReview_UserInfo_Controller(HaveFunDbContext funDbContext)
		{
			_context = funDbContext;
		}
		[HttpGet]
		public async Task<IEnumerable<UserInfo>> GetAll()
		{
			return _context.UserInfos;
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<UserInfoDTO>> GetById(int id)
		{

			var user1 = await _context.UserInfos.FindAsync(id);
			if (user1 == null)
			{ return NotFound(); }
			
				UserInfoDTO userInfoDTO1 = new UserInfoDTO
				{
					Gender = user1.Gender,
					Name = user1.Name,
					Id = user1.Id,
					Status = user1.Status,
					Introduction = user1.Introduction,
					BirthDay = user1.BirthDay,
				};
			

			return userInfoDTO1;
		}
		[HttpPut]
		public async Task<string> ChangeUserState(int id, UserInfoDTO userInfoDTO)
		{
			UserInfo user1 = await _context.UserInfos.FindAsync(id);
			if (user1 == null) { return "用戶狀態修改失敗"; }
			if (userInfoDTO.Id == user1.Id)
			{
				user1.Status = userInfoDTO.Status;
			}
			_context.Entry(user1).State = EntityState.Modified;
			try
			{
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				return "用戶狀態修改失敗";
			}

			return "用戶狀態修改成功";
		}


	}
}
