using Dashboard_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Dashboard.SignalR
{
    [Authorize]
    public class NotificationHub : Hub<IReceiveNotification>
    {
        public override async Task OnConnectedAsync()
        {
            // Enviar una notificación de conexión establecida con datos simplificados
            var initialData = new SimplifiedSalesData
            {
                Name = "Conexión establecida",
                Description = "Conexión inicial al hub de notificaciones",
                Price = 0,
                AffiliateId = string.Empty,
                CardId = string.Empty
            };

            await Clients.Client(Context.ConnectionId).ReceiveSalesData(initialData);
            await base.OnConnectedAsync();
        }
    }

    public interface IReceiveNotification
    {
        Task ReceiveSalesData(SimplifiedSalesData salesData);
    }
}
