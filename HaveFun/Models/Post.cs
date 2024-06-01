using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
    public class Post
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual UserInfo User { get; set; }
        [Required]
        [MaxLength(2000, ErrorMessage ="文字上限為2000字")]
        [Column("Contents", TypeName = "nvarchar")]
        public string Contents { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [MaxLength(255)]
        public string? Pictures { get; set; }
        [Required]
        [Range(0, 1)]
        public int Status { get; set; } = 0;

    }
}
