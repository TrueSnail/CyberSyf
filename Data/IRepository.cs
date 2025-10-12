namespace E_Book_Store.Data;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll();
    TEntity? GetById(string ID);
    void Insert(TEntity obj);
    void Update(TEntity obj);
    void Delete(string ID);
    void Save();
}
