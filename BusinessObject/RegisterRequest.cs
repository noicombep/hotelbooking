using System.ComponentModel.DataAnnotations;

namespace BusinessObject
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Name không được để trống")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password không được để trống")]
        [MinLength(6, ErrorMessage = "Password ít nhất 6 ký tự")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password không được để trống")]
        [Compare("Password", ErrorMessage = "Confirm Password phải giống Password")]
        public string ConfirmPassword { get; set; }
    }
}