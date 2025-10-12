using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Book_Store.Models;

public class EBookPurchase
{
    [Key]
    public string Id { get; set; }
    [ForeignKey(nameof(EBook))]
    public required string EBookId { get; set; }
    [ForeignKey(nameof(IdentityUser))]
    public required string UserId { get; set; }
    public required decimal PurchasePrice { get; set; }
    public required DateTimeOffset PurchaseTimestamp { get; set; }

    public EBookPurchase()
    {
        Id = Guid.NewGuid().ToString();
    }
}
