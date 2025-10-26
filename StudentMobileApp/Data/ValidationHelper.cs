using System.Text.RegularExpressions;

namespace StudentMobileApp.Data
{
    public static class ValidationHelper
    {
        public static bool IsNotEmpty(string input) =>
            !string.IsNullOrWhiteSpace(input);

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            string pattern = @"^[0-9\-\+\s\(\)]{7,20}$";
            return Regex.IsMatch(phone, pattern);
        }

        public static bool IsDateRangeValid(DateTime start, DateTime end) =>
            end >= start;
    }
}

