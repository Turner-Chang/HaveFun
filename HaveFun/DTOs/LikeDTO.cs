using HaveFun.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.DTOs
{
    public class LikeDTO
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}
