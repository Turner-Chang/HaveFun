using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HaveFun.Models;

namespace HaveFun.Controllers
{
    public class DeleteModel : PageModel
    {
        private readonly HaveFun.Models.HaveFunDbContext _context;

        public DeleteModel(HaveFun.Models.HaveFunDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ChatRoom ChatRoom { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatroom = await _context.ChatRooms.FirstOrDefaultAsync(m => m.Id == id);

            if (chatroom == null)
            {
                return NotFound();
            }
            else
            {
                ChatRoom = chatroom;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatroom = await _context.ChatRooms.FindAsync(id);
            if (chatroom != null)
            {
                ChatRoom = chatroom;
                _context.ChatRooms.Remove(ChatRoom);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
