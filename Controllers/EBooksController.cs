using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Book_Store.Models;
using E_Book_Store.Services;
using E_Book_Store.ViewModels.EBooks;
using FluentValidation;
using FormHelper;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace E_Book_Store.Controllers;

[Authorize]
public class EBooksController : Controller
{
    private readonly IEBooksService EBookService;
    private readonly IEBooksContentService EBookContentService;
    private readonly IValidator<EBook> EBookValidator;
    private readonly IMapper Mapper;
    private readonly UserManager<IdentityUser> UserManager;

    public EBooksController(IEBooksService eBookService, IEBooksContentService ebookContentService, IValidator<EBook> eBookValidator, IMapper mapper, UserManager<IdentityUser> userManager)
    {
        EBookService = eBookService;
        EBookContentService = ebookContentService;
        EBookValidator = eBookValidator;
        Mapper = mapper;
        UserManager = userManager;
    }

    // GET: EBooks
    //public IActionResult Index() => View(Mapper.Map<EBooksIndexViewModel>(EBookService.GetByBought(UserManager.GetUserId(User)!, false)));
    public IActionResult Index()
    {
        if (User.IsInRole(nameof(Roles.EBookEditor)))
        {
            return View(new EBooksIndexViewModel() { EBooks = EBookService.GetAll().ToList().Select(ebook => new EBooksIndexItemViewModel() { Id = ebook.Id, Author = ebook.Author, Price = ebook.Price, Title = ebook.Title, Bought = EBookService.IsBought(ebook.Id, UserManager.GetUserId(User)!) }).ToList() });
        }
        else
        {
            return View(Mapper.Map<EBooksIndexViewModel>(EBookService.GetByBought(UserManager.GetUserId(User)!, false)));
        }
    }

    // GET: EBooks/Buy/5
    public IActionResult Buy(string id)
    {
        EBookService.Buy(id, UserManager.GetUserId(User));
        return RedirectToAction(nameof(Index));
    }

    // GET: EBooks/Create
    [Authorize(Roles = nameof(Roles.EBookEditor))]
    public IActionResult Create() => View();

    // POST: EBooks/Create
    [HttpPost]
    [FormValidator]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Roles.EBookEditor))]
    public IActionResult Create(EBooksCreateViewModel model)
    {
        EBook eBook = Mapper.Map<EBook>(model);
        var validationResult = EBookValidator.Validate(eBook);
        if (validationResult.IsValid)
        {
            EBookService.Create(eBook);
            if (!string.IsNullOrEmpty(model.Content)) EBookContentService.SetContent(eBook, model.Content);
            return RedirectToAction(nameof(Index));
        }
        return FormResult.CreateErrorResult(validationResult.Errors[0].ErrorMessage);
    }

    // GET: EBooks/Edit/5
    [Authorize(Roles = nameof(Roles.EBookEditor))]
    public IActionResult Edit(string id)
    {
        var eBook = EBookService.GetById(id);
        return eBook != null ? View(Mapper.Map<EBooksEditViewModel>(eBook)) : NotFound();
    }

    // POST: EBooks/Edit/5
    [HttpPost]
    [FormValidator]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Roles.EBookEditor))]
    public IActionResult Edit(EBooksEditViewModel model)
    {
        EBook eBook = Mapper.Map<EBook>(model);

        var validationResult = EBookValidator.Validate(eBook);
        if (validationResult.IsValid)
        {
            EBookService.Update(eBook);
            if (!string.IsNullOrEmpty(model.Content)) EBookContentService.SetContent(eBook, model.Content);
            return RedirectToAction(nameof(Index));
        }
        return FormResult.CreateErrorResult(validationResult.Errors[0].ErrorMessage);
    }

    // GET: EBooks/Delete/5
    [Authorize(Roles = nameof(Roles.EBookEditor))]
    public IActionResult Delete(string id)
    {
        var eBook = EBookService.GetById(id);
        return eBook != null ? View(Mapper.Map<EBooksDeleteViewModel>(eBook)) : NotFound();
    }

    // POST: EBooks/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Roles.EBookEditor))]
    public IActionResult DeleteConfirmed(EBooksDeleteViewModel model)
    {
        EBookContentService.DeleteContent(EBookService.GetById(model.Id)!);
        EBookService.Delete(model.Id);
        return RedirectToAction(nameof(Index));
    }
}
