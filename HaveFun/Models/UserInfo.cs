using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
    public class UserInfo
    {
        [Key] // 主鍵
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 設定自動編號
        public int Id { get; set; }

        
        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 15)]
        [Column("Account",TypeName = "nvarchar(100)")]
        public string Account { get; set; }

        [Required]
        [StringLength(maximumLength:255, MinimumLength =10)]
        [Column("Password", TypeName = "nvarchar(255)")]
        public string Password { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 1)]
        [Column("Name", TypeName = "nvarchar(50)")]
        public string Name {  get; set; }

        [StringLength(maximumLength: 100)]
        [Column("Address", TypeName = "nvarchar(100)")]
        public string? Address { get; set; }

        [StringLength(maximumLength: 20,MinimumLength =10)]
        [Column("PhoneNumber", TypeName = "varchar(20)")]
        public string? PhoneNumber { get; set; }

        [Required]
        public int Gender { get; set; }

        [Required]
        [Column("BirthDay",TypeName ="Date")]
        public DateTime BirthDay { get; set; }

        public string? ProfilePicture { get; set; }

        [Required]
        public int Status { get; set; } = 0;

        [Required]
        public int AccountStatus { get; set; } = 0;

        public DateTime? RegistrationDate { get; set; }

        public DateTime? LastLoginTime { get; set; }

        [StringLength(maximumLength:100)]
        [Column("Introduction",TypeName ="nvarchar(100)")]
        public string? Introduction { get; set; }

        [Required]
        public int Level { get; set; } = 0;

        [Required]
        public string PasswordSalt { get; set; }

        
        public virtual ICollection<ChatRoom> SenderMessages { get; set; }

        public virtual ICollection<ChatRoom> ReceiverMessages { get; set; }

		public virtual ICollection<FriendList> Friends1 { get; set; }

		public virtual ICollection<FriendList> Friends2 { get; set; }

		public virtual ICollection<ActivityParticipant> ActivityParticipants { get; set; }

		public virtual ICollection<ActivityReview> ActivityReviews { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

		public virtual ICollection<PostReview> PostReviews { get; set; }

        public virtual ICollection<MemberLabel> MemberLabels { get; set; }
	}
}
