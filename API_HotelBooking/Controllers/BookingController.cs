using System.Security.Claims;
using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace API_HotelBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingController(IBookingService service)
        {
            _service = service;
        }

        // 🟢 Tạo booking
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingRequest request)
        {

            if (request == null)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Request không hợp lệ"
                });
            }
            // 🔥 LẤY USER ID TỪ TOKEN
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Token không hợp lệ"
                });
            }

            var userId = int.Parse(userIdClaim);
            var result = await _service.CreateBooking(
                userId,
                request.RoomId,
                request.CheckIn,
                request.CheckOut);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // 🟢 Xác nhận booking
        [HttpPut("confirm/{id}")]
        public async Task<IActionResult> Confirm(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Id không hợp lệ"
                });
            }

            var result = await _service.ConfirmBooking(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status )
        {
            ApiResponse<string> result ;
            if(status == "Paid")
            {
                result = await _service.UpdatePaymentStatus(id, status);

            }else
            if (status == "Approved")
            {
                 result = await _service.ConfirmBooking(id);
            }
            else
            {
                 result = await _service.UpdateBookingStatus(id, status);

            }
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _service.GetAllBookings();
            return Ok(bookings);
        }

        [Authorize]
        [HttpGet("my-booking")]
        public async Task<IActionResult> GetMyBookings()
        {
            // 🔥 LẤY USER ID TỪ TOKEN
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Token không hợp lệ"
                });
            }

            var userId = int.Parse(userIdClaim);
            var bookings = await _service.GetMyBookings(userId);
            return Ok(bookings);
        }
    }
}