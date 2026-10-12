using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessObject;

public class AdminController : BaseController
{
    private readonly ApiService _api;

    public AdminController(ApiService api)
    {
        _api = api;
    }

    // ================= DASHBOARD =================
    public async Task<IActionResult> Dashboard()
    {
        // Lấy tổng quan
        var bookings = await _api.GetBookings();
        var rooms = await _api.GetRooms();

        ViewBag.TotalBookings = bookings.Count;
        ViewBag.TotalRooms = rooms.Count;
        ViewBag.TotalRevenue = bookings.Where(b=>b.Status== "Approved" &&b.PaymentStatus=="Paid").Sum(b => b.TotalPrice);

        ViewBag.PendingBookings = bookings.Count(b => b.Status == "Pending");
        ViewBag.ApprovedBookings = bookings.Count(b => b.Status == "Approved");
        ViewBag.CanceledBookings = bookings.Count(b => b.Status == "Canceled");

        // 🔥 Gọi API riêng để lấy Revenue by Room Type
        var revenueByType = await _api.GetRevenueByRoomType();

        ViewBag.RoomTypeNames = revenueByType.Select(r => r.RoomTypeName).ToList();
        ViewBag.RevenueByRoomType = revenueByType.Select(r => r.Revenue).ToList();
        ViewBag.BookingCountByType = revenueByType.Select(r => r.BookingCount).ToList();

        return View();
    }


    // ================= ROOM TYPES =================

    public async Task<IActionResult> RoomTypes()
    {
        var types = await _api.GetRoomTypes();
        return View(types);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoomType(string Name)
    {
        await _api.CreateRoomType(Name ); // nếu chưa có DTO thì dùng object
        return RedirectToAction("RoomTypes");
    }

    public async Task<IActionResult> DeleteRoomType(int id)
    {
        await _api.DeleteRoomType(id);
        return RedirectToAction("RoomTypes");
    }

    // ================= ROOMS =================

    public async Task<IActionResult> Rooms()
    {
        var prices = await _api.GetRevenueByRoom();
        ViewBag.Prices = prices;

        var rooms = await _api.GetRooms();
        return View(rooms);
    }

    public async Task<IActionResult> CreateRoom()
    {
        var types = await _api.GetRoomTypes();
        ViewBag.RoomTypes = types;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom(RoomCreateRequest req)
    {
        var result = await _api.CreateRoom(req);
        return Json(result);
    }

    public async Task<IActionResult> DeleteRoom(int id)
    {
        var message = await _api.DeleteRoom(id);

        TempData["Message"] = message.Message;

        return RedirectToAction("Rooms");
    }

    public async Task<IActionResult> UpdateRoom(int id)
    {
        var room = await _api.GetRoomById(id); // API trả RoomDTO
        var types = await _api.GetRoomTypes();
        ViewBag.RoomId = id;
        var roomUpdate = new RoomCreateRequest
        {
            RoomNumber = room.RoomNumber,
            RoomTypeId = room.TypeId,
            Capacity = room.Capacity,
            Price = room.Price,
            Status = room.Status,
            Description = room.Description,
            Images = null // hoặc để trống, preview ảnh cũ xử lý khác
        };
        ViewBag.RoomTypes = types;
        ViewBag.ExistingImages = room.Images;
        return View(roomUpdate); // model là RoomDTO
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRoom(int id, RoomCreateRequest req)
    {
        // gọi API truyền id và object
        var result = await _api.UpdateRoom(id, req);

        if (result.Success)
        {
            TempData["Message"] = "Cập nhật phòng thành công";
            return RedirectToAction("Rooms");
        }
        else
        {
            // load RoomTypes lại để quay lại form
            ViewBag.RoomId = id;
            ViewBag.RoomTypes = await _api.GetRoomTypes();
            ViewBag.Error = result.Message;
            return View(req); // giữ dữ liệu cũ trên form
        }
    }

    // ================= BOOKINGS =================

    [HttpGet]
    public async Task<IActionResult> GetFilteredBookings(string user, string room, string status, string payment, DateTime? from, DateTime? to)
    {
        var bookings = await _api.GetBookings();

        if (!string.IsNullOrEmpty(user))
            bookings = bookings.Where(b => b.UserName.Contains(user, StringComparison.OrdinalIgnoreCase)).ToList();
        if (!string.IsNullOrEmpty(room))
            bookings = bookings.Where(b => b.RoomName.Contains(room, StringComparison.OrdinalIgnoreCase)).ToList();
        if (!string.IsNullOrEmpty(status))
            bookings = bookings.Where(b => b.Status == status).ToList();
        if (!string.IsNullOrEmpty(payment))
            bookings = bookings.Where(b => b.PaymentStatus == payment).ToList();
        if (from.HasValue)
            bookings = bookings.Where(b => b.CheckIn >= from.Value).ToList();
        if (to.HasValue)
            bookings = bookings.Where(b => b.CheckOut <= to.Value).ToList();

        return Json(bookings);
    }

    public async Task<IActionResult> Bookings()
    {
        var bookings = await _api.GetBookings();
        return View(bookings);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        var token = HttpContext.Session.GetString("token");
        if (string.IsNullOrEmpty(token))
        {
            return Json(new { Success = false, Message = "Chưa đăng nhập" });
        }

        var result = await _api.UpdateBookingStatus(id, status); // ApiResponse<string>
        return Json(result);
    }

    // ================= USERS =================
    public async Task<IActionResult> User()
    {
        var users = await _api.GetUsers(); // giả sử API trả List<UserDTO>
        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> EditUserRole(int id)
    {
        var user = await _api.GetUserById(id);
        if (user == null) return NotFound();

        var model = new UpdateUserRoleDto { Role = user.Role };
        ViewBag.UserId = id;
        ViewBag.UserName = user.Name;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditUserRole(int id, [FromBody] UpdateUserRoleDto model)
    {
        if (model == null || string.IsNullOrEmpty(model.Role))
            return BadRequest(new { Success = false, Message = "Role không hợp lệ" });

        try
        {
            await _api.UpdateUserRole(id, model);
            return Ok(new { Success = true, Message = "Cập nhật role thành công" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Success = false, Message = ex.Message });
        }
    }

}