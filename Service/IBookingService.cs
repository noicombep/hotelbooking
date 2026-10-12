using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repository
{
    public interface IBookingService
    {
        Task<ApiResponse<string>> CreateBooking(int userId, int roomId, DateTime checkIn, DateTime checkOut);
        Task<ApiResponse<string>> ConfirmBooking(int bookingId);
        Task<ApiResponse<string>> UpdateBookingStatus(int id, string status);
        Task<ApiResponse<string>> UpdatePaymentStatus(int id, string status);

        Task<List<BookingDTO>> GetAllBookings();
        Task<List<BookingDTO>> GetMyBookings(int userId);



    }
}
