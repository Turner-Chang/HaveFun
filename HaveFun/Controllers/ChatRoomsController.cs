using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaveFun.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;

namespace HaveFun.Controllers
{

    public class ChatRoomsController : Controller
    {
        private readonly HaveFunDbContext _context;
        //這是類別的建構函式,它接受一個 HaveFunDbContext 類型的參數並將它賦值給 _context 欄位。
        private int _Id;

        public ChatRoomsController(HaveFunDbContext context)
        {
            _context = context;
           
           
        }
		//
		[Authorize(AuthenticationSchemes = "Bearer,Cookies")]
		public async Task<IActionResult> Main()
        {
            var loginUser = Convert.ToInt32(Request.Cookies["userId"]);
            ViewBag.UserId = loginUser;
            var haveFunDbContext = _context.ChatRooms.Include(c => c.Receiver).Include(c => c.Sender);
               return View(await haveFunDbContext.ToListAsync());
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            //檢查Cookie 是否存在並嘗試獲取其值
            if (Request.Cookies.TryGetValue("Id", out string IdString) && int.TryParse(IdString, out int Id))
            {
                _Id = Id;
            }
            else
            {
                Id = -1; //默認值或其他處理
            }
        }
        //

        [NonAction]
        // GET: ChatRooms
        public async Task<IActionResult> Index()
        {
            var haveFunDbContext = _context.ChatRooms.Include(c => c.Receiver).Include(c => c.Sender);
            return View(await haveFunDbContext.ToListAsync());
        }


        [NonAction]
        // GET: ChatRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatRoom = await _context.ChatRooms
                .Include(c => c.Receiver)
                .Include(c => c.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatRoom == null)
            {
                return NotFound();
            }

            return View(chatRoom);
        }

        // GET: ChatRooms/Create
        [NonAction]
        public IActionResult Create()
        {
            ViewData["User2Id"] = new SelectList(_context.UserInfos, "Id", "Account");
            ViewData["User1Id"] = new SelectList(_context.UserInfos, "Id", "Account");
            return View();
        }

        // POST: ChatRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [NonAction]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MessageText,CreateTime,User1Id,User2Id,IsRead")] ChatRoom chatRoom)
        {
            if (chatRoom.User1Id != chatRoom.User2Id)
            {
                if (ModelState.IsValid == false)
                {
                    _context.Add(chatRoom);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
                ModelState.AddModelError(string.Empty, "User1Id 和 User2Id 不能相同");
                ViewData["User2Id"] = new SelectList(_context.UserInfos, "Id", "Account", chatRoom.User2Id);
            ViewData["User1Id"] = new SelectList(_context.UserInfos, "Id", "Account", chatRoom.User1Id);
            return View(chatRoom);
        }
        [NonAction]
        // GET: ChatRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatRoom = await _context.ChatRooms.FindAsync(id);
            if (chatRoom == null)
            {
                return NotFound();
            }
            ViewData["User2Id"] = new SelectList(_context.UserInfos, "Id", "Account", chatRoom.User2Id);
            ViewData["User1Id"] = new SelectList(_context.UserInfos, "Id", "Account", chatRoom.User1Id);
            return View(chatRoom);
        }

        // POST: ChatRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [NonAction]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MessageText,CreateTime,User1Id,User2Id,IsRead")] ChatRoom chatRoom)
        {
            if (id != chatRoom.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(chatRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatRoomExists(chatRoom.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["User2Id"] = new SelectList(_context.UserInfos, "Id", "Account", chatRoom.User2Id);
            ViewData["User1Id"] = new SelectList(_context.UserInfos, "Id", "Account", chatRoom.User1Id);
            return View(chatRoom);
        }
        [NonAction]
        // GET: ChatRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatRoom = await _context.ChatRooms
                .Include(c => c.Receiver)
                .Include(c => c.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatRoom == null)
            {
                return NotFound();
            }

            return View(chatRoom);
        }
        [NonAction]
        // POST: ChatRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chatRoom = await _context.ChatRooms.FindAsync(id);
            if (chatRoom != null)
            {
                _context.ChatRooms.Remove(chatRoom);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int messageId)
        {
            var message = await _context.ChatRooms.FindAsync(messageId);
            if (message != null && !message.IsRead)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }
            return Ok();
        }



        private bool ChatRoomExists(int id)
        {
            return _context.ChatRooms.Any(e => e.Id == id);
        }
    }
}
