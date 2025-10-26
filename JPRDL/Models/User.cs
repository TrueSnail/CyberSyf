using System.Text.Json.Serialization;

namespace JPRDL.Models
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsLocked { get; set; } = false;
        public DateTime? PasswordExpiryDate { get; set; }
        public List<string> PasswordHistory { get; set; } = new List<string>();
        public bool MustChangePassword { get; set; } = true;
        
        [JsonIgnore]
        public bool IsAdmin => Username == "ADMIN";
    }
}
