using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MyApiNetCore8.Data;
using MyApiNetCore8.Model;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly MyContext _myContext;

    public ChatHub(MyContext myContext)
    {
        _myContext = myContext;
    }

    public async Task SendMessage(int chatGroupId, string userId, string message)
    {
        var sentAt = DateTime.UtcNow;
        var chatMessage = new ChatMessage
        {
            ChatGroupId = chatGroupId,
            UserId = userId,
            Message = message,
            SentAt = sentAt
        };
        _myContext.ChatMessages.Add(chatMessage);
        await _myContext.SaveChangesAsync();

        var user = await _myContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        var avatar = user?.Avatar;
        var username = user?.UserName;
        await Clients.Group(chatGroupId.ToString()).SendAsync("ReceiveMessage", userId, avatar, username, sentAt, message);
    }

    public async Task JoinGroup(int chatGroupId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatGroupId.ToString());
    }

    public async Task LeaveGroup(int chatGroupId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatGroupId.ToString());
    }
}
