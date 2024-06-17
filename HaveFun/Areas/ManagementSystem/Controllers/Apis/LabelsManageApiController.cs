using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaveFun.Models;
using HaveFun.Areas.ManagementSystem.DTOs;

namespace HaveFun.Areas.ManagementSystem.Controllers.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelsManageApiController : ControllerBase
    {
        private readonly HaveFunDbContext _context;

        public LabelsManageApiController(HaveFunDbContext context)
        {
            _context = context;
        }       

        // GET: api/LabelsManage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LabelManageDTO>>> GetLabels()
        {
            var labels = await _context.Labels.Include(l => l.LabelCategory).ToListAsync();
            var labelDTOs = labels.Select(l => new LabelManageDTO
            {
                Id = l.Id,
                Name = l.Name,
                CategoryId = l.CategoryId
                
            }).ToList();
            return labelDTOs; 
		}

        // GET: api/LabelsManage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LabelManageDTO>> GetLabel(int id)
        {
            var label = await _context.Labels.Include(l => l.LabelCategory).FirstOrDefaultAsync(l => l.Id == id);

            if (label == null)
            {
                return NotFound();
            }
            var labelDTO = new LabelManageDTO
            {
                Id = label.Id,
                Name = label.Name,
                CategoryId = label.CategoryId
                
            };
            return labelDTO;
        }

        // PUT: api/LabelsManage/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdataLabel(int id, LabelManageDTO labelDTO)
        {
            
            if (id != labelDTO.Id) { 
                return BadRequest();
            }
            var existingLabel = await _context.Labels.FindAsync(id);
            if (existingLabel == null) { return NotFound(); }

            existingLabel.Name = labelDTO.Name;
            existingLabel.CategoryId = labelDTO.CategoryId;

            _context.Entry(existingLabel).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!LabelExists(id))
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

        // POST: api/LabelsManage
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LabelManageDTO>> CreateLabel(LabelManageDTO labelDTO)
        {
            var label = new Label
            {
                Name = labelDTO.Name,
                CategoryId = labelDTO.CategoryId,
            };
            _context.Labels.Add(label);
            await _context.SaveChangesAsync();
            labelDTO.Id = label.Id;
            return CreatedAtAction(nameof(GetLabel), new { id = label.Id }, labelDTO);
        }
       

        // DELETE: api/LabelsManage/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLabel(int id)
        {
            var label = await _context.Labels.FindAsync(id);
            if (label == null)
            {
                return NotFound();
            }
            _context.Labels.Remove(label);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LabelExists(int id)
        {
            return _context.Labels.Any(e => e.Id == id);
        }
    }
}
