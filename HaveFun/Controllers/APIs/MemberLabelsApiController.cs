using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaveFun.Models;

namespace HaveFun.Controllers.APIs
{
    [Route("api/MemberLabelsApi/[action]")]
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MemberLabel>> PostMemberLabel(MemberLabel memberLabel)
        {
            _context.MemberLabels.Add(memberLabel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMemberLabel", new { id = memberLabel.Id }, memberLabel);
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
