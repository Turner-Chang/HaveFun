using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HaveFun.Models;

namespace HaveFun.Controllers
{
    public class CreateModel : PageModel
    {
        private readonly HaveFun.Models.HaveFunDbContext _context;

        public CreateModel(HaveFun.Models.HaveFunDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["User2Id"] = new SelectList(_context.UserInfos, "Id", "Account");
        ViewData["User1Id"] = new SelectList(_context.UserInfos, "Id", "Account");
            return Page();
        }

        [BindProperty]
        public ChatRoom ChatRoom { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ChatRooms.Add(ChatRoom);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
