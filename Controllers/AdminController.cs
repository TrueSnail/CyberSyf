using E_Book_Store.Services;
using E_Book_Store.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Book_Store.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> UserManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEBooksService EbooksService;

    public AdminController(UserManager<IdentityUser> userManager, IEBooksService eBookService, RoleManager<IdentityRole> roleManager)
    {
        UserManager = userManager;
        EbooksService = eBookService;
        _roleManager = roleManager;
    }

    public IActionResult Index()
    {
        var users = _userManager.Users.ToList();
        var model = new AdminIndexViewModel(users, ebooksCount: 42) // przykładowo
        {
            EnforcePasswordPolicy = true,
            DefaultPasswordExpiryDays = 90
        };
        return View(model);
    }

    public async Task<IActionResult> Delete(string Id)
    {
        await UserManager.DeleteAsync((await UserManager.FindByIdAsync(Id))!);
        return RedirectToAction(nameof(Index));
    }
}
