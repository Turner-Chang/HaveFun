namespace HaveFun.Models
{
    public class ConId_UserId
    {
        public int Id { get; set; }
        public virtual UserInfo User { get; set; }
        public string connId { get; set; }


    }
}
