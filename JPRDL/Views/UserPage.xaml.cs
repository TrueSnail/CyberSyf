using JPRDL.Services;
using JPRDL.Models;

namespace SecuritySystem.Views
{
    public partial class UserPage : ContentPage
    {
        private readonly UserService _userService;
        private readonly User _currentUser;

        public UserPage(UserService userService, User currentUser)
        {
            InitializeComponent();
            _userService = userService;
            _currentUser = currentUser;
            WelcomeLabel.Text = $"Witaj, {currentUser.FullName}!";
        }

        private async void OnChangePasswordClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangePasswordPage(_userService, _currentUser, false));
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
