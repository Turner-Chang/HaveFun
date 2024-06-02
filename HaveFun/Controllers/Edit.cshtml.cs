using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaveFun.Models;

namespace HaveFun.Controllers
{
    public class EditModel : PageModel
    {
        private readonly HaveFun.Models.HaveFunDbContext _context;

        public EditModel(HaveFun.Models.HaveFunDbContext context)
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

            var chatroom =  await _context.ChatRooms.FirstOrDefaultAsync(m => m.Id == id);
            if (chatroom == null)
            {
                return NotFound();
            }
            ChatRoom = chatroom;
           ViewData["User2Id"] = new SelectList(_context.UserInfos, "Id", "Account");
           ViewData["User1Id"] = new SelectList(_context.UserInfos, "Id", "Account");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ChatRoom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatRoomExists(ChatRoom.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ChatRoomExists(int id)
        {
            return _context.ChatRooms.Any(e => e.Id == id);
        }
    }
}
