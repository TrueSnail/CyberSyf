using E_Book_Store.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections;

namespace E_Book_Store.ViewModels.Admin
{
    public class AdminIndexViewModel
    {
        public List<ApplicationUser> Users { get; set; }
        public int EBooksCount { get; set; }
        public int UsersCount => Users.Count;

        public AdminIndexViewModel(List<ApplicationUser> users, int ebooksCount)
        {
            Users = users;
            EBooksCount = ebooksCount;
        }
        //ustawienia haseł admina

        public bool EnforcePasswordPolicy { get; set; }
        public int DefaultPasswordExpiryDays { get; set; }

        //dodawanie nowego użytkownika
        //public NewUserModel NewUser { get; set; } = new();

    
    }
    //klasa tworząca nowego użytkownika
    //public class NewUserModel
    //{
    //    public string UserName { get; set; }
    //    public string FullName { get; set; }
    //    public string Password { get; set; }
    //    public bool IsAdmin { get; set; }
    //}
}


