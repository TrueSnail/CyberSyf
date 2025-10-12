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
    private readonly IEBooksService EbooksService;

    public AdminController(UserManager<IdentityUser> userManager, IEBooksService eBookService)
    {
        UserManager = userManager;
        EbooksService = eBookService;
    }

    public IActionResult Index()
    {
        return View(new AdminIndexViewModel(UserManager.Users.ToList(), EbooksService.GetEbookCount()));
    }

    public async Task<IActionResult> Delete(string Id)
    {
        await UserManager.DeleteAsync((await UserManager.FindByIdAsync(Id))!);
        return RedirectToAction(nameof(Index));
    }
}
