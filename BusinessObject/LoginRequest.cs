using System.ComponentModel.DataAnnotations;

namespace BusinessObject
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password không được để trống")]
        [MinLength(6, ErrorMessage = "Password ít nhất 6 ký tự")]
        public string Password { get; set; }
    }
}