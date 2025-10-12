using System.ComponentModel.DataAnnotations;

namespace E_Book_Store.Models;

public class EBook
{
    [Key]
    public string Id { get; set; }
    public required string Author { get; set; }
    public required string Title { get; set; }
    public string? PathToContent { get; set; }
    public required decimal Price { get; set; }

    public EBook()
    {
        Id = Guid.NewGuid().ToString();
    }
}
