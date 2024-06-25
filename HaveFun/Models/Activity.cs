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
        public virtual UserInfo? User { get; set; }

        [Required(ErrorMessage = "此項為必填")]       
        [ForeignKey("ActivityType")]
        [Display(Name ="活動類型")]
        public int Type { get; set; }
        public virtual ActivityType? ActivityType { get; set; }

        [Required(ErrorMessage = "此項為必填")]
        [MaxLength(50)]
        [Display(Name ="活動標題")]
        public string Title { get; set; }

        [Required(ErrorMessage = "此項為必填")]
        [MaxLength(1000)]
        [Display(Name ="活動内容")]
        public string Content { get; set; }

        [Required(ErrorMessage = "此項為必填")]
        [MaxLength(1000)]
        [Display(Name = "活動注意事項")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "此項為必填")]
        [Display(Name ="活動預算")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "此項為必填")]
        [Range(1, int.MaxValue, ErrorMessage = "活動最大人數必須大於 0")]
        [Display(Name ="活動最大人數")]
        public int MaxParticipants { get; set; }

        [Required(ErrorMessage = "此項為必填")]
        [MaxLength(200)]
        [Display(Name ="活動地點")]
        public string Location { get; set; }

        [Required(ErrorMessage = "此項為必填")]
        [Display(Name = "活動發起時間")]
        public DateTime InitiationTime { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "此項為必填")]
        [Display(Name = "活動報名時間")]
        public DateTime RegistrationTime { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "此項為必填")]
        [Display(Name = "活動截止時間")]
        public DateTime DeadlineTime { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "此項為必填")]
        [Display(Name = "活動開始時間")]
        public DateTime ActivityTime { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "活動狀態")]
        public int Status { get; set; } = 0;

        [Display(Name = "活動圖片")]
		public byte[]? Picture { get; set; }

		public ICollection<ActivityParticipant>? ActivityParticipants { get; set; }

        public ICollection<ActivityReview>? ActivityReviews { get; set; }

    }




}
