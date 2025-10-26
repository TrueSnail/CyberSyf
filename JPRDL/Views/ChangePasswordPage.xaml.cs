using JPRDL.Services;
using JPRDL.Models;

namespace JPRDL.Views
{
    public partial class ChangePasswordPage : ContentPage
    {
        private readonly UserService _userService;
        private readonly User _currentUser;
        private readonly bool _isRequired;

        public ChangePasswordPage(UserService userService, User currentUser, bool isRequired)
        {
            InitializeComponent();
            _userService = userService;
            _currentUser = currentUser;
            _isRequired = isRequired;

            RequirementsLabel.Text = _userService.GetPasswordRequirementsText();

            if (_isRequired)
            {
                InfoLabel.Text = "Musisz zmienić hasło przed kontynuowaniem";
                CancelButton.IsVisible = false;
            }
            else
            {
                InfoLabel.IsVisible = false;
            }
        }

        private async void OnChangePasswordClicked(object sender, EventArgs e)
        {
            ErrorLabel.IsVisible = false;

            string oldPassword = OldPasswordEntry.Text ?? string.Empty;
            string newPassword = NewPasswordEntry.Text ?? string.Empty;
            string confirmPassword = ConfirmPasswordEntry.Text ?? string.Empty;

            if (string.IsNullOrEmpty(oldPassword) || 
                string.IsNullOrEmpty(newPassword) || 
                string.IsNullOrEmpty(confirmPassword))
            {
                ShowError("Wypełnij wszystkie pola");
                return;
            }

            if (newPassword != confirmPassword)
            {
                ShowError("Nowe hasła nie są identyczne");
                return;
            }

            if (!_userService.ValidatePassword(newPassword))
            {
                ShowError($"Hasło nie spełnia wymagań:\n{_userService.GetPasswordRequirementsText()}");
                return;
            }

            bool success = _userService.ChangePassword(_currentUser.Username, oldPassword, newPassword);

            if (!success)
            {
                ShowError("Nie udało się zmienić hasła.\nSprawdź stare hasło lub upewnij się,\nże nowe hasło nie było używane wcześniej.");
                return;
            }

            await DisplayAlert("Sukces", "Hasło zostało pomyślnie zmienione", "OK");

            if (_isRequired)
            {
                if (_currentUser.IsAdmin)
                {
                    await Navigation.PushAsync(new AdminPage(_userService, _currentUser));
                }
                else
                {
                    await Navigation.PushAsync(new UserPage(_userService, _currentUser));
                }
            }
            else
            {
                await Navigation.PopAsync();
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void ShowError(string message)
        {
            ErrorLabel.Text = message;
            ErrorLabel.IsVisible = true;
        }
    }
}
