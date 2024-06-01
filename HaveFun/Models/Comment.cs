using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
    [Table("Comment")]
    public class Comment
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("ParentComment")]
        public int? ParentCommentId { get; set; }
        public virtual Comment ParentComment { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual UserInfo User { get; set; }

        [Required]
        [MaxLength(280, ErrorMessage ="回覆內容上限為280字")]
        [Column("Contents", TypeName = "nvarchar")]
        public string Contents { get; set; }
        [Required]
        public DateTime Time { get; set; }
    }
}
