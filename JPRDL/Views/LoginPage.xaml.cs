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

            string username = UsernameEntry.Text?.Trim() ?? "";
            string password = PasswordEntry.Text ?? "";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ErrorLabel.Text = "Podaj nazwę użytkownika i hasło";
                ErrorLabel.IsVisible = true;
                return;
            }

            var user = _userService.AuthenticateUser(username, password);

            if (user == null)
            {
                ErrorLabel.Text = "Login lub Hasło niepoprawny";
                ErrorLabel.IsVisible = true;
                return;
            }

            // Sprawdź czy użytkownik musi zmienić hasło
            if (user.MustChangePassword)
            {
                await DisplayAlert("Zmiana hasła wymagana", 
                    "Musisz zmienić swoje hasło przed pierwszym użyciem systemu.", 
                    "OK");
                await Navigation.PushAsync(new ChangePasswordPage(_userService, user, true));
                return;
            }

            // Zalogowano pomyślnie - przejdź do odpowiedniego panelu
            if (user.IsAdmin)
            {
                await Navigation.PushAsync(new AdminPage(_userService, user));
            }
            else
            {
                await Navigation.PushAsync(new UserPage(_userService, user));
            }
        }
    }
}
