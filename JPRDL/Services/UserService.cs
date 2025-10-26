using JPRDL.Models;
using System.Text.Json;

namespace JPRDL.Services
{
    public class UserService
    {
        private readonly string _dataPath;
        private UserDatabase _database;

        public UserService()
        {
            _dataPath = Path.Combine(FileSystem.AppDataDirectory, "users.json");
            LoadDatabase();
        }

        private void LoadDatabase()
        {
            try
            {
                if (File.Exists(_dataPath))
                {
                    var json = File.ReadAllText(_dataPath);
                    _database = JsonSerializer.Deserialize<UserDatabase>(json) ?? new UserDatabase();
                }
                else
                {
                    InitializeDefaultDatabase();
                }
            }
            catch
            {
                InitializeDefaultDatabase();
            }
        }

        private void InitializeDefaultDatabase()
        {
            _database = new UserDatabase();
            var adminUser = new User
            {
                Username = "ADMIN",
                FullName = "Administrator",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123", workFactor: 12),
                IsLocked = false,
                MustChangePassword = false
            };
            _database.Users.Add(adminUser);
            SaveDatabase();
        }

        private void SaveDatabase()
        {
            var options = new JsonSerializerOptions 
            { 
                WriteIndented = true 
            };
            var json = JsonSerializer.Serialize(_database, options);
            File.WriteAllText(_dataPath, json);
        }

        public User? AuthenticateUser(string username, string password)
        {
            var user = _database.Users.FirstOrDefault(u => u.Username == username);
            
            if (user == null || user.IsLocked)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            if (user.PasswordExpiryDate.HasValue && user.PasswordExpiryDate.Value < DateTime.Now)
            {
                user.MustChangePassword = true;
                SaveDatabase();
            }

            return user;
        }

        public bool ValidatePassword(string password)
        {
            var restrictions = _database.Restrictions;
            
            if (password.Length < restrictions.MinimumLength)
                return false;

            if (restrictions.RequireUppercase)
            {
                int uppercaseCount = password.Count(char.IsUpper);
                if (uppercaseCount < restrictions.MinimumUppercase)
                    return false;
            }

            if (restrictions.RequireDigits)
            {
                int digitCount = password.Count(char.IsDigit);
                if (digitCount < restrictions.MinimumDigits)
                    return false;
            }

            return true;
        }

        public string GetPasswordRequirementsText()
        {
            var restrictions = _database.Restrictions;
            var requirements = new List<string>();

            requirements.Add($"Minimalna długość: {restrictions.MinimumLength} znaków");

            if (restrictions.RequireUppercase)
                requirements.Add($"Co najmniej {restrictions.MinimumUppercase} wielka litera");

            if (restrictions.RequireDigits)
                requirements.Add($"Co najmniej {restrictions.MinimumDigits} cyfry");

            return string.Join("\n", requirements);
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var user = _database.Users.FirstOrDefault(u => u.Username == username);
            
            if (user == null)
                return false;

            if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
                return false;

            if (!ValidatePassword(newPassword))
                return false;

            if (user.PasswordHistory.Any(h => BCrypt.Net.BCrypt.Verify(newPassword, h)))
                return false;

            user.PasswordHistory.Add(user.PasswordHash);
            if (user.PasswordHistory.Count > 5)
                user.PasswordHistory.RemoveAt(0);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword, workFactor: 12);
            user.MustChangePassword = false;
            SaveDatabase();
            return true;
        }

        public bool AddUser(string username, string fullName, string password)
        {
            if (_database.Users.Any(u => u.Username == username))
                return false;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(fullName))
                return false;

            var user = new User
            {
                Username = username,
                FullName = fullName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12),
                IsLocked = false,
                MustChangePassword = true
            };

            _database.Users.Add(user);
            SaveDatabase();
            return true;
        }

        public List<User> GetAllUsers()
        {
            return _database.Users.ToList();
        }

        public bool DeleteUser(string username)
        {
            if (username == "ADMIN")
                return false;

            var user = _database.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                _database.Users.Remove(user);
                SaveDatabase();
                return true;
            }
            return false;
        }

        public bool LockUser(string username, bool locked)
        {
            if (username == "ADMIN")
                return false;

            var user = _database.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                user.IsLocked = locked;
                SaveDatabase();
                return true;
            }
            return false;
        }

        public bool UpdatePasswordRestrictions(PasswordRestrictions restrictions)
        {
            _database.Restrictions = restrictions;
            SaveDatabase();
            return true;
        }

        public PasswordRestrictions GetPasswordRestrictions()
        {
            return _database.Restrictions;
        }

        public bool SetPasswordExpiry(string username, int days)
        {
            var user = _database.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                user.PasswordExpiryDate = DateTime.Now.AddDays(days);
                SaveDatabase();
                return true;
            }
            return false;
        }

        public bool UpdateUserDetails(string username, string fullName, string? newPassword = null)
        {
            var user = _database.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                user.FullName = fullName;
                if (!string.IsNullOrEmpty(newPassword))
                {
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword, workFactor: 12);
                    user.MustChangePassword = true;
                }
                SaveDatabase();
                return true;
            }
            return false;
        }
    }
}
