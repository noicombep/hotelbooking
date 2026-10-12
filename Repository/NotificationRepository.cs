using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly HotelBookingDbContext _context;

        public NotificationRepository(HotelBookingDbContext context)
        {
            _context = context;
        }

        public async Task Add(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
        public async Task<int> GetUnreadCount()
        {
            return await _context.Notifications.CountAsync(n => !n.IsRead);
        }
    }
}
