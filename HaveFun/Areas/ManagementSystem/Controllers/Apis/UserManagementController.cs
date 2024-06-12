using HaveFun.Areas.ManagementSystem.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaveFun.Areas.ManagementSystem.Controllers.Apis
{
    [Route("api/UserInfo/[action]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private HaveFunDbContext _context;
        public UserManagementController(HaveFunDbContext funDbContext)
        {
            _context = funDbContext;
        }
        [HttpGet]
        public async Task<IEnumerable<UserInfo>> GetAll()
        {
            return _context.UserInfos;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfo>> GetById(int id)
        {
            var user1 = await _context.UserInfos.FindAsync(id);
            if (user1 == null)
            { return NotFound(); }

            return user1;
        }
        [HttpPut]
        public async Task<string> ChangeUserState(int id,UserInfoDTO userInfoDTO)
        {
			UserInfo user1 = await _context.UserInfos.FindAsync(id);
			if (user1 == null) { return "貼文狀態修改失敗"; }
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
				return "貼文狀態修改失敗";
			}

			return "貼文狀態修改成功";
		}
    }
}
