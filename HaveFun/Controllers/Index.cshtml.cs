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
    public class IndexModel : PageModel
    {
        private readonly HaveFun.Models.HaveFunDbContext _context;

        public IndexModel(HaveFun.Models.HaveFunDbContext context)
        {
            _context = context;
        }

        public IList<ChatRoom> ChatRoom { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ChatRoom = await _context.ChatRooms
                .Include(c => c.Receiver)
                .Include(c => c.Sender).ToListAsync();
        }
    }
}
