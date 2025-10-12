using E_Book_Store.Models;

namespace E_Book_Store.ViewModels.EBooks;

public class EBooksIndexItemViewModel
{
    public string Id { get; set; } = "";
    public string Author { get; set; } = "";
    public string Title { get; set; } = "";
    public decimal Price { get; set; }
    public bool Bought { get; set; }
}
