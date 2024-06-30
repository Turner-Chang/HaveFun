using HaveFun.Areas.ManagementSystem.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Mvc;
namespace HaveFun.DTOs
{
    public class ConId_UserIdDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string connId { get; set; }
    }
}
