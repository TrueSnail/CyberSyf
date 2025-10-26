using JPRDL.Services;
using JPRDL.Models;

namespace SecuritySystem.Views
{
    public partial class AdminPage : ContentPage
    {
        private readonly UserService _userService;
        private readonly User _currentUser;

        public AdminPage(UserService userService, User currentUser)
        {
            InitializeComponent();
            _userService = userService;
            _currentUser = currentUser;
        }

        private async void OnChangePasswordClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangePasswordPage(_userService, _currentUser, false));
        }

        private async void OnAddUserClicked(object sender, EventArgs e)
        {
            string username = await DisplayPromptAsync("Dodaj użytkownika", 
                "Podaj nazwę użytkownika:", 
                placeholder: "np. jkowalski");

            if (string.IsNullOrWhiteSpace(username))
                return;

            string fullName = await DisplayPromptAsync("Dodaj użytkownika", 
                "Podaj pełną nazwę:", 
                placeholder: "np. Jan Kowalski");

            if (string.IsNullOrWhiteSpace(fullName))
                return;

            string password = await DisplayPromptAsync("Dodaj użytkownika", 
                "Podaj hasło początkowe:", 
                placeholder: "Hasło tymczasowe");

            if (string.IsNullOrWhiteSpace(password))
                return;

            if (!_userService.ValidatePassword(password))
            {
                await DisplayAlert("Błąd", 
                    $"Hasło nie spełnia wymagań:\n{_userService.GetPasswordRequirementsText()}", 
                    "OK");
                return;
            }

            bool success = _userService.AddUser(username, fullName, password);

            if (success)
            {
                await DisplayAlert("Sukces", 
                    $"Użytkownik {username} został dodany.\nPrzy pierwszym logowaniu musi zmienić hasło.", 
                    "OK");
            }
            else
            {
                await DisplayAlert("Błąd", 
                    "Nie udało się dodać użytkownika.\nUżytkownik o takiej nazwie już istnieje.", 
                    "OK");
            }
        }

        private void OnManageUsersClicked(object sender, EventArgs e)
        {
            UsersListFrame.IsVisible = !UsersListFrame.IsVisible;
            
            if (UsersListFrame.IsVisible)
            {
                LoadUsers();
            }
        }

        private void LoadUsers()
        {
            UsersContainer.Clear();
            var users = _userService.GetAllUsers();

            foreach (var user in users)
            {
                var userFrame = new Frame
                {
                    BorderColor = Colors.LightGray,
                    CornerRadius = 8,
                    Padding = 12,
                    Margin = new Thickness(0, 5),
                    BackgroundColor = user.IsLocked ? Color.FromArgb("#FFEBEE") : Colors.White
                };

                var layout = new VerticalStackLayout { Spacing = 8 };

                var nameLabel = new Label
                {
                    Text = $"👤 {user.FullName} ({user.Username})",
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold
                };

                var statusLabel = new Label
                {
                    Text = user.IsLocked ? "🔒 ZABLOKOWANY" : "✓ Aktywny",
                    TextColor = user.IsLocked ? Colors.Red : Colors.Green,
                    FontSize = 14
                };

                layout.Children.Add(nameLabel);
                layout.Children.Add(statusLabel);

                if (user.PasswordExpiryDate.HasValue)
                {
                    var expiryLabel = new Label
                    {
                        Text = $"⏰ Hasło wygasa: {user.PasswordExpiryDate.Value:dd.MM.yyyy}",
                        FontSize = 12,
                        TextColor = Colors.Gray
                    };
                    layout.Children.Add(expiryLabel);
                }

                if (user.Username != "ADMIN")
                {
                    var buttonsLayout = new Grid
                    {
                        ColumnDefinitions = new ColumnDefinitionCollection
                        {
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                        },
                        RowDefinitions = new RowDefinitionCollection
                        {
                            new RowDefinition { Height = GridLength.Auto },
                            new RowDefinition { Height = GridLength.Auto }
                        },
                        ColumnSpacing = 5,
                        RowSpacing = 5,
                        Margin = new Thickness(0, 10, 0, 0)
                    };

                    var lockButton = new Button
                    {
                        Text = user.IsLocked ? "Odblokuj" : "Zablokuj",
                        BackgroundColor = user.IsLocked ? Colors.Green : Colors.Orange,
                        TextColor = Colors.White,
                        FontSize = 12,
                        Padding = 5
                    };
                    lockButton.Clicked += (s, e) => OnLockUserClicked(user);
                    Grid.SetRow(lockButton, 0);
                    Grid.SetColumn(lockButton, 0);

                    var expiryButton = new Button
                    {
                        Text = "Wygaśnięcie",
                        BackgroundColor = Colors.Blue,
                        TextColor = Colors.White,
                        FontSize = 12,
                        Padding = 5
                    };
                    expiryButton.Clicked += (s, e) => OnSetExpiryClicked(user);
                    Grid.SetRow(expiryButton, 0);
                    Grid.SetColumn(expiryButton, 1);

                    var editButton = new Button
                    {
                        Text = "Edytuj",
                        BackgroundColor = Colors.Gray,
                        TextColor = Colors.White,
                        FontSize = 12,
                        Padding = 5
                    };
                    editButton.Clicked += (s, e) => OnEditUserClicked(user);
                    Grid.SetRow(editButton, 1);
                    Grid.SetColumn(editButton, 0);

                    var deleteButton = new Button
                    {
                        Text = "Usuń",
                        BackgroundColor = Colors.Red,
                        TextColor = Colors.White,
                        FontSize = 12,
                        Padding = 5
                    };
                    deleteButton.Clicked += (s, e) => OnDeleteUserClicked(user);
                    Grid.SetRow(deleteButton, 1);
                    Grid.SetColumn(deleteButton, 1);

                    buttonsLayout.Children.Add(lockButton);
                    buttonsLayout.Children.Add(expiryButton);
                    buttonsLayout.Children.Add(editButton);
                    buttonsLayout.Children.Add(deleteButton);

                    layout.Children.Add(buttonsLayout);
                }

                userFrame.Content = layout;
                UsersContainer.Children.Add(userFrame);
            }
        }

        private async void OnLockUserClicked(User user)
        {
            bool newLockState = !user.IsLocked;
            string action = newLockState ? "zablokować" : "odblokować";
            
            bool confirm = await DisplayAlert("Potwierdzenie", 
                $"Czy na pewno chcesz {action} użytkownika {user.Username}?", 
                "Tak", "Nie");

            if (confirm)
            {
                _userService.LockUser(user.Username, newLockState);
                await DisplayAlert("Sukces", 
                    $"Użytkownik {user.Username} został {(newLockState ? "zablokowany" : "odblokowany")}", 
                    "OK");
                LoadUsers();
            }
        }

        private async void OnSetExpiryClicked(User user)
        {
            string result = await DisplayPromptAsync("Ustaw wygaśnięcie hasła", 
                $"Po ilu dniach hasło użytkownika {user.Username} ma wygasnąć?", 
                keyboard: Keyboard.Numeric, 
                placeholder: "np. 30");

            if (!string.IsNullOrEmpty(result) && int.TryParse(result, out int days) && days > 0)
            {
                _userService.SetPasswordExpiry(user.Username, days);
                await DisplayAlert("Sukces", 
                    $"Hasło użytkownika {user.Username} wygaśnie za {days} dni", 
                    "OK");
                LoadUsers();
            }
            else if (!string.IsNullOrEmpty(result))
            {
                await DisplayAlert("Błąd", "Podaj prawidłową liczbę dni (większą od 0)", "OK");
            }
        }

        private async void OnEditUserClicked(User user)
        {
            string newFullName = await DisplayPromptAsync("Edytuj użytkownika", 
                "Podaj nową pełną nazwę:", 
                initialValue: user.FullName);

            if (!string.IsNullOrWhiteSpace(newFullName) && newFullName != user.FullName)
            {
                bool changePassword = await DisplayAlert("Edytuj użytkownika", 
                    "Czy chcesz również zmienić hasło użytkownika?", 
                    "Tak", "Nie");

                string newPassword = null;
                if (changePassword)
                {
                    newPassword = await DisplayPromptAsync("Nowe hasło", 
                        "Podaj nowe hasło dla użytkownika:");

                    if (!string.IsNullOrWhiteSpace(newPassword) && 
                        !_userService.ValidatePassword(newPassword))
                    {
                        await DisplayAlert("Błąd", 
                            $"Hasło nie spełnia wymagań:\n{_userService.GetPasswordRequirementsText()}", 
                            "OK");
                        return;
                    }
                }

                _userService.UpdateUserDetails(user.Username, newFullName, newPassword);
                await DisplayAlert("Sukces", 
                    "Dane użytkownika zostały zaktualizowane" + 
                    (changePassword ? "\nUżytkownik musi zmienić hasło przy następnym logowaniu" : ""), 
                    "OK");
                LoadUsers();
            }
        }

        private async void OnDeleteUserClicked(User user)
        {
            bool confirm = await DisplayAlert("Potwierdzenie", 
                $"Czy na pewno chcesz usunąć użytkownika {user.Username}?\nTej operacji nie można cofnąć!", 
                "Tak, usuń", "Nie");

            if (confirm)
            {
                bool success = _userService.DeleteUser(user.Username);
                if (success)
                {
                    await DisplayAlert("Sukces", 
                        $"Użytkownik {user.Username} został usunięty", 
                        "OK");
                    LoadUsers();
                }
            }
        }

        private async void OnPasswordRestrictionsClicked(object sender, EventArgs e)
        {
            var restrictions = _userService.GetPasswordRestrictions();

            string minLength = await DisplayPromptAsync("Ograniczenia haseł", 
                "Minimalna długość hasła:", 
                keyboard: Keyboard.Numeric, 
                initialValue: restrictions.MinimumLength.ToString());

            if (string.IsNullOrEmpty(minLength) || !int.TryParse(minLength, out int minLengthValue))
                return;

            bool requireUpper = await DisplayAlert("Ograniczenia haseł", 
                "Czy wymagać wielkich liter?", 
                "Tak", "Nie");

            int minUppercase = 0;
            if (requireUpper)
            {
                string upperCount = await DisplayPromptAsync("Ograniczenia haseł", 
                    "Minimalna liczba wielkich liter:", 
                    keyboard: Keyboard.Numeric, 
                    initialValue: restrictions.MinimumUppercase.ToString());

                if (string.IsNullOrEmpty(upperCount) || !int.TryParse(upperCount, out minUppercase))
                    minUppercase = 1;
            }

            bool requireDigits = await DisplayAlert("Ograniczenia haseł", 
                "Czy wymagać cyfr?", 
                "Tak", "Nie");

            int minDigits = 0;
            if (requireDigits)
            {
                string digitsCount = await DisplayPromptAsync("Ograniczenia haseł", 
                    "Minimalna liczba cyfr:", 
                    keyboard: Keyboard.Numeric, 
                    initialValue: restrictions.MinimumDigits.ToString());

                if (string.IsNullOrEmpty(digitsCount) || !int.TryParse(digitsCount, out minDigits))
                    minDigits = 2;
            }

            var newRestrictions = new PasswordRestrictions
            {
                MinimumLength = minLengthValue,
                RequireUppercase = requireUpper,
                MinimumUppercase = minUppercase,
                RequireDigits = requireDigits,
                MinimumDigits = minDigits
            };

            _userService.UpdatePasswordRestrictions(newRestrictions);
            await DisplayAlert("Sukces", 
                $"Ograniczenia haseł zostały zaktualizowane:\n{_userService.GetPasswordRequirementsText()}", 
                "OK");
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Wylogowanie", 
                "Czy na pewno chcesz się wylogować?", 
                "Tak", "Nie");

            if (confirm)
            {
                await Navigation.PopToRootAsync();
            }
        }
    }
}
