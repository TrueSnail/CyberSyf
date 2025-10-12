using E_Book_Store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Book_Store.Data;

public class EBookDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<EBook> EBooks { get; set; }
    public DbSet<EBookPurchase> EBookPurchases { get; set; }

    public EBookDbContext(DbContextOptions<EBookDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<EBookPurchase>()
            .HasOne<EBook>()
            .WithMany()
            .HasForeignKey(p => p.EBookId);
        builder.Entity<EBookPurchase>()
            .HasOne<IdentityUser>()
            .WithMany()
            .HasForeignKey(p => p.UserId);

        base.OnModelCreating(builder);
    }
}

