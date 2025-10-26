using JPRDL.Services;
using JPRDL.Models;

namespace JPRDL.Views
{
    public partial class ChangePasswordPage : ContentPage
    {
        private readonly UserService _userService;
        private readonly User _currentUser;
        private readonly bool _isFirstLogin;

        public ChangePasswordPage(UserService userService, User currentUser, bool isFirstLogin)
        {
            InitializeComponent();
            _userService = userService;
            _currentUser = currentUser;
            _isFirstLogin = isFirstLogin;

            RequirementsLabel.Text = "Wymagania:\n" + _userService.GetPasswordRequirementsText();

            if (_isFirstLogin)
            {
                Title = "Zmiana hasła - Pierwsze logowanie";
            }
        }

        private async void OnChangePasswordClicked(object sender, EventArgs e)
        {
            ErrorLabel.IsVisible = false;

            string oldPassword = OldPasswordEntry.Text?.Trim() ?? "";
            string newPassword = NewPasswordEntry.Text?.Trim() ?? "";
            string confirmPassword = ConfirmPasswordEntry.Text?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(oldPassword) || 
                string.IsNullOrWhiteSpace(newPassword) || 
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                ErrorLabel.Text = "Wszystkie pola są wymagane";
                ErrorLabel.IsVisible = true;
                return;
            }

            if (newPassword != confirmPassword)
            {
                ErrorLabel.Text = "Nowe hasła nie są identyczne";
                ErrorLabel.IsVisible = true;
                return;
            }

            if (!_userService.ValidatePassword(newPassword))
            {
                ErrorLabel.Text = $"Hasło nie spełnia wymagań:\n{_userService.GetPasswordRequirementsText()}";
                ErrorLabel.IsVisible = true;
                return;
            }

            bool success = _userService.ChangePassword(_currentUser.Username, oldPassword, newPassword);

            if (success)
            {
                await DisplayAlert("Sukces", "Hasło zostało zmienione pomyślnie", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                ErrorLabel.Text = "Nie udało się zmienić hasła. Sprawdź stare hasło lub upewnij się, że nowe hasło nie było używane wcześniej.";
                ErrorLabel.IsVisible = true;
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            if (_isFirstLogin)
            {
                bool confirm = await DisplayAlert(
                    "Wymagana zmiana hasła",
                    "Musisz zmienić hasło przy pierwszym logowaniu. Czy na pewno chcesz wrócić do ekranu logowania?",
                    "Tak", "Nie");

                if (confirm)
                {
                    await Navigation.PopToRootAsync();
                }
            }
            else
            {
                await Navigation.PopAsync();
            }
        }
    }
}
