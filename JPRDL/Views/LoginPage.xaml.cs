using JPRDL.Services;
using JPRDL.Models;

namespace JPRDL.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly UserService _userService;

        public LoginPage()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            ErrorLabel.IsVisible = false;
            
            string username = UsernameEntry.Text?.Trim() ?? string.Empty;
            string password = PasswordEntry.Text ?? string.Empty;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Wypełnij wszystkie pola");
                return;
            }

            var user = _userService.AuthenticateUser(username, password);

            if (user == null)
            {
                ShowError("Login lub Hasło niepoprawny");
                return;
            }

            if (user.MustChangePassword)
            {
                await Navigation.PushAsync(new ChangePasswordPage(_userService, user, true));
            }
            else if (user.IsAdmin)
            {
                await Navigation.PushAsync(new AdminPage(_userService, user));
            }
            else
            {
                await Navigation.PushAsync(new UserPage(_userService, user));
            }
        }

        private void ShowError(string message)
        {
            ErrorLabel.Text = message;
            ErrorLabel.IsVisible = true;
        }
    }
}
