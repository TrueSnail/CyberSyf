using E_Book_Store.Models;
using E_Book_Store.Services;
using E_Book_Store.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Book_Store.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> UserManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEBooksService EbooksService;

    public AdminController(UserManager<ApplicationUser> userManager, IEBooksService eBookService, RoleManager<IdentityRole> roleManager)
    {
        UserManager = userManager;
        EbooksService = eBookService;
        _roleManager = roleManager;
    }

    public IActionResult Index()
    {
        var users = UserManager.Users.ToList();
        var model = new AdminIndexViewModel(users, ebooksCount: 42)
        {
            EnforcePasswordPolicy = true,
            DefaultPasswordExpiryDays = 90
        };
        return View(model);
    }
    


     public async Task<IActionResult> BlockUser(string id)
    {
        if (string.IsNullOrEmpty(id)) return RedirectToAction(nameof(Index));

        var user = await UserManager.FindByIdAsync(id);
        if (user != null)
        {
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.MaxValue; // blokada na czas nieokreślony
            await UserManager.UpdateAsync(user);
        }
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> UnblockUser(string id)
    {
        if (string.IsNullOrEmpty(id)) return RedirectToAction(nameof(Index));

        var user = await UserManager.FindByIdAsync(id);
        if (user != null)
        {
            user.LockoutEnd = null;
            await UserManager.UpdateAsync(user);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePasswordPolicies(List<PasswordSettingsViewModel> Users)
    {
        foreach (var userModel in Users)
        {
            var user = await UserManager.FindByIdAsync(userModel.UserId);
            if (user is ApplicationUser appUser)
            {
                appUser.EnforcePasswordPolicy = userModel.EnforcePasswordPolicy;
                appUser.PasswordExpiryDays = userModel.PasswordExpiryDays;

                await UserManager.UpdateAsync(appUser);
            }
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> PasswordSettings(PasswordSettingsViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
    
        var user = await UserManager.FindByIdAsync(model.UserId);
        if (user == null) return RedirectToAction(nameof(Index));

        var appUser = (ApplicationUser)user;
        appUser.EnforcePasswordPolicy = model.EnforcePasswordPolicy;
        appUser.PasswordExpiryDays = model.PasswordExpiryDays;

        await UserManager.UpdateAsync(appUser);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> EditUser(string id)
    {
        if (string.IsNullOrEmpty(id)) return RedirectToAction(nameof(Index));

        var user = await UserManager.FindByIdAsync(id);
        if (user == null) return RedirectToAction(nameof(Index));

        var model = new AdminEditUserViewModel
        {
            UserId = id,
            UserName = user.UserName!,
            Email = user.Email
        };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> EditUser(AdminEditUserViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await UserManager.FindByIdAsync(model.UserId);
        if (user == null) return RedirectToAction(nameof(Index));

        user.UserName = model.UserName;
        user.Email = model.Email;

        var updateResult = await UserManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            foreach (var error in updateResult.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);
        }

        // Jeśli hasło zostało podane i nie jest puste, zmień je
        if (!string.IsNullOrWhiteSpace(model.NewPassword))
        {
            // Usuń stare hasło
            var removePassResult = await UserManager.RemovePasswordAsync(user);
            if (!removePassResult.Succeeded)
            {
                foreach (var error in removePassResult.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }

            var addPassResult = await UserManager.AddPasswordAsync(user, model.NewPassword);
            if (!addPassResult.Succeeded)
            {
                foreach (var error in addPassResult.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }
        }

        return RedirectToAction(nameof(Index));
    }

    //private bool IsUserBlocked(IdentityUser user)
    //{
    //    return user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow;
    //}

    public async Task<IActionResult> Delete(string Id)
    {
        await UserManager.DeleteAsync((await UserManager.FindByIdAsync(Id))!);
        return RedirectToAction(nameof(Index));
    }
}
