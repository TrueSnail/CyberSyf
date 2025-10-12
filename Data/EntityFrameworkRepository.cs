using Microsoft.EntityFrameworkCore;

namespace E_Book_Store.Data;

public class EntityFrameworkRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    internal EBookDbContext context;
    internal DbSet<TEntity> dbSet;

    public EntityFrameworkRepository(EBookDbContext context)
    {
        this.context = context;
        dbSet = context.Set<TEntity>();
    }

    public void Delete(string ID)
    {
        TEntity? entity = dbSet.Find(ID);
        if (entity != null)
        {
            dbSet.Remove(entity);
        }
    }

    public IQueryable<TEntity> GetAll()
    {
        return dbSet;
    }

    public TEntity? GetById(string ID)
    {
        return dbSet.Find(ID);
    }

    public void Insert(TEntity obj)
    {
        dbSet.Add(obj);
    }

    public void Save()
    {
        context.SaveChanges();
    }

    public void Update(TEntity obj)
    {
        dbSet.Entry(obj).State = EntityState.Modified;
    }
}