using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using HaveFun.Models;
using Microsoft.EntityFrameworkCore;


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
                var userId = GetUserIdFromContext();
                _logger.LogInformation($"User connected. UserId: {userId}, ConnectionId: {Context.ConnectionId}");

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
                        try
                        {
                            await _context.SaveChangesAsync();
                            _logger.LogInformation($"Added new connection for UserId: {userId}");
                        }
                        catch (DbUpdateException ex)
                        {
                            _logger.LogError(ex, $"DbUpdateException when adding connection for UserId: {userId}. Error: {ex.InnerException?.Message}");
                            // 可能需要在這裡進行一些錯誤處理，例如重試或清理
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"UserInfo not found for UserId: {userId}");
                    }
                }
                else
                {
                    _logger.LogInformation($"Connection already exists for UserId: {userId}");
                }

                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in OnConnectedAsync. Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        private int GetUserIdFromContext()
        {
            // 實現此方法以從連接上下文獲取用戶ID
            // 這可能涉及讀取聲明、查詢數據庫等
            // 目前返回一個佔位符值
            return 1; // 替換為實際實現
        }
    }