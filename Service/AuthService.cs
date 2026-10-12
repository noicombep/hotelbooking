using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using Repository;

namespace Service
{
    public class AuthService: IAuthService
    {
        IUserRepository _userRepo;
        IEmailService _emailService;
        IJwtService _jwtService;
        public AuthService(
    IUserRepository userRepo,
    IEmailService emailService,
    IJwtService jwtService)
        {
            _userRepo = userRepo;
            _emailService = emailService; // 🔥 thiếu cái này là lỗi ngay
            _jwtService = jwtService;
        }

        public async Task<ApiResponse<string>> Register(string name, string email, string password)
        {
            var exist = await _userRepo.GetByEmail(email);
            if (exist != null)
                return new ApiResponse<string> { Success = false, Message = "Email đã tồn tại" };

            var token = Guid.NewGuid().ToString();

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = "Customer",
                IsActive = true,
                EmailConfirmed = false, // ❗ chưa verify
                VerifyToken = token,
                CreatedAt = DateTime.Now
            };

            await _userRepo.Add(user);

            var verifyLink = $"https://localhost:7131/api/auth/verify?token={token}";

            await _emailService.SendAsync(
                email,
                "Xác nhận email",
                $"Click để xác nhận tài khoản: <a href='{verifyLink}'>Verify</a>"
            );

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Đăng ký thành công, kiểm tra email để xác nhận"
            };
        }
        public async Task<ApiResponse<string>> Login(string email, string password)
        {
            var user = await _userRepo.GetByEmail(email);

            if (user == null)
                return new ApiResponse<string> { Success = false, Message = "Sai email hoặc password" };

            if (!(bool)user.EmailConfirmed)
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Vui lòng xác nhận email trước khi đăng nhập"
                };

            var valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!valid)
                return new ApiResponse<string> { Success = false, Message = "Sai email hoặc password" };

            var token = _jwtService.GenerateToken(user);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Đăng nhập thành công",
                Data = token
            };
        }

        public async Task<ApiResponse<string>> VerifyEmail(string token)
        {
            var user = await _userRepo.GetByVerifyToken(token);

            if (user == null)
                return new ApiResponse<string> { Success = false, Message = "Token không hợp lệ" };

            user.EmailConfirmed = true;
            user.VerifyToken = null;

            await _userRepo.Update(user);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Xác nhận email thành công"
            };
        }

    }
}
