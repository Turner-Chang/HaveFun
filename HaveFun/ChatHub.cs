using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using HaveFun.Models;  // 確保引用模型命名空間

public class ChatHub : Hub
{
    private readonly HaveFunDbContext _context;  // DbContext名稱
    //注入
    public ChatHub(HaveFunDbContext context)
    {
        _context = context;
    }

    public override Task OnConnectedAsync()
    {
        Clients.All.SendAsync("SomeOneOnline", Context.ConnectionId);
        return base.OnConnectedAsync();
    }


    // Commented code
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}