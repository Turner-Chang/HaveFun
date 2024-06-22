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

    public  override   Task OnConnectedAsync()
    {
        Clients.All.SendAsync("SomeOneOnline", Context.ConnectionId);
        //int userId = 2; // 替換為實際的用戶 ID 獲取邏輯

        //// 檢查是否已存在相同的 connId
        //var existingConnIdUserId = _context.ConId_UserId
        //    .FirstOrDefault(c => c.connId == Context.ConnectionId);

        //if (existingConnIdUserId == null)
        //{
        //    var userInfo = _context.UserInfos.Find(userId); // 查找 UserInfo 實體

        //    if (userInfo != null)
        //    {
        //        var connIdUserId = new ConId_UserId
        //        {
        //            Id = userId,
        //            connId = Context.ConnectionId,
        //        };

        //        // 將新物件添加到 DbContext 中
        //        _context.ConId_UserId.Add(connIdUserId);
        //    }
        //}
        //else
        //{
        //    // 如果存在，則不需要新增
        //}

        //// 保存變更到資料庫
        //await _context.SaveChangesAsync();

        return   base.OnConnectedAsync();
    }


    // Commented code
    public async Task SendMessage(string user, string message)
    {
        await Clients.Others.SendAsync("ReceiveMessage", user, message);
    }
}