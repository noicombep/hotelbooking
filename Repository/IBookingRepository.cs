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
    public interface IBookingRepository
    {
        Task<bool> IsRoomAvailable(int roomId, DateTime checkIn, DateTime checkOut);
        Task<Booking> CreateBooking(Booking booking);
        Task<Booking?> GetById(int id);
        Task Update(Booking booking);
        Task<IDbContextTransaction> BeginTransactionAsync();
        IQueryable<Booking> GetAllBookingsQuery();

    }
}
