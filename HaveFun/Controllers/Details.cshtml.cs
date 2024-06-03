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
    public class DetailsModel : PageModel
    {
        private readonly HaveFun.Models.HaveFunDbContext _context;

        public DetailsModel(HaveFun.Models.HaveFunDbContext context)
        {
            _context = context;
        }

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
    }
}
