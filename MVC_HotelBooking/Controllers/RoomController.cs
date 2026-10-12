using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using BusinessObject;
namespace MVC_HotelBooking.Controllers
{


    public class RoomController : BaseController
    {
        private readonly ApiService _api;

        public RoomController(ApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var room = await _api.GetRoomById(id);
                return View(room);
            }
            catch
            {
                return NotFound();
            }
        }


    }
}
