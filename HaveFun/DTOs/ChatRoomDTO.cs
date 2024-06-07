namespace HaveFun.DTOs
{
    public class ChatRoomDTO
    {

        public int Id { get; set; }
        public string MessageText { get; set; }
        public DateTime CreateTime { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public bool IsRead { get; set; }
       
    }
}
