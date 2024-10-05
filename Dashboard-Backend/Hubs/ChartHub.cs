using Microsoft.AspNetCore.SignalR;

namespace Dashboard_Backend.Hubs
{
    public class ChartHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}