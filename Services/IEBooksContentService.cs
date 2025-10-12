using E_Book_Store.Models;

namespace E_Book_Store.Services;

public interface IEBooksContentService
{
    public string GetContent(EBook eBook);
    public void SetContent(EBook eBook, string content);
    public void DeleteContent(EBook eBook);
}
