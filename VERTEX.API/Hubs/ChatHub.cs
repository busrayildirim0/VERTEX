using Microsoft.AspNetCore.SignalR;

namespace VERTEX.API.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string channelName, string user, string message)
        {
            await Clients.Group(channelName).SendAsync("ReceiveMessage", user, message);
        }

        public async Task JoinChannel(string channelName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, channelName);
            await Clients.Group(channelName).SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} joined {channelName}");
        }

        public async Task LeaveChannel(string channelName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, channelName);
            await Clients.Group(channelName).SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} left {channelName}");
        }
    }
}
