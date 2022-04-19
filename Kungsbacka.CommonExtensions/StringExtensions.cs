using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Kungsbacka.CommonExtensions
{
    public static class StringExtensions
    {
        public static int IndexOfUnescaped(this string str, char value)
        {
            if (string.IsNullOrEmpty(str))
            {
                return -1;
            }
            int p = 0;
            while (p < str.Length)
            {
                if (str[p] == value)
                {
                    int b = p - 1;
                    while (b > 0 && str[b] == '\\')
                    {
                        b--;
                    }
                    if (b > 0 || (b == 0 && str[0] != '\\'))
                    {
                        b++;
                    }
                    if (b < 0 || (p - b) % 2 == 0)
                    {
                        return p;
                    }
                }
                p++;
            }
            return -1;
        }

        public static int LastIndexOfUnescaped(this string str, char value)
        {
            if (string.IsNullOrEmpty(str))
            {
                return -1;
            }
            int p = str.Length - 1;
            while (p > -1)
            {
                if (str[p] == value)
                {
                    int b = p - 1;
                    while (b > 0 && str[b] == '\\')
                    {
                        b--;
                    }
                    if (b > 0 || (b == 0 && str[0] != '\\'))
                    {
                        b++;
                    }
                    if (b < 0 || (p - b) % 2 == 0)
                    {
                        return p;
                    }
                }
                p--;
            }
            return -1;
        }

        public static string RemoveDiacritic(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            string normalizedString = str.Normalize(NormalizationForm.FormD);
            int len = normalizedString.Length;
            StringBuilder sb = new StringBuilder(len);
            for (int i = 0; i < len; i++)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(normalizedString[i]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        public static string RemoveRepeating(this string str, char c)
        {
            return str?.RemoveRepeating(new char[] { c });
        }

        public static string RemoveRepeating(this string str, char[] chars)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            char previous = str[0];
            sb.Append(previous);
            for (int i = 1; i < str.Length; i++)
            {
                char current = str[i];
                if (current != previous)
                {
                    previous = current;
                    sb.Append(current);
                }
                else if (!chars.Contains(current))
                {
                    sb.Append(current);
                }
            }
            return sb.ToString();
        }

        public static int GetNumberSuffix(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return -1;
            }
            int i = str.Length;
            while (i-- >= 0 && char.IsDigit(str[i])) { }
            string suffix = str.Substring((i + 1), str.Length - (i + 1));
            if (suffix == "")
            {
                return -1;
            }
            return int.Parse(suffix);
        }

        public static string GetProperFirstName(this string firstName, string indicator)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                return "";
            }
            if (string.IsNullOrEmpty(indicator))
            {
                indicator = "10";
            }
            string[] nameArray = firstName.Replace("-", " -").Split(' ');
            string properName = string.Empty;
            foreach (char c in indicator)
            {
                int i = c - '0' - 1;
                if (i >= 0 && i < nameArray.Length)
                {
                    properName += nameArray[i] + " ";
                }
            }
            return properName.Replace(" -", "-").Trim(new char[] { ' ', '-' });
        }

        public static string FormatAsEmailAddress(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = str.RemoveDiacritic().ToLower().Replace(' ', '.');
            str = Regex.Replace(str, @"[^\w.@-]", "");
            str = str.Trim(".@-".ToCharArray());
            return str.RemoveRepeating(new char[] { '.', '@', '-' });
        }

        public static string FormatAsUsername(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = str.RemoveDiacritic().ToLower();
            Regex.Replace(str, @"[^\w.-]", "");
            str = str.Trim(".-".ToCharArray());
            return str.RemoveRepeating(new char[] { '.', '-' });
        }

        public static string FormatAsPersonnummer(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = str.ToUpper();
            if (Regex.IsMatch(str, "^[0-9]{8}((TF[0-9]{2})|([0-9]{4}))$"))
            {
                return str;
            }
            if (Regex.IsMatch(str, "^[0-9]{8}-((TF[0-9]{2})|([0-9]{4}))$"))
            {
                return str.Replace("-", "");
            }
            if (Regex.IsMatch(str, "^[0-9]{6}-((TF[0-9]{2})|([0-9]{4}))$"))
            {
                str = str.Replace("-", "");
            }
            if (Regex.IsMatch(str, "^[0-9]{6}((TF[0-9]{2})|([0-9]{4}))$"))
            {
                int year = int.Parse(str.Substring(0, 2));
                int century = DateTime.Today.Year / 100;
                int currentYear = DateTime.Today.Year % 100;
                if (year >= currentYear)
                {
                    century--;
                }
                return century.ToString() + str;
            }
            return "";
        }

        public static string FormatAsShortPersonnummer(this string str)
        {
            string pnr = str.FormatAsPersonnummer();
            if (string.IsNullOrEmpty(pnr))
            {
                return "";
            }
            return pnr.Substring(2, 6) + "-" + pnr.Substring(8, 4);
        }

        public static bool IEquals(this string left, string right)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }
            return left.Equals(right, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IStartsWith(this string left, string right)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }
            return left.StartsWith(right, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IEndsWith(this string left, string right)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }
            return left.EndsWith(right, StringComparison.OrdinalIgnoreCase);
        }

        public static int IIndexOf(this string str, string sub)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }
            return str.IndexOf(sub, StringComparison.OrdinalIgnoreCase);
        }
    }
}
