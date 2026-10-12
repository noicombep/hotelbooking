using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Service;
using BusinessObject;

namespace API_HotelBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] BusinessObject.RegisterRequest req)
        {
            // Kiểm tra model validation
            if (!ModelState.IsValid)
            {
                // Trả về lỗi nếu confirm password không khớp hoặc email/name invalid
                return BadRequest(new
                {
                    Success = false,
                    Errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList()
                });
            }

            // Gọi service đăng ký
            var result = await _service.Register(req.Name, req.Email, req.Password);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] BusinessObject.LoginRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.Login(req.Email, req.Password);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("verify")]
        public async Task<IActionResult> Verify(string token)
        {
            var result = await _service.VerifyEmail(token);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


    }
}
