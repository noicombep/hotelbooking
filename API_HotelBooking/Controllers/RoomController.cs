using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace API_HotelBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _service;

        public RoomController(IRoomService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _service.GetAll();

           

            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var r = await _service.GetById(id);
            if (r == null) return NotFound();



            return Ok(r);
        }

        // ================= CREATE =================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] RoomCreateRequest req)
        {
            var result = await _service.Create(req);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // ================= UPDATE =================
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] RoomCreateRequest req)
        {
            var result = await _service.Update(id, req);

           

            return Ok(result);
        }

        // ================= DELETE =================
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
