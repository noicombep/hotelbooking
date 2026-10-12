using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_HotelBooking.Hubs;
using BusinessObject;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IRoomRepository _roomRepo;
        private readonly INotificationRepository _notiRepo;
        private readonly IEmailLogRepository _emailRepo;
        private readonly IEmailService _emailService;
        private readonly IHubContext<NotificationHub> _hub;

        public BookingService(
            IBookingRepository bookingRepo,
            IRoomRepository roomRepo,
            INotificationRepository notiRepo,
            IEmailLogRepository emailRepo,
            IEmailService emailService)
        {
            _bookingRepo = bookingRepo;
            _roomRepo = roomRepo;
            _notiRepo = notiRepo;
            _emailRepo = emailRepo;
            _emailService = emailService;
        }

        public async Task<ApiResponse<string>> CreateBooking(int userId, int roomId, DateTime checkIn, DateTime checkOut)
        {
            if (checkIn >= checkOut)
                return new ApiResponse<string> { Success = false, Message = "Ngày không hợp lệ" };

            var room = await _roomRepo.GetById(roomId);
            if (room == null)
                return new ApiResponse<string> { Success = false, Message = "Không tìm thấy phòng" };

            using var transaction = await _bookingRepo.BeginTransactionAsync();

            try
            {
                // 🔥 check lại trong transaction (tránh double booking)
                var isAvailable = await _bookingRepo.IsRoomAvailable(roomId, checkIn, checkOut);
                if (!isAvailable)
                    return new ApiResponse<string> { Success = false, Message = "Phòng đã được đặt" };

                var totalPrice = (checkOut - checkIn).Days * room.Price;

                var booking = await _bookingRepo.CreateBooking(new Booking
                {
                    UserId = userId,
                    RoomId = roomId,
                    CheckIn = checkIn,
                    CheckOut = checkOut,
                    TotalPrice = totalPrice,
                    Status = "Pending",
                    PaymentStatus = "Unpaid",
                    CreatedAt = DateTime.Now
                });



                await _notiRepo.Add(new Notification
                {
                    Title = "New Booking",
                    Message = $"User {userId} vừa đặt phòng {room.RoomNumber}",
                    UserId = userId
                });
                //var unreadCount = await _notiRepo.GetUnreadCount();

                //await _hub.Clients.All.SendAsync("ReceiveUnreadCount", unreadCount);
                await transaction.CommitAsync();

                return new ApiResponse<string>
                {
                    Success = true,
                    Message = "Đặt phòng thành công"
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ApiResponse<string>> ConfirmBooking(int bookingId)
        {
            var booking = await _bookingRepo.GetById(bookingId);
            if (booking == null)
                return new ApiResponse<string> { Success = false, Message = "Không tìm thấy booking" };

            booking.Status = "Approved";



            // 🔥 lấy email thật từ user
            var user = booking.User;

            await _emailService.SendAsync(
user.Email,
"Xác nhận đặt phòng",
$"Booking #{booking.Room.RoomNumber} đã được xác nhận"
);
            await _emailRepo.Add(new EmailLog
            {
                ToEmail = user.Email,
                Subject = "Xác nhận đặt phòng",
                Body = $"Booking #{booking.Room.RoomNumber} đã được xác nhận",
                Status = "Sent"
            });

            await _bookingRepo.Update(booking);
            return new ApiResponse<string>
            {
                Success = true,
                Message = "Đã xác nhận booking"
            };
        }
        public async Task<ApiResponse<string>> UpdatePaymentStatus(int id, string status)
        {
            var booking = await _bookingRepo.GetById(id);
            if (booking == null)
                return new ApiResponse<string> { Success = false, Message = "Booking không tồn tại" };

            booking.PaymentStatus = "Paid";
            await _bookingRepo.Update(booking);

            var user = booking.User;


            return new ApiResponse<string> { Success = true, Message = $"Payments status updated to {status}" };
        }
        public async Task<ApiResponse<string>> UpdateBookingStatus(int id, string status)
        {
            var booking = await _bookingRepo.GetById(id);
            if (booking == null)
                return new ApiResponse<string> { Success = false, Message = "Booking không tồn tại" };

            if (status != "Pending" && status != "Approved" && status != "Canceled")
                return new ApiResponse<string> { Success = false, Message = "Status không hợp lệ" };

            booking.Status = status;
            await _bookingRepo.Update(booking);

            var user = booking.User;


            await _emailService.SendAsync(
user.Email,
"Xác nhận đặt phòng",
$"Booking #{booking.Room.RoomNumber} đã bị từ chối"
);
            await _emailRepo.Add(new EmailLog
            {
                ToEmail = user.Email,
                Subject = "Xác nhận đặt phòng",
                Body = $"Booking #{booking.Room.RoomNumber} đã bị từ chối",
                Status = "Sent"
            });

            return new ApiResponse<string> { Success = true, Message = $"Booking status updated to {status}" };
        }


        public async Task<List<BookingDTO>> GetAllBookings()
        {
            var bookings = await _bookingRepo.GetAllBookingsQuery()
                .Select(b => new BookingDTO
                {
                    Id = b.Id,
                    UserName = b.User.Name,       // lấy tên user
                    RoomName = b.Room.RoomNumber,       // lấy tên phòng
                    CheckIn = b.CheckIn,
                    CheckOut = b.CheckOut,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status,
                    Note = b.Note,
                    PaymentStatus = b.PaymentStatus,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();

            return bookings;
        }

        public async Task<List<BookingDTO>> GetMyBookings(int userId)
        {
            var bookings = await _bookingRepo.GetAllBookingsQuery().Where(a=>a.UserId == userId)
                .Select(b => new BookingDTO
                {
                    Id = b.Id,
                    UserName = b.User.Name,       // lấy tên user
                    RoomName = b.Room.RoomNumber,       // lấy tên phòng
                    CheckIn = b.CheckIn,
                    CheckOut = b.CheckOut,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status,
                    Note = b.Note,
                    PaymentStatus = b.PaymentStatus,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();

            return bookings;
        }

    }

}
