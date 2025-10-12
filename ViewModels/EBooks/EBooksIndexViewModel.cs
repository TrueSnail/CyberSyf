using AutoMapper;
using E_Book_Store.Models;

namespace E_Book_Store.ViewModels.EBooks;

public class EBooksIndexViewModel
{
    public List<EBooksIndexItemViewModel> EBooks { get; set; } = new();
    public List<EBooksIndexItemViewModel> OrderedEBooks { get => EBooks.OrderBy(ebook => ebook.Bought).ThenBy(ebook => ebook.Price).ToList(); }
}
