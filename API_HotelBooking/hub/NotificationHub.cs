using Microsoft.AspNetCore.SignalR;

namespace API_HotelBooking.Hubs
{
    public class NotificationHub : Hub
    {
        // Gửi số notification chưa đọc đến client
        public async Task SendUnreadCount(int userId, int count)
        {
            await Clients.User(userId.ToString()).SendAsync("ReceiveUnreadCount", count);
        }
    }
}