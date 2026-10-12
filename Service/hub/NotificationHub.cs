using Microsoft.AspNetCore.SignalR;

namespace API_HotelBooking.Hubs
{
    public class NotificationHub : Hub
    {

        public async Task SendUnreadCountToAll(int count)
        {
            await Clients.All.SendAsync("ReceiveUnreadCount", count);
        }
    }
}