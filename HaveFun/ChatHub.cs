using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using HaveFun.Models;
using Microsoft.EntityFrameworkCore;

public class ChatHub : Hub
{
    private readonly HaveFunDbContext _context;

    public ChatHub(HaveFunDbContext context)
    {
        _context = context;
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("SomeOneOnline", Context.ConnectionId);

        var userId = GetUserIdFromContext(); // 您需要實現這個方法來獲取用戶ID

        // 檢查是否已存在相同的 connId
        var existingConnIdUserId = await _context.ConId_UserId
            .FirstOrDefaultAsync(c => c.connId == Context.ConnectionId);

        if (existingConnIdUserId == null)
        {
            var userInfo = await _context.UserInfos.FindAsync(userId);

            if (userInfo != null)
            {
                var connIdUserId = new ConId_UserId
                {
                    Id = userId,
                    connId = Context.ConnectionId,
                };

                _context.ConId_UserId.Add(connIdUserId);
                await _context.SaveChangesAsync();
            }
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await Clients.All.SendAsync("SomeOneOffline", Context.ConnectionId);

        var connIdUserId = await _context.ConId_UserId
            .FirstOrDefaultAsync(c => c.connId == Context.ConnectionId);

        if (connIdUserId != null)
        {
            _context.ConId_UserId.Remove(connIdUserId);
            await _context.SaveChangesAsync();
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.Others.SendAsync("ReceiveMessage", user, message);
    }

    private int GetUserIdFromContext()
    {
        // 實現這個方法來從連接上下文獲取用戶ID
        // 這可能涉及讀取聲明、查詢數據庫等
        throw new NotImplementedException("GetUserIdFromContext 需要被實現");
    }
}