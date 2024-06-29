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
        #region delete
        [HttpDelete("delete")]
        public async Task<ActionResult<IEnumerable<ConId_UserIdDTO>>> delete()
        {
            try
            {
                // Retrieve all records from ConId_UserId table
                var conIdUsers = await _context.ConId_UserId.ToListAsync();
                if (conIdUsers == null || !conIdUsers.Any())
                {
                    return NotFound("No records found in ConId_UserId table");
                }

                // Create DTOs before deletion
                var conIdUserDTOs = conIdUsers.Select(u => new ConId_UserIdDTO
                {
                    Id = u.Id,
                    UserId = u.UserId,
                    connId = u.ConnId
                }).ToList();

                // Remove all records from the ConId_UserId table
                _context.ConId_UserId.RemoveRange(conIdUsers);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok(conIdUserDTOs);
            }
            catch (Exception ex)
            {
                // Log the exception
                // You might want to use a proper logging framework here
                Console.WriteLine($"An error occurred while deleting data from ConId_UserId: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting data from the ConId_UserId table.");
            }
        }
        #endregion
    }
}