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
	[Route("api/[controller]/[action]")]
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

        // POST: api/MemberLabelsApi/PostMemberLabel       
        [HttpPost]
        public async Task<ActionResult<MemberLabelDTO>> PostMemberLabel(MemberLabelDTO memberLabelDTO)
        {
			if (memberLabelDTO == null || memberLabelDTO.LabelIds == null || !memberLabelDTO.LabelIds.Any())
			{ 
                return BadRequest("請求數據不能為空或標籤列表不能為空");
            }
            try
            {
                var memberLabels = new List<MemberLabel>();
                //在資料庫中建立新的MemberLabel紀錄
                foreach (var labelId in memberLabelDTO.LabelIds)
                {
                    //檢查LabelId是否有效
                    var labelExists = await _context.Labels.AnyAsync(l => l.Id == labelId);
                    if (!labelExists)
                    {
                        return BadRequest($"標籤ID{labelId} 不存在");
                    }

                    var memberLabel = new MemberLabel
                    {
                        UserId = memberLabelDTO.UserId,
                        LabelId = labelId
                    };
                    memberLabels.Add(memberLabel);
                }
                _context.MemberLabels.AddRange(memberLabels);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetMemberLabel), new { id = memberLabelDTO.UserId }, memberLabelDTO);
            }
            catch (Exception ex)
            {
				// 紀錄異常
				Console.Error.WriteLine($"保存標籤時發生異常: {ex.Message}");
				return StatusCode(500, $"保存標籤時發生異常2: {ex.Message}");
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
