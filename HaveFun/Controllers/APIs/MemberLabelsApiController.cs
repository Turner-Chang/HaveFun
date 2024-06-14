using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaveFun.Models;
using HaveFun.DTOs;

namespace HaveFun.Controllers.APIs
{
	[Route("api/[controller]")]
	[ApiController]
    public class MemberLabelsApiController : ControllerBase
    {
        private readonly HaveFunDbContext _context;

        public MemberLabelsApiController(HaveFunDbContext context)
        {
            _context = context;
        }

        // GET: api/MemberLabelsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberLabel>>> GetMemberLabels()
        {
            return await _context.MemberLabels.ToListAsync();
        }

        // GET: api/MemberLabelsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberLabel>> GetMemberLabel(int id)
        {
            var memberLabel = await _context.MemberLabels.FindAsync(id);

            if (memberLabel == null)
            {
                return NotFound();
            }

            return memberLabel;
        }

        // PUT: api/MemberLabelsApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMemberLabel(int id, MemberLabel memberLabel)
        {
            if (id != memberLabel.Id)
            {
                return BadRequest();
            }

            _context.Entry(memberLabel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberLabelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

		// POST: api/MemberLabelsApi/submitLabels      
		[HttpPost("submitLabels")]
		public async Task<IActionResult> SubmitLabels([FromBody] MemberLabelDTO memberLabelDTO)
		{
			if (memberLabelDTO == null || memberLabelDTO.LabelIds == null || !memberLabelDTO.LabelIds.Any())
			{
				return BadRequest("请求数据不能为空或标签列表不能为空");
			}

			try
			{
				Console.WriteLine($"接收到的数据: UserId = {memberLabelDTO.UserId}, LabelIds = {string.Join(", ", memberLabelDTO.LabelIds)}");

				// 清理旧的标签记录
				var existingLabels = await _context.MemberLabels
					.Where(ml => ml.UserId == memberLabelDTO.UserId)
					.ToListAsync();
				_context.MemberLabels.RemoveRange(existingLabels);

				// 添加新的标签记录
				foreach (var labelId in memberLabelDTO.LabelIds)
				{
					var memberLabel = new MemberLabel
					{
						UserId = memberLabelDTO.UserId,
						LabelId = labelId
					};
					_context.MemberLabels.Add(memberLabel);
				}

				await _context.SaveChangesAsync();
				return CreatedAtAction(nameof(GetMemberLabel), new { id = memberLabelDTO.UserId }, memberLabelDTO);
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"保存成员标签时发生异常: {ex.Message}");
				return StatusCode(500, $"保存成员标签时发生异常: {ex.Message}");
			}
		}

		// DELETE: api/MemberLabelsApi/5
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMemberLabel(int id)
        {
            var memberLabel = await _context.MemberLabels.FindAsync(id);
            if (memberLabel == null)
            {
                return NotFound();
            }

            _context.MemberLabels.Remove(memberLabel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberLabelExists(int id)
        {
            return _context.MemberLabels.Any(e => e.Id == id);
        }
    }
}
