using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repository
{
    public class EmailLogRepository : IEmailLogRepository
    {
        private readonly HotelBookingDbContext _context;

        public EmailLogRepository(HotelBookingDbContext context)
        {
            _context = context;
        }

        public async Task Add(EmailLog log)
        {
            _context.EmailLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
