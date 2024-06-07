using HaveFun.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HaveFun.DTO
{
    public class MemberProfileDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberProfileId { get; set; }

        [Required]
        [ForeignKey("UseId")]
        public int UsId { get; set; }
        public virtual UserInfo UseId { get; set; }

        
        [MaxLength(10)]
        public string Nickname { get; set; }

        [Required]
        [MaxLength(10)]
        public string Location { get; set; }

        [Required]
        [Column("BirthDay", TypeName = "Date")]
        public DateTime Birthday { get; set; }

        [MaxLength(10)]
        public string Occupation { get; set; }
        [MaxLength(30)]
        public string? Interests { get; set; }

        [MaxLength(50)]
        public string Introduction { get; set; }

        public IFormFile? Image { get; set; }
    }
}
