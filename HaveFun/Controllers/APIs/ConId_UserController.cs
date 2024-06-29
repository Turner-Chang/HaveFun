using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaveFun.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConId_UserController : ControllerBase
    {
        private readonly HaveFunDbContext _context;

        public ConId_UserController(HaveFunDbContext context)
        {
            _context = context;
        }
        #region getId one by one

        [HttpGet("GetOnlineStatus/{friendId}")]
        public async Task<ActionResult<ConId_UserIdDTO>> GetOnlineStatus(int friendId)
        {
            var conIdUser = await _context.ConId_UserId
                .FirstOrDefaultAsync(c => c.UserId == friendId);

            if (conIdUser == null)
            {
                return NotFound($"No online status found for user with ID {friendId}");
            }

            return new ConId_UserIdDTO
            {
                Id = conIdUser.Id,
                UserId = conIdUser.UserId,
                connId = conIdUser.ConnId
            };
        }
        #endregion

        #region getall
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ConId_UserIdDTO>>> GetAllConIdUsers()
        {
            var conIdUsers = await _context.ConId_UserId.ToListAsync();

            if (conIdUsers == null || !conIdUsers.Any())
            {
                return NotFound("No records found in ConId_UserId table");
            }

            var conIdUserDTOs = conIdUsers.Select(u => new ConId_UserIdDTO
            {
                Id = u.Id,
                UserId = u.UserId,
                connId = u.ConnId
            }).ToList();

            return Ok(conIdUserDTOs);
        }
        #endregion
    }
}