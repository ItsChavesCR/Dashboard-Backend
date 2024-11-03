using Microsoft.AspNetCore.SignalR;

namespace Dashboard.SignalR
{
    public class ChatHub : Hub<IChatClient>
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.ReceiveMessage($"{Context.ConnectionId} se ha unido!");
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.ReceiveMessage($"{Context.ConnectionId}: {message}");
        }
    }

    public interface IChatClient
    {
        Task ReceiveMessage(string message);
    }
}
