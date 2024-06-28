using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using HaveFun.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

public class ChatHub : Hub
{
    private readonly HaveFunDbContext _context;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(HaveFunDbContext context, ILogger<ChatHub> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        try 
        {   
            await Clients.All.SendAsync("SomeOneOnline", Context.ConnectionId);

            int userId = GetUserIdFromContext();

            var existingConnIdUserId = await _context.ConId_UserId
                .FirstOrDefaultAsync(c => c.ConnId == Context.ConnectionId);

            if (existingConnIdUserId == null)
            {
                var userInfo = await _context.UserInfos.FindAsync(userId);
                if (userInfo != null)
                {
                    var connIdUserId = new ConId_UserId
                    {
                        Id = userId,
                        ConnId = Context.ConnectionId,
                    };
                    _context.ConId_UserId.Add(connIdUserId);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"New connection added for user {userId}: {Context.ConnectionId}");
                }
                else
                {
                    _logger.LogWarning($"User {userId} not found in UserInfos");
                }
            }
            else
            {
                _logger.LogInformation($"Existing connection found for user {existingConnIdUserId.Id}: {Context.ConnectionId}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in OnConnectedAsync");
        }

        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.Others.SendAsync("ReceiveMessage", user, message);
    }

    private int GetUserIdFromContext()
    {
        var userIdClaim = Context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        {
            return userId;
        }

        // If the user ID is not found in claims, you might want to handle this case
        // For example, you could throw an exception or return a default value
        throw new InvalidOperationException("User ID not found in the connection context");
    }

  
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        try
        {
            var connIdUserId = await _context.ConId_UserId
                .FirstOrDefaultAsync(c => c.ConnId == Context.ConnectionId);

            if (connIdUserId != null)
            {
                _context.ConId_UserId.Remove(connIdUserId);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Connection removed for user {connIdUserId.Id}: {Context.ConnectionId}");
            }

            await Clients.All.SendAsync("SomeOneOffline", Context.ConnectionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in OnDisconnectedAsync");
        }

        await base.OnDisconnectedAsync(exception);
    }
}