using E_Book_Store.Data;
using E_Book_Store.Models;
using Microsoft.AspNetCore.Identity;

namespace E_Book_Store.Services;

public class EBooksService : IEBooksService
{
    private readonly IRepository<EBook> EBookRepository;
    private readonly IRepository<EBookPurchase> EBookPurchaseRepository;

    public EBooksService(IRepository<EBook> eBookRepository, IRepository<EBookPurchase> eBookPurchaseRepository)
    {
        EBookRepository = eBookRepository;
        EBookPurchaseRepository = eBookPurchaseRepository;
    }

    public void Create(EBook eBook)
    {
        EBookRepository.Insert(eBook);
        EBookRepository.Save();
    }

    public void Delete(string Id)
    {
        var eBook = EBookRepository.GetById(Id);
        if (eBook != null) EBookRepository.Delete(Id);
        EBookRepository.Save();
    }

    public IEnumerable<EBook> GetAll() => EBookRepository.GetAll();

    public int GetEbookCount() => EBookRepository.GetAll().Count();

    public IEnumerable<EBook> GetByBought(string userId, bool areBought)
    {
        var purchases = EBookPurchaseRepository.GetAll().Where(p => p.UserId == userId);
        return EBookRepository.GetAll().Where(b => areBought ^ !purchases.Select(p => p.EBookId).Contains(b.Id));
    }

    public bool IsBought(string ebookId, string userId)
    {
        return EBookPurchaseRepository.GetAll().Any(p => p.UserId == userId && p.EBookId == ebookId);
    }

    public EBook? GetById(string Id)
    {
        if (string.IsNullOrEmpty(Id)) return null;
        return EBookRepository.GetById(Id);
    }

    public IEnumerable<EBook> GetPage(int pageSize, int pageNumber) => EBookRepository.GetAll().Skip((pageNumber - 1) * pageSize).Take(pageSize);

    public void Update(EBook eBook)
    {
        EBookRepository.Update(eBook);
        EBookRepository.Save();
    }

    public void Buy(string ebookId, string? userId)
    {
        EBook? eBook = GetById(ebookId) ?? throw new Exception("Could not retrive EBook by ID");
        EBookPurchase purchase = new EBookPurchase() { EBookId = eBook.Id, PurchasePrice = eBook.Price, PurchaseTimestamp = DateTime.UtcNow, UserId = userId};
        EBookPurchaseRepository.Insert(purchase);
        EBookPurchaseRepository.Save();
    }
}
