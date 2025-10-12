using E_Book_Store.Services;
using E_Book_Store.ViewModels.MyEBooks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Book_Store.Controllers;

[Authorize]
public class MyEBooksController : Controller
{
    private readonly IEBooksService EBookService;
    private readonly IEBooksContentService EBooksContentService;
    private readonly UserManager<IdentityUser> UserManager;

    public MyEBooksController(IEBooksService eBookService, IEBooksContentService ebookContentService, UserManager<IdentityUser> userManager)
    {
        EBooksContentService = ebookContentService;
        EBookService = eBookService;
        UserManager = userManager;
    }

    public IActionResult Index()
    {
        return View(EBookService.GetByBought(UserManager.GetUserId(User)!, true).Select(ebook => new MyEBooksItemViewModel() { Author = ebook.Author, Title = ebook.Title, Id = ebook.Id}));
    }

    public IActionResult Read(string id)
    {
        var eBook = EBookService.GetById(id);
        if (eBook == null) return NotFound();

        if (EBookService.IsBought(id, UserManager.GetUserId(User)!))
        {
            return View(new MyEBooksReadViewModel() { Author = eBook.Author, Content = EBooksContentService.GetContent(eBook), Title = eBook.Title });
        }
        else
        {
            return Forbid();
        }
    }
}
