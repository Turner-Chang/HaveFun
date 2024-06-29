using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HaveFun.Models
{
    public class ConId_UserId
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserInfo User { get; set; }

        [Required]
        public string ConnId { get; set; }

    }
}
