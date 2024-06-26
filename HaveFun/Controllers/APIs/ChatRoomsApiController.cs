﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HaveFun.Models;
using Microsoft.EntityFrameworkCore;
using HaveFun.DTOs;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HaveFun.Controllers.APIs
{
    [Route("api/ChatRoom/[controller]")]
    [ApiController]
    public class ChatRoomsApiController : ControllerBase
    {
        private readonly HaveFunDbContext _context;

        public ChatRoomsApiController(HaveFunDbContext context)
        {
            _context = context;
        }
     

        public static ChatRoomDTO MapToChatRoomDTO(ChatRoom chatRoom, UserInfo sender, UserInfo receiver)
        {
            return new ChatRoomDTO
            {
                Id = chatRoom.Id,
                MessageText = chatRoom.MessageText,
                CreateTime = chatRoom.CreateTime,
                User1Id = chatRoom.User1Id,
                User2Id = chatRoom.User2Id,
                IsRead = chatRoom.IsRead,
            };
        }
  
        // GET: api/ChatRooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatRoomDTO>>> GetChatRooms()
        {
            var chatRooms = await _context.ChatRooms
                .Include(c => c.Receiver)
                .Include(c => c.Sender)
                .ToListAsync();

            var chatRoomDTOs = chatRooms.Select(cr => MapToChatRoomDTO(cr, cr.Sender, cr.Receiver));

            return Ok(chatRoomDTOs);
        }
   
        // POST: api/ChatRooms
        [HttpPost]
        public async Task<ActionResult<string>> PostChatRoom(ChatRoomDTO chatRoomDTO)
        {
            if (chatRoomDTO.User1Id == chatRoomDTO.User2Id)
            {
                return BadRequest("User1Id 和 User2Id 不能相同");
            }
            var chatRoom = new ChatRoom
            {
                MessageText = chatRoomDTO.MessageText,
                CreateTime = DateTime.UtcNow.AddHours(8),// Assume the creation time is now, in UTC
                User1Id = chatRoomDTO.User1Id,
                User2Id = chatRoomDTO.User2Id,
                IsRead = chatRoomDTO.IsRead
            };

            _context.ChatRooms.Add(chatRoom);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetChatRoom), new { id = chatRoom.Id }, $"聊天室編號:{chatRoom.Id}");
        }
     
        // GET: api/ChatRooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatRoomDTO>> GetChatRoom(int id)
        {
            var chatRoom = await _context.ChatRooms
                .Include(c => c.Receiver)
                .Include(c => c.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (chatRoom == null)
            {
                return NotFound();
            }

            var chatRoomDTO = MapToChatRoomDTO(chatRoom, chatRoom.Sender, chatRoom.Receiver);
            return Ok(chatRoomDTO);
        }
      
        // GET: api/ChatRooms/GetByUser1IdAndUser2Id/1/2
        [HttpGet("GetByUser1IdAndUser2Id/{User1Id}/{User2Id}")]
        public async Task<ActionResult<IEnumerable<ChatRoomDTO>>> GetByUser1IdAndUser2Id(int User1Id, int User2Id)
        {
            var chatRooms = await _context.ChatRooms
                .Where(m => m.User1Id == User1Id && m.User2Id == User2Id)
                .Include(c => c.Receiver)
                .Include(c => c.Sender)
                .ToListAsync();

            if (!chatRooms.Any())
            {
                return null;
            }

            var chatRoomDTOs = chatRooms.Select(chatRoom => MapToChatRoomDTO(chatRoom, chatRoom.Sender, chatRoom.Receiver));
            return Ok(chatRoomDTOs);
        }
      
        // PUT: api/ChatRooms/5
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> PutChatRoom(int id, ChatRoomDTO chatRoomDTO)
        {
            if (id != chatRoomDTO.Id)
            {
                return BadRequest("修改聊天室紀錄失敗");
            }

            var chatRoom = await _context.ChatRooms.FindAsync(id);
            if (chatRoom == null)
            {
                return NotFound("修改聊天室紀錄失敗");
            }

            chatRoom.MessageText = chatRoomDTO.MessageText;
            chatRoom.CreateTime = chatRoomDTO.CreateTime.AddHours(8);
            chatRoom.User1Id = chatRoomDTO.User1Id;
            chatRoom.User2Id = chatRoomDTO.User2Id;
            chatRoom.IsRead = chatRoomDTO.IsRead;

            _context.Entry(chatRoom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatRoomExists(id))
                {
                    return NotFound("修改聊天室紀錄失敗");
                }
                else
                {
                    throw;
                }
            }

            return Ok("修改聊天室紀錄成功");
        }
     
        // DELETE: api/ChatRooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChatRoom(int id)
        {
            var chatRoom = await _context.ChatRooms.FindAsync(id);
            if (chatRoom == null)
            {
                return NotFound();
            }

            _context.ChatRooms.Remove(chatRoom);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("GetAllChatHistory/{user1Id}/{user2Id}")]
        public async Task<ActionResult<IEnumerable<ChatRoom>>> GetAllChatHistory(int user1Id, int user2Id)
        {
            var chatHistory = await _context.ChatRooms
                .Where(c => (c.User1Id == user1Id && c.User2Id == user2Id) || (c.User1Id == user2Id && c.User2Id == user1Id))
                .OrderBy(c => c.CreateTime)
                .ToListAsync();

            return chatHistory;
        }
        [HttpGet("OnlineUserId")]
        public async Task<IActionResult> GetFriendsByConnId(int userId)
        {
            // 從UserIdConnId表中取得所有UserId
            var userIds = await _context.ConId_UserId
                .Select(uci => uci.UserId)
                .ToListAsync();

            if (userIds == null || !userIds.Any())
            {
                return NoContent();
            }

            // 用所有UserId篩選FriendList中的User
            var friends = await _context.FriendLists
                .Where(fl => fl.Clicked == userId && userIds.Contains(fl.BeenClicked))
                .Select(f => f.BeenClicked)
                .ToListAsync();

            return Ok(friends);
        }

        private bool ChatRoomExists(int id)
        {
            return _context.ChatRooms.Any(e => e.Id == id);
        }
    }
}