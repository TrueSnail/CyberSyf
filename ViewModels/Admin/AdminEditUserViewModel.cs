using System.ComponentModel.DataAnnotations;

namespace E_Book_Store.ViewModels.Admin
{
    public class AdminEditUserViewModel
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Hasła muszą się zgadzać.")]
        public string? ConfirmPassword { get; set; }
    }
}
