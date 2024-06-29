using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using HaveFun.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using Azure.Core;

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
            string userid = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            int userId = Convert.ToInt32(userid);
            _context.ConId_UserId.Add(new ConId_UserId
            {
                UserId = userId,
                ConnId = Context.ConnectionId,
            });
            await _context.SaveChangesAsync();
            _logger.LogInformation($"New connection added for user {userId}: {Context.ConnectionId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in OnConnectedAsync");
        }

        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string user, string friend, string message)
    {
        //connId資料庫撈取該friend的connid
        IEnumerable<string> conn = new string[] { "123" };
        var friendConnIds = await _context.ConId_UserId
            .Where(c => c.UserId.ToString() == friend)
            .Select(c => c.ConnId)
            .ToListAsync();

        if (friendConnIds.Any())
        {
            await Clients.Clients(friendConnIds).SendAsync("ReceiveMessage", user, message);
        }
        else
        {
            _logger.LogWarning($"No active connections found for friend ID: {friend}");
        }
        await Clients.Clients(conn).SendAsync("ReceiveMessage", user, message);
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