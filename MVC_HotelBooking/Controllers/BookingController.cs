using Microsoft.AspNetCore.Mvc;

namespace MVC_HotelBooking.Controllers
{
using Microsoft.AspNetCore.Mvc;
using BusinessObject;

public class BookingController : BaseController
    {
    private readonly ApiService _api;

    public BookingController(ApiService api)
    {
        _api = api;
    }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                TempData["Message"] = "Bạn cần đăng nhập để xem booking";
                return RedirectToAction("Login", "Account");
            }

            var bookings = await _api.GetMyBookings(); // trả về List<BookingDTO>
            return View(bookings);
        }

        // 🟢 Update trạng thái (MVC gọi API)


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingRequest request)
        {
            if (request == null)
                return Json(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Request không hợp lệ"
                });

            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return Json(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Chưa đăng nhập"
                });

            var result = await _api.BookRoom(request); // trả về ApiResponse<string>
            return Json(result); // ✅ trả JSON cho JS
        }
    }
}
