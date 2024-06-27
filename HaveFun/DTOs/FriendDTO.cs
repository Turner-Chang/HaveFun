using HaveFun.Models;

namespace HaveFun.DTOs
{
    public class FriendDTO
    {
		
		public int Id { get; set; }
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsBlocked { get; set; }
        public int state { get; set; } = 0; //狀態,0-等待、1-配對、2不喜歡 3封鎖
    }
}
