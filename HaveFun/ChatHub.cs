using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using HaveFun.Models;  // 確保引用模型命名空間

public class ChatHub : Hub
{
    private readonly HaveFunDbContext _context;  // DbContext名稱

    public ChatHub(HaveFunDbContext context)
    {
        _context = context;
    }

    public override Task OnConnectedAsync()
    {
        Clients.All.SendAsync("SomeOneOnline", Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public async Task SendMessage(int senderId, int receiverId, string message)
    {
        var chatRoom = new ChatRoom
        {
            User1Id = senderId,
            User2Id = receiverId,
            MessageText = message,
            CreateTime = DateTime.UtcNow
        };

        _context.ChatRooms.Add(chatRoom);
        await _context.SaveChangesAsync();

        // Send the message to both the sender and receiver
        await Clients.User(senderId.ToString()).SendAsync("ReceiveMessage", chatRoom);
        await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", chatRoom);
    }
}