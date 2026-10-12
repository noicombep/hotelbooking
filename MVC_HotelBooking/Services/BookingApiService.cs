//namespace MVC_HotelBooking.Services
//{
//    public class BookingApiService
//    {
//        private readonly HttpClient _http;

//        public BookingApiService(HttpClient http)
//        {
//            _http = http;
//        }

//        public async Task<List<RoomDTO>> GetRooms()
//        {
//            return await _http.GetFromJsonAsync<List<RoomDTO>>("api/room");
//        }

//        public async Task<(bool, string)> CreateBooking(CreateBookingVM model)
//        {
//            var res = await _http.PostAsJsonAsync("api/booking", new
//            {
//                userId = model.UserId,
//                roomId = model.RoomId,
//                checkIn = model.CheckIn,
//                checkOut = model.CheckOut
//            });

//            if (!res.IsSuccessStatusCode)
//            {
//                var msg = await res.Content.ReadAsStringAsync();
//                return (false, msg);
//            }

//            return (true, "Đặt phòng thành công");
//        }
//    }
//}
