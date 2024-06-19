using HaveFun.Models;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace HaveFun.DTOs
{
    public class PostDateTimeConverter : IsoDateTimeConverter
    {
        public PostDateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd hh:mm:ss";
        }
    }


    public class PostsDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Contents { get; set; }
        //[JsonConverter(typeof(PostDateTimeConverter))]
        public string? Time { get; set; } 
        public string? Pictures { get; set; }
        public int Status { get; set; } = 0;
        public virtual ICollection<Like>? Like { get; set; } = new List<Like>();
        public List<CommentsDTO> Replies { get; set; } // 新增 Replies 屬性

    }
}