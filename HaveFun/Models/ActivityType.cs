using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
    public class ActivityType
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id {  get; set; }

        [Required]
        public string TypeName {  get; set; }
      
        public virtual ICollection<Activity>? Activities { get; set; }
    }
}
