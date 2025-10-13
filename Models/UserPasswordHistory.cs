namespace E_Book_Store.Models
{
    public class UserPasswordHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string HashedPassword { get; set; }
        public DateTime ChangedDate { get; set; }
    }
}
