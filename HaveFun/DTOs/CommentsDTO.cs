using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace HaveFun.DTOs
{
    public class CommentDateTimeConverter : IsoDateTimeConverter
    {
        public CommentDateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }

    public class CommentsDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PostId { get; set; }    
        public int? ParentCommentId { get; set; } // 可為null
        public string Content { get; set; }

        //[JsonConverter(typeof(CommentDateTimeConverter))]
        public DateTime Time {  get; set; }
        public List<CommentsDTO> NestedReplies { get; set; } // 新增 NestedReplies 屬性
    }

}
