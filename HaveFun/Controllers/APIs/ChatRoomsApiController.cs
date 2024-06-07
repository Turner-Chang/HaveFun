﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HaveFun.Models;
using Microsoft.EntityFrameworkCore;
using HaveFun.DTOs;

namespace HaveFun.Controllers.APIs
{
    [Route("api/ChatRoom/[controller]")]
    [ApiController]
    public class ChatRoomsApiController : ControllerBase
    {
        private readonly HaveFunDbContext _context;
        //建構函式
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
        // POST: api/ChatRooms
        [HttpPost]
        public async Task<ActionResult<string>> PostChatRoom(ChatRoomDTO chatRoomDTO)
        {
            var chatRoom = new ChatRoom
            {
                MessageText = chatRoomDTO.MessageText,
                CreateTime = chatRoomDTO.CreateTime,
                User1Id = chatRoomDTO.User1Id,
                User2Id = chatRoomDTO.User2Id,
                IsRead = chatRoomDTO.IsRead
            };

            _context.ChatRooms.Add(chatRoom);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetChatRoom), new { id = chatRoom.Id }, $"聊天室編號:{chatRoom.Id}");
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
            chatRoom.CreateTime = chatRoomDTO.CreateTime;
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

        private bool ChatRoomExists(int id)
        {
            return _context.ChatRooms.Any(e => e.Id == id);
        }
    }
}