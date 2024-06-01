using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
    public class ActivityParticipant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual UserInfo User { get; set; }

        [ForeignKey("Activity")]
        public int ActivityId { get; set; }
        
        public virtual Activity Activity { get; set; }
    }
}