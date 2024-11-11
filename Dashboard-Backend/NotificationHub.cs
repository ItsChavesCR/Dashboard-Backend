using Dashboard_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Dashboard.SignalR
{
    [Authorize]
    public class NotificationHub : Hub<IReceiveNotification>
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).ReceiveSalesData(new SalesData { Id = 0, Producto = "Conexión establecida", Cantidad = 0, Precio = 0, Fecha = DateTime.Now });
            await base.OnConnectedAsync();
        }
    }

    public interface IReceiveNotification
    {
        Task ReceiveSalesData(SalesData salesData);
    }
}
