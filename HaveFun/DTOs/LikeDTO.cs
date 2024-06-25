using HaveFun.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.DTOs
{
    public class LikeDTO
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserPicture { get; set; }
    }
}
