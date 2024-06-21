using HaveFun.Models;

namespace HaveFun.DTOs
{
    public class FriendDTO
    {
		
		public int Id { get; set; }
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsBlocked { get; set; }
        public int state { get; set; } = 0; //狀態,0-等待、1-配對、2封鎖

        public int Clicked { get; set; }//按讚會員編號,參考：UserInfo(會員)
        public virtual UserInfo User1 { get; set; }

        public int BeenClicked { get; set; }//被按讚會員編號,參考：UserInfo(會員)
        public virtual UserInfo User2 { get; set; }
    }
}
