namespace E_Book_Store.ViewModels.Admin
{
    public class PasswordSettingsViewModel
    {
        public string UserId { get; set; }
        public bool EnforcePasswordPolicy { get; set; }
        public int PasswordExpiryDays { get; set; }
    }

}
