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
    public class LabelCategoriesManageApiController : ControllerBase
    {
        private readonly HaveFunDbContext _context;

        public LabelCategoriesManageApiController(HaveFunDbContext context)
        {
            _context = context;
        }
        

        // GET: api/LabelCategoriesManageApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LabelCategoryManageDTO>>> GetLabelCategories()
        {
            var categories = await _context.LabelCategories.ToListAsync();
            var categoryDTOs = categories.Select(c => new LabelCategoryManageDTO
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();

            return categoryDTOs;
        }

        // GET: api/LabelCategoriesManageApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LabelCategoryManageDTO>> GetLabelCategory(int id)
        {
            var category = await _context.LabelCategories.FindAsync(id);
            
            if (category == null)
            {
                return NotFound();
            }

            var categoryDTO = new LabelCategoryManageDTO
            {
                Id = category.Id,
                Name = category.Name
            };

            return categoryDTO;
        }

        // PUT: api/LabelCategoriesManageApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLabelCategory(int id, LabelCategoryManageDTO categoryDTO)
        {
            if (id != categoryDTO.Id)
            {
                return BadRequest();
            }
            var existingCategory = await _context.LabelCategories.FindAsync(id);
            if (existingCategory == null) { return NotFound(); }

            existingCategory.Name = categoryDTO.Name;
            _context.Entry(existingCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LabelCategoryExists(id))
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

        // POST: api/LabelCategoriesManageApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LabelCategoryManageDTO>> CreateLabelCategory(LabelCategoryManageDTO categoryDTO)
        {
            //檢查標籤種類是否存在
            var existingCategory = await _context.LabelCategories.FirstOrDefaultAsync(c => c.Name == categoryDTO.Name);
            if (existingCategory != null) {
                //返回錯誤訊息
                return Conflict(new { message = "此標籤種類已存在" });
            }

            var category = new LabelCategory
            {
                Name = categoryDTO.Name
            };
            _context.LabelCategories.Add(category);
            await _context.SaveChangesAsync();
            categoryDTO.Id = category.Id;

            return CreatedAtAction(nameof(GetLabelCategories), new { id = category.Id }, categoryDTO);
        }

        // DELETE: api/LabelCategoriesManageApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLabelCategory(int id)
        {
            var category = await _context.LabelCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.LabelCategories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LabelCategoryExists(int id)
        {
            return _context.LabelCategories.Any(e => e.Id == id);
        }
    }
}
