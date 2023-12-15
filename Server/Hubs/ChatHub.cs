using Microsoft.AspNetCore.SignalR;

namespace BlazorChatSignalR.Client.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> _users = new Dictionary<string, string>();
        public override async Task OnConnectedAsync()
        {
            string userName = Context.GetHttpContext().Request.Query["username"];
            _users.Add(Context.ConnectionId, userName);
            await AddMessageToChat(string.Empty, $"{userName} Connected!");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string userName = _users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
            await AddMessageToChat(string.Empty, $"{userName} left!");
        }
        public async Task AddMessageToChat(string userName, string message)
        {
            await Clients.All.SendAsync("RecieveMessage",userName, message);
        }
    }
}
