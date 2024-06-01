using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
    public class UserInfo
    {
        [Key] // 主鍵
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 設定自動編號
        public int Id { get; set; }
        [StringLength(maximumLength: 100, MinimumLength = 15)]
        [Required]
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
        [Column("Account",TypeName = "nvarchar(100)")]
        public string Account { get; set; }
        [Required]
        [StringLength(maximumLength:255, MinimumLength =10)]
        // 設定密碼一定要有一個大寫英文跟一個小寫英文，然後至少10字
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{10,20}$",ErrorMessage = "密碼必須包含至少一個大寫字母、一個小寫字母和一個數字，且長度在 10 到 20 個字符之間")]
        [Column("Password", TypeName = "nvarchar(255)")]
        public string Password { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 1,ErrorMessage = "姓名長度必須介於 1 到 50 個字之間")]
        [Column("Name", TypeName = "nvarchar(50)")]
        public string Name {  get; set; }

        [StringLength(maximumLength: 100, ErrorMessage = "地址長度不可超過100字")]
        [Column("Address", TypeName = "nvarchar(100)")]
        public string Address { get; set; }
        [StringLength(maximumLength: 20,MinimumLength =10, ErrorMessage = "電話號碼應該介於 10 到 20 個字元之間")]
        [Column("PhoneNumber", TypeName = "varchar(20)")]
        public string PhoneNumber { get; set; }
        [Required]
        public int Gender { get; set; }
        [Required]
        [Column("BirthDay",TypeName ="Date")]
        public DateTime BirthDay { get; set; }

        public string ProfilePicture { get; set; }
        [Required]
        public int Status { get; set; } = 0;
        [Required]
        public int AccountStatus { get; set; } = 0;

        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginTime { get; set; }
        [StringLength(maximumLength:100)]
        [Column("Introduction",TypeName ="nvarchar(100)")]
        public string Introduction { get; set; }
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
	}
}
