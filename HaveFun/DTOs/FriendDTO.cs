namespace HaveFun.DTOs
{
    public class FriendDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool IsBlocked { get; set; }
        public int ActivityTime { get; set; }
        public int ClickedUser { get; set; }
        public int BeenClickedUser { get; set; }
    }
}
