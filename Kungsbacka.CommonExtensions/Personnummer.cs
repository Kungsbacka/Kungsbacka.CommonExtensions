using System;

namespace Kungsbacka.CommonExtensions
{
    public class Personnummer
    {
        private enum PersonnummerLength
        {
            Long,
            Short
        }

        private class PersonnummerInfo
        {
            public PersonnummerLength Length { get; set; }
            public bool IsValid { get; set; }
            public bool ContainsDash { get; set; }
            public bool IsTemporary { get; set; }
            public DateTime BirthDate { get; set; }
            public string FormatError { get; set; }
        }

        public string LongForm
        {
            get
            {
                if (_long == null)
                {
                    _long = _original;
                    if (_originalLength == PersonnummerLength.Short)
                    {
                        _long = BirthDate.Year + _long.Substring(2);
                    }
                    if (_originalContainsDash)
                    {
                        _long = _long.Substring(0, 8) + _long.Substring(9, 4);
                    }
                }
                return _long;
            }
        }
        public string ShortForm
        {
            get
            {
                if (_short == null)
                {
                    if (_originalLength == PersonnummerLength.Long)
                    {
                        _short = _originalContainsDash ? _original.Substring(2) : _original.Substring(2, 6) + "-" + _original.Substring(8, 4);
                    }
                    else // Short
                    {
                        _short = _originalContainsDash ? _original : _original.Substring(0, 6) + "-" + _original.Substring(6, 4);
                    }
                }
                return _short;
            }
        }
        public string Original => _original;
        public bool IsTemporary => _temporary;
        public DateTime BirthDate => _birthDate;

        private string _original;
        private PersonnummerLength _originalLength;
        private bool _originalContainsDash;
        private string _long;
        private string _short;
        private bool _temporary;
        private DateTime _birthDate;

        public Personnummer(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            PersonnummerInfo info = Validate(input);
            if (!info.IsValid)
            {
                throw new FormatException(info.FormatError);
            }
            SetPrivateFields(input, info);
        }

        private Personnummer(string input, PersonnummerInfo info)
        {
            SetPrivateFields(input, info);
        }

        public static bool TryParse(string input, out Personnummer personnummer)
        {
            personnummer = null;
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            PersonnummerInfo info = Validate(input);
            if (info.IsValid)
            {
                personnummer = new Personnummer(input, info);
            }
            return info.IsValid;
        }

        private void SetPrivateFields(string original, PersonnummerInfo info)
        {
            _original = original;
            _temporary = info.IsTemporary;
            _originalLength = info.Length;
            _originalContainsDash = info.ContainsDash;
            _birthDate = info.BirthDate;
            if (info.Length == PersonnummerLength.Long && !info.ContainsDash)
            {
                _long = original;
            }
            else if (info.Length == PersonnummerLength.Short && info.ContainsDash)
            {
                _short = original;
            }
        }

        private static PersonnummerInfo Validate(string input)
        {
            PersonnummerInfo info = ValidateStructure(input);
            if (!info.IsValid)
            {
                return info;
            }
            if (info.Length == PersonnummerLength.Short)
            {
                int year = int.Parse(input.Substring(0, 2));
                int century = DateTime.Today.Year / 100;
                int currentYear = DateTime.Today.Year % 100;
                if (year >= currentYear)
                {
                    century--;
                }
                input = century + input;

            }

            if (!info.IsTemporary && !TestAgainsCheckDigit(input, info.ContainsDash))
            {
                info.IsValid = false;
                info.FormatError = "Validating check digit failed";
            }

            // Samordningsnummer adds 60 to birth day
            int day = int.Parse(input.Substring(6, 2));
            if (day > 60)
            {
                day -= 60;
            }

            string birthDatestring = string.Format("{0}{1,2:D2}", input.Substring(0, 6), day);
            // Only check that birth date is a valid date. Not if the date is in the future or if it's very old.
            if (DateTime.TryParseExact(birthDatestring, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime birthDate))
            {
                info.BirthDate = birthDate;
            }
            else
            {
                info.IsValid = false;
                info.FormatError = "Birth date is not a valid date";
                return info;
            }
            return info;
        }

        private static PersonnummerInfo ValidateStructure(string input)
        {
            PersonnummerInfo info = new PersonnummerInfo()
            {
                IsValid = false
            };
            if (string.IsNullOrEmpty(input))
            {
                return info;
            }
            if (input.Length < 10 || input.Length > 13)
            {
                return info;
            }
            info.Length = input.Length == 10 || input.Length == 11 ? PersonnummerLength.Short : PersonnummerLength.Long;
            // Pad with leading digits if short to avoid having to duplicate validation logic
            if (info.Length == PersonnummerLength.Short)
            {
                input = "00" + input;
            }
            info.IsValid =
                char.IsDigit(input[0]) &&
                char.IsDigit(input[1]) &&
                char.IsDigit(input[2]) &&
                char.IsDigit(input[3]) &&
                char.IsDigit(input[4]) &&
                char.IsDigit(input[5]);
            if (!info.IsValid)
            {
                return info;
            }
            info.IsValid = char.IsDigit(input[6]) && char.IsDigit(input[7]);
            if (!info.IsValid)
            {
                return info;
            }
            if (input.Length == 12)
            {
                info.IsValid =
                    (char.IsDigit(input[8]) && char.IsDigit(input[9]) && char.IsDigit(input[10]) && char.IsDigit(input[11]))
                 || (input[8] == 'T' && input[9] == 'F' && char.IsDigit(input[10]) && char.IsDigit(input[11]));
                info.IsTemporary = info.IsValid && input[8] == 'T';
                return info;
            }
            info.IsValid =
                (input[8] == '-' && char.IsDigit(input[9]) && char.IsDigit(input[10]) && char.IsDigit(input[11]) && char.IsDigit(input[12]))
             || (input[8] == '-' && input[9] == 'T' && input[10] == 'F' && char.IsDigit(input[11]) && char.IsDigit(input[12]));
            info.IsTemporary = info.IsValid && input[9] == 'T';
            info.ContainsDash = info.IsValid;
            return info;
        }


        private static bool TestAgainsCheckDigit(string input, bool containsDash)
        {
            int len = input.Length;
            if (containsDash)
            {
                return 0 == (
                    (input[len - 1] - 48) +
                    2 * (input[len - 2] - 48) / 10 +
                    2 * (input[len - 2] - 48) % 10 +
                    (input[len - 3] - 48) +
                    2 * (input[len - 4] - 48) / 10 +
                    2 * (input[len - 4] - 48) % 10 +
                    (input[len - 6] - 48) +
                    2 * (input[len - 7] - 48) / 10 +
                    2 * (input[len - 7] - 48) % 10 +
                    (input[len - 8] - 48) +
                    2 * (input[len - 9] - 48) / 10 +
                    2 * (input[len - 9] - 48) % 10 +
                    (input[len - 10] - 48) +
                    2 * (input[len - 11] - 48) / 10 +
                    2 * (input[len - 11] - 48) % 10
                ) % 10;
            }
            return 0 == (
                (input[len - 1] - 48) +
                2 * (input[len - 2] - 48) / 10 +
                2 * (input[len - 2] - 48) % 10 +
                (input[len - 3] - 48) +
                2 * (input[len - 4] - 48) / 10 +
                2 * (input[len - 4] - 48) % 10 +
                (input[len - 5] - 48) +
                2 * (input[len - 6] - 48) / 10 +
                2 * (input[len - 6] - 48) % 10 +
                (input[len - 7] - 48) +
                2 * (input[len - 8] - 48) / 10 +
                2 * (input[len - 8] - 48) % 10 +
                (input[len - 9] - 48) +
                2 * (input[len - 10] - 48) / 10 +
                2 * (input[len - 10] - 48) % 10
            ) % 10;
        }
    }
}
