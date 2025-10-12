using E_Book_Store.Models;
using Microsoft.AspNetCore.Identity;

namespace E_Book_Store.Validation
{
    public class CustomPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : ApplicationUser
{
    public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, 
        TUser user, string password)
    {
        if (!user.EnforcePasswordPolicy)
            return Task.FromResult(IdentityResult.Success);

        if (!password.Any(char.IsUpper))
            return Task.FromResult(IdentityResult.Failed(
                new IdentityError { Description = "Password must contain at least one uppercase letter." }));

        int digitCount = password.Count(char.IsDigit);
        if (digitCount < 2)
            return Task.FromResult(IdentityResult.Failed(
                new IdentityError { Description = "Password must contain at least two digits." }));

        return Task.FromResult(IdentityResult.Success);
    }
}

}
