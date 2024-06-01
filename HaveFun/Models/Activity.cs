using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
    public class Activity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual UserInfo User { get; set; }

        [Required]
       
        [ForeignKey("ActivityType")]
        public int Type { get; set; }
        public virtual ActivityType ActivityType { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Notes { get; set; }

        [Required]
        [MaxLength(10)]
        public string Amount { get; set; }

        [Required]
        public int MaxParticipants { get; set; }

        [Required]
        [MaxLength(200)]
        public string Location { get; set; }

        [Required]
        public DateTime InitiationTime { get; set; }

        [Required]
        public DateTime RegistrationTime { get; set; }

        [Required]
        public DateTime DeadlineTime { get; set; }

        [Required]
        public DateTime ActivityTime { get; set; }

        [Required]
        public int Status { get; set; }

        public ICollection<ActivityParticipant> ActivityParticipants { get; set; }

        public ICollection<ActivityReview> ActivityReviews { get; set; }
    }




}
