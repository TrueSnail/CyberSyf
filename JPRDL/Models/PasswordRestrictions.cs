namespace JPRDL.Models
{
    public class PasswordRestrictions
    {
        public bool RequireUppercase { get; set; } = true;
        public bool RequireDigits { get; set; } = true;
        public int MinimumUppercase { get; set; } = 1;
        public int MinimumDigits { get; set; } = 2;
        public int MinimumLength { get; set; } = 8;
    }
}
