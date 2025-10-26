namespace JPRDL.Models
{
    public class UserDatabase
    {
        public List<User> Users { get; set; } = new List<User>();
        public PasswordRestrictions Restrictions { get; set; } = new PasswordRestrictions();
    }
}
