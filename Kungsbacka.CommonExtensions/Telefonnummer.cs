using System.Text.RegularExpressions;

namespace Kungsbacka.CommonExtensions
{
    public static class Telefonnummer
    {
        private static Regex phoneNumberRegex = new Regex("^\\+46[1-9][0-9]{6,9}$", RegexOptions.Compiled | RegexOptions.Singleline);

        private static Regex mobilePhoneRegex = new Regex("^\\+46(?:70|72|73|76|79)[0-9]{7}$", RegexOptions.Compiled | RegexOptions.Singleline);

        public static bool IsValidTelephoneNumber(this string phoneNumber)
        {
            return phoneNumber != null && phoneNumberRegex.IsMatch(phoneNumber);
        }

        public static bool IsMobilePhoneNumber(this string phoneNumber)
        {
            return phoneNumber != null && mobilePhoneRegex.IsMatch(phoneNumber);
        }
    }
}
