using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessObject;
using Service;

namespace API_HotelBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomTypeController : ControllerBase
    {
        private readonly IRoomTypeService _service;

        public RoomTypeController(IRoomTypeService service)
        {
            _service = service;
        }

        // ================= GET ALL =================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }

        // ================= CREATE =================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomTypeDTO req)
        {
            var result = await _service.Create(req.Name);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // ================= UPDATE =================
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoomTypeDTO req)
        {
            var result = await _service.Update(id, req.Name);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // ================= DELETE =================
        [Authorize(Roles = "Admin")]
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