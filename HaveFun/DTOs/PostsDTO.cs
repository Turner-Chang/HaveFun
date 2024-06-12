using System.ComponentModel.DataAnnotations;
using System.Data;

namespace HaveFun.DTOs
{
    public class PostsDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Contents { get; set; }
        public DateTime Time { get; set; }
        public string Pictures { get; set; }
        public List<CommentsDTO> Replies { get; set; } // 新增 Replies 屬性

    }
}