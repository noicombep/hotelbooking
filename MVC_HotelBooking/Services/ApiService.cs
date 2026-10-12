using System.Net.Http.Headers;
using System.Net.Http.Json;
using BusinessObject;
using Microsoft.AspNetCore.Identity.Data;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _context;

    public ApiService(HttpClient http, IHttpContextAccessor context)
    {
        _http = http;
        _context = context;
    }

    private void AddToken()
    {
        var token = _context.HttpContext.Session.GetString("token");
        _http.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrEmpty(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }


    public async Task<ApiResponse<string>> Login(BusinessObject.LoginRequest req)
    {
        var res = await _http.PostAsJsonAsync("api/auth/login", req);
        return await res.Content.ReadFromJsonAsync<ApiResponse<string>>();
    }

    public async Task<ApiResponse<string>> Register(BusinessObject.RegisterRequest req)
    {
        var res = await _http.PostAsJsonAsync("api/auth/register", req);
        return await res.Content.ReadFromJsonAsync<ApiResponse<string>>();
    }

    public async Task<List<RoomDTO>> GetRooms()
    {
        var res = await _http.GetAsync("api/room");

        if (!res.IsSuccessStatusCode)
            throw new Exception("API lỗi");

        return await res.Content.ReadFromJsonAsync<List<RoomDTO>>();
    }
    public async Task<RoomDTO> GetRoomById(int id)
    {
        var res = await _http.GetAsync($"api/room/{id}");

        if (!res.IsSuccessStatusCode)
            throw new Exception("Không tìm thấy phòng");

        return await res.Content.ReadFromJsonAsync<RoomDTO>();
    }

    public async Task<ApiResponse<string>> BookRoom(CreateBookingRequest req)
    {
        AddToken(); // 🔥

        var res = await _http.PostAsJsonAsync("api/booking", req);

        ApiResponse<string> result;

        try
        {
            result = await res.Content.ReadFromJsonAsync<ApiResponse<string>>();
        }
        catch
        {
            result = new ApiResponse<string>
            {
                Success = false,
                Message = "Đã xảy ra lỗi khi gửi request"
            };
        }

        if (!res.IsSuccessStatusCode)
        {
            // Nếu API trả lỗi, vẫn trả object với message từ API
            if (result == null)
            {
                result = new ApiResponse<string>
                {
                    Success = false,
                    Message = "Booking thất bại"
                };
            }
            else
            {
                result.Success = false; // đảm bảo Success=false
            }
        }

        return result;
    }



    public async Task<List<BookingDTO>> GetBookings()
    {
        AddToken();
        var res = await _http.GetAsync("api/booking");
        if (!res.IsSuccessStatusCode) throw new Exception("API lỗi");
        return await res.Content.ReadFromJsonAsync<List<BookingDTO>>();
    }
    public async Task<List<BookingDTO>> GetMyBookings()
    {
        AddToken();
        var res = await _http.GetAsync("api/booking/my-booking");
        if (!res.IsSuccessStatusCode) throw new Exception("API lỗi");
        return await res.Content.ReadFromJsonAsync<List<BookingDTO>>();
    }

    public async Task<List<RoomTypeDTO>> GetRoomTypes()
    {
        AddToken();
        var res = await _http.GetAsync("api/roomtype");
        if (!res.IsSuccessStatusCode) throw new Exception("API lỗi");
        return await res.Content.ReadFromJsonAsync<List<RoomTypeDTO>>();
    }

    public async Task<ApiResponse<string>> UpdateBookingStatus(int id, string status)
    {
        AddToken();
        var res = await _http.PutAsJsonAsync($"api/booking/{id}/status", status);
        return await res.Content.ReadFromJsonAsync<ApiResponse<string>>();
    }
    public async Task<ApiResponse<string>> CreateRoom(RoomCreateRequest req)
    {
        AddToken();

        var form = new MultipartFormDataContent();

        form.Add(new StringContent(req.RoomNumber), "RoomNumber");
        form.Add(new StringContent(req.Capacity.ToString()), "Capacity");
        form.Add(new StringContent(req.Price.ToString()), "Price");
        form.Add(new StringContent(req.Status), "Status");
        form.Add(new StringContent(req.Description ?? ""), "Description");

        if (req.RoomTypeId != null)
            form.Add(new StringContent(req.RoomTypeId.ToString()), "RoomTypeId");

        if (req.Images != null)
        {
            foreach (var file in req.Images)
            {
                var stream = file.OpenReadStream();
                form.Add(new StreamContent(stream), "Images", file.FileName);
            }
        }

        var res = await _http.PostAsync("api/room", form);
        return await res.Content.ReadFromJsonAsync<ApiResponse<string>>();
    }

    public async Task<ApiResponse<string>> DeleteRoom(int id)
    {
        AddToken();
        var res= await _http.DeleteAsync($"api/room/{id}");
        return await res.Content.ReadFromJsonAsync<ApiResponse<string>>();

    }

    public async Task<ApiResponse<string>> CreateRoomType(string name)
    {
        AddToken();
        var res = await _http.PostAsJsonAsync("api/roomtype", new { Name = name });
        return await res.Content.ReadFromJsonAsync<ApiResponse<string>>();

    }

    public async Task<ApiResponse<string>> DeleteRoomType(int id)
    {
        AddToken();
        var res = await _http.DeleteAsync($"api/roomtype/{id}");
        return await res.Content.ReadFromJsonAsync<ApiResponse<string>>();

    }
    public async Task<ApiResponse<string>> UpdateRoom(int id, RoomCreateRequest req)
    {
        AddToken();

        var form = new MultipartFormDataContent();

        form.Add(new StringContent(req.RoomNumber), "RoomNumber");
        form.Add(new StringContent(req.Capacity.ToString()), "Capacity");
        form.Add(new StringContent(req.Price.ToString()), "Price");
        form.Add(new StringContent(req.Status), "Status");
        form.Add(new StringContent(req.Description ?? ""), "Description");

        if (req.RoomTypeId != null)
            form.Add(new StringContent(req.RoomTypeId.ToString()), "RoomTypeId");

        if (req.Images != null)
        {
            foreach (var file in req.Images)
            {
                var stream = file.OpenReadStream();
                form.Add(new StreamContent(stream), "Images", file.FileName);
            }
        }

        var res = await _http.PutAsync($"api/room/{id}", form);
        return await res.Content.ReadFromJsonAsync<ApiResponse<string>>();
    }



    // Lấy tất cả users
    public async Task<List<User>> GetUsers()
    {
        AddToken();
        var res = await _http.GetAsync("api/user");
        if (!res.IsSuccessStatusCode)
            throw new Exception("API lỗi khi lấy danh sách user");

        return await res.Content.ReadFromJsonAsync<List<User>>();
    }

    // Lấy user theo id
    public async Task<User> GetUserById(int id)
    {
        AddToken();
        var res = await _http.GetAsync($"api/user/{id}");
        if (!res.IsSuccessStatusCode)
            throw new Exception("Không tìm thấy user");

        return await res.Content.ReadFromJsonAsync<User>();
    }

    // Cập nhật role user
    public async Task<User> UpdateUserRole(int id, UpdateUserRoleDto req)
    {
        AddToken();
        var res = await _http.PutAsJsonAsync($"api/user/{id}", req);
        if (!res.IsSuccessStatusCode)
            throw new Exception("Cập nhật role thất bại");

        return await res.Content.ReadFromJsonAsync<User>();
    }

    public async Task<List<RevenueByRoomTypeDto>> GetRevenueByRoomType()
    {
        AddToken();
        var res = await _http.GetAsync("api/admin/revenue-by-roomtype");
        if (!res.IsSuccessStatusCode)
            throw new Exception("API lỗi khi lấy revenue");

        return await res.Content.ReadFromJsonAsync<List<RevenueByRoomTypeDto>>();
    }
    public async Task<List<RevenueByRoomDto>> GetRevenueByRoom()
    {
        AddToken();
        var res = await _http.GetAsync("api/admin/price");
        if (!res.IsSuccessStatusCode)
            throw new Exception("API lỗi khi lấy revenue");

        return await res.Content.ReadFromJsonAsync<List<RevenueByRoomDto>>();
    }

    // DTO để nhận dữ liệu
    public class RevenueByRoomTypeDto
    {
        public string RoomTypeName { get; set; } = null!;
        public decimal Revenue { get; set; }
        public int BookingCount { get; set; }
    }

    public class RevenueByRoomDto
    {
        public int RoomId { get; set; } 
        public decimal Revenue { get; set; }
        public int BookingCount { get; set; }
    }
}