using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace HaveFun.Models
{
	public class FriendList
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }//好友清單流水號

		//public virtual ICollection<UserInfo> User1 { get; set; }


		[ForeignKey("User1")]
		[Required]
		public int Clicked { get; set; }//按讚會員編號,參考：UserInfo(會員)
		public virtual UserInfo User1 { get; set; }


		[ForeignKey("User2")]
		[Required]
		public int BeenClicked { get; set; }//被按讚會員編號,參考：UserInfo(會員)
		public virtual UserInfo User2 { get; set; }


		[Required]
		public int state { get; set; } = 0; //狀態,0-等待、1-配對、2封鎖

		public List<Friend> Friends { get; set; } //存儲從 FriendService 獲取的好友列表數據
        public List<Friend> BlackList { get; set; }

	}
}
