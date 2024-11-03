using Microsoft.AspNetCore.SignalR;

namespace Dashboard.SignalR
{
    public class NotificationHub : Hub<IReceiveNotification>
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).ReceiveNotification($"Gracias, conectando {Context.User?.Identity?.Name}");

            await base.OnConnectedAsync();
        }
    }

    public interface IReceiveNotification
    {
        Task ReceiveNotification(string message);
    }
}
