using Microsoft.AspNetCore.Identity;

namespace E_Book_Store.Models
{
    public class ApplicationUser : IdentityUser
{
    public bool EnforcePasswordPolicy { get; set; } = true;
    public int PasswordExpiryDays { get; set; } = 90;
}

}
