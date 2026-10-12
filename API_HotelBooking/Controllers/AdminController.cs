using BusinessObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_HotelBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly HotelBookingDbContext _context;

        public AdminController(HotelBookingDbContext context)
        {
            _context = context;
        }

        [HttpGet("revenue-by-roomtype")]
        public async Task<IActionResult> GetRevenueByRoomType()
        {
            // Lấy tất cả bookings có liên quan room
            var bookings = await _context.Bookings.Where(b=>b.Status=="Approved" && b.PaymentStatus == "Paid")
                .Include(b => b.Room)
                .ThenInclude(r => r.RoomType)
                .ToListAsync();

            // Tính doanh thu theo RoomType
            var revenueByType = bookings
                .GroupBy(b => b.Room.RoomType.Name)
                .Select(g => new
                {
                    RoomTypeName = g.Key,
                    Revenue = g.Sum(b => b.TotalPrice),
                    BookingCount = g.Count() // thêm nếu muốn thống kê số booking theo type
                })
                .ToList();

            return Ok(revenueByType);
        }
        [HttpGet("price")]
        public async Task<IActionResult> GetRevenueByRoom()
        {
            var bookings = await _context.Bookings.Where(b => b.Status == "Approved" && b.PaymentStatus == "Paid")
                           .Include(b => b.Room)
                           .ThenInclude(r => r.RoomType)
                           .ToListAsync();

            // Tính doanh thu theo RoomType
            var revenueByType = bookings
                .GroupBy(b => b.Room.Id)
                .Select(g => new
                {
                    RoomId = g.Key,
                    Revenue = g.Sum(b => b.TotalPrice),
                    BookingCount = g.Count() 
                })
                .ToList();

            return Ok(revenueByType);
        }


    }
}
