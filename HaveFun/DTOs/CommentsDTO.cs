namespace HaveFun.DTOs
{
    public class CommentsDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; } // 可為null
        public string Content { get; set; }
        public DateTime Time {  get; set; }
        public List<CommentsDTO> NestedReplies { get; set; } // 新增 NestedReplies 屬性
    }

}
