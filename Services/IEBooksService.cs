using E_Book_Store.Models;

namespace E_Book_Store.Services;

public interface IEBooksService
{
    public IEnumerable<EBook> GetPage(int pageSize, int pageNumber);
    public IEnumerable<EBook> GetAll();
    public EBook? GetById(string Id);
    public void Create(EBook eBook);
    public void Update(EBook eBook);
    public void Delete(string Id);
    public void Buy(string ebookId, string? userId);
    public IEnumerable<EBook> GetByBought(string userId, bool areBought);
    bool IsBought(string ebookId, string userId);
    public int GetEbookCount();
}
