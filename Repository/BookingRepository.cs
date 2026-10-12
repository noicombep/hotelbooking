using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelBookingDbContext _context;

        public BookingRepository(HotelBookingDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsRoomAvailable(int roomId, DateTime checkIn, DateTime checkOut)
        {
            if (checkIn >= checkOut)
                return false;

            var activeStatuses = new[] { "Pending", "Approved" };

            return !await _context.Bookings
                .Where(bd => bd.RoomId == roomId)
                .Where(bd => activeStatuses.Contains(bd.Status))
                .AnyAsync(bd =>
                    bd.CheckIn < checkOut &&
                    bd.CheckOut > checkIn
                );
        }

        public async Task<Booking> CreateBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }


        public async Task<Booking> GetById(int id)
        {
            return await _context.Bookings
                                 .Include(b => b.User)
                                 .Include(b=>b.Room)// Load luôn User
                                 .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task Update(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        public IQueryable<Booking> GetAllBookingsQuery()
        {
            return _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room);
        }


    }
}
