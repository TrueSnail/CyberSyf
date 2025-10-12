using Microsoft.AspNetCore.Identity;
using System.Collections;

namespace E_Book_Store.ViewModels.Admin;

public class AdminIndexViewModel
{
    public List<IdentityUser> Users { get; set; }
    public int EBooksCount { get; set; }
    public int UsersCount => Users.Count;

    public AdminIndexViewModel(List<IdentityUser> users, int ebooksCount)
    {
        Users = users;
        EBooksCount = ebooksCount;
    }
}
