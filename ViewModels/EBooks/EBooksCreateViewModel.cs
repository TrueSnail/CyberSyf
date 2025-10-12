using E_Book_Store.Models;
using System.ComponentModel.DataAnnotations;

namespace E_Book_Store.ViewModels.EBooks;

public class EBooksCreateViewModel
{
    public string Author { get; set; } = "";
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    [Range(0, 1_000_000)]
    public decimal Price { get; set; }
}
