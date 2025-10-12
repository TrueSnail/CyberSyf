using E_Book_Store.Models;

namespace E_Book_Store.ViewModels.EBooks;

public class EBooksDeleteViewModel
{
    public string Id { get; set; } = "";
    public string Author { get; set; } = "";
    public string Title { get; set; } = "";
    public string? PathToContent { get; set; } = "";
    public decimal Price { get; set; }
}
