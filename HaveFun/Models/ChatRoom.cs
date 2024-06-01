using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaveFun.Models
{
    public class ChatRoom
    {

        [Key]
        public int Id { get; set; }  //Chat room
        public string MessageText { get; set; } // Message NVARCHAR(100)
     
        public DateTime CreateTime { get; set; } // CreateTime DATETIME

        public int User1Id { get; set; }
        public int User2Id { get; set; }

        [ForeignKey("User1Id")]
        public UserInfo Sender { get; set; }

        [ForeignKey("User2Id")]
        public UserInfo Receiver { get; set; }

        public bool IsRead { get; set; } = false;
       
    }
}