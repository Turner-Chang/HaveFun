namespace HaveFun.DTOs
{
    public class CommentsDTO
    {

        public int userId { get; set; }
        public int postId { get; set; }
        public int? parentCommentId { get; set; } // 可為null
        public string content { get; set; }
        public DateTime time {  get; set; }
        public List<CommentsDTO> nestedReplies { get; set; } // 新增 NestedReplies 屬性
    }

}
