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
        public DateTime Time { get; set; }
        public string Pictures { get; set; }
        public List<CommentsDTO> Replies { get; set; } // 新增 Replies 屬性

    }
}