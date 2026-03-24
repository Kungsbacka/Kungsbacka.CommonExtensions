using Xunit;

namespace Kungsbacka.CommonExtensions.Tests
{
    public class TestStringExtensions
    {
        [Theory]
        [InlineData(null,        -1)]
        [InlineData("",          -1)]
        [InlineData(".",          0)]
        [InlineData("A.",         1)]
        [InlineData(@"\.",       -1)]
        [InlineData(@"\\.",       2)]
        [InlineData(@"A\.",      -1)]
        [InlineData(@"A\\.",      3)]
        [InlineData(@"\\\\.",     4)]
        [InlineData(@"A\\\\.",    5)]
        [InlineData(@"\\\.",     -1)]
        [InlineData(@"A\\\.",    -1)]
        [InlineData(@"AA\.",     -1)]
        [InlineData(@"\",        -1)]
        [InlineData(@"A\.A\\.",   6)]
        public void TestIndexOfUnescaped(string input, int expected)
        {
            Assert.Equal(expected, input.IndexOfUnescaped('.'));
        }

        [Theory]
        [InlineData(null,          -1)]
        [InlineData(@"ABC.123",     3)]
        [InlineData(@"ABC\.123",   -1)]
        [InlineData(@"ABC.123.",    7)]
        [InlineData(@"ABC\.123.",   8)]
        [InlineData(@"\.",         -1)]
        [InlineData(@".\.\\\.",     0)]
        [InlineData("",            -1)]
        [InlineData(".",            0)]
        [InlineData(@".\.",         0)]
        [InlineData(@"\",          -1)]
        [InlineData(@".\",          0)]
        [InlineData(@"\\..",        3)]
        [InlineData(@"\\...",       4)]
        public void TestLastIndexOfUnescaped(string input, int expected)
        {
            Assert.Equal(expected, input.LastIndexOfUnescaped('.'));
        }

        [Theory]
        [InlineData(null,    "")]
        [InlineData("",      "")]
        [InlineData("åäöÅÄÖ", "aaoAAO")]
        [InlineData("ñéí´",  "nei´")]
        [InlineData("ßÆØ",   "ßÆØ")]
        public void TestRemoveDiacritics(string input, string expected)
        {
            Assert.Equal(expected, input.RemoveDiacritic());
        }

        [Theory]
        [InlineData(null,         "",    "")]
        [InlineData("aabbcc",     "ac",  "abbc")]
        [InlineData("--.-.--..-", ".-",  "-.-.-.-")]
        [InlineData("   ",        " ",   " ")]
        public void TestRemoveRepeating(string input, string chars, string expected)
        {
            Assert.Equal(expected, input.RemoveRepeating(chars.ToCharArray()));
        }

        [Theory]
        [InlineData(null,            -1)]
        [InlineData("somestring123", 123)]
        [InlineData("",              -1)]
        [InlineData("123somestring", -1)]
        public void TestGetNumberSuffix(string input, int expected)
        {
            Assert.Equal(expected, input.GetNumberSuffix());
        }

        [Theory]
        [InlineData(null,                    "00", "")]
        [InlineData("Givenname",             "10", "Givenname")]
        [InlineData("Name1 Name2 Name3",     "10", "Name1")]
        [InlineData("Name1 Name2 Name3",     "20", "Name2")]
        [InlineData("Name1 Name2 Name3",     "12", "Name1 Name2")]
        [InlineData("Name1 Name2-Name3 Name4", "12", "Name1 Name2")]
        [InlineData("Name1 Name2-Name3 Name4", "14", "Name1 Name4")]
        [InlineData("Name1 Name2-Name3 Name4", "23", "Name2-Name3")]
        [InlineData("Name1 Name2-Name3 Name4", "78", "")]
        public void TestGetProperFirstName(string input, string code, string expected)
        {
            Assert.Equal(expected, input.GetProperFirstName(code));
        }

        [Theory]
        [InlineData(null,                          "")]
        [InlineData("Givenname.Surname@Example.com",  "givenname.surname@example.com")]
        [InlineData("givénname.sûrname@example.cöm",  "givenname.surname@example.com")]
        [InlineData("givenname...surname@example.com", "givenname.surname@example.com")]
        [InlineData("givenname.sur name@example.com",  "givenname.sur.name@example.com")]
        [InlineData("givenname.sur-name@example.com",  "givenname.sur-name@example.com")]
        public void TestFormatAsEmailAddress(string input, string expected)
        {
            Assert.Equal(expected, input.FormatAsEmailAddress());
        }

        [Theory]
        [InlineData(null,               "")]
        [InlineData("Givenname.Surname", "givenname.surname")]
        [InlineData("givsur4",          "givsur4")]
        [InlineData("givsür",           "givsur")]
        [InlineData("givenname..surname", "givenname.surname")]
        public void TestFormatAsUsername(string input, string expected)
        {
            Assert.Equal(expected, input.FormatAsUsername());
        }

        // Remember to update tests before new years 2099/2100 ;-)
        [Theory]
        [InlineData(null,             "")]
        [InlineData("198909097788",   "198909097788")]
        [InlineData("19890909-7788",  "198909097788")]
        [InlineData("8909097788",     "198909097788")]
        [InlineData("0109097788",     "200109097788")]
        [InlineData("990909-7788",    "199909097788")]
        [InlineData("000909-7788",    "200009097788")]
        [InlineData("19890909TF88",   "19890909TF88")]
        [InlineData("19890909-TF88",  "19890909TF88")]
        [InlineData("890909TF88",     "19890909TF88")]
        [InlineData("010909TF88",     "20010909TF88")]
        public void TestFormatAsPersonnummer(string input, string expected)
        {
            Assert.Equal(expected, input.FormatAsPersonnummer());
        }

        // Remember to update tests before new years 2099/2100 ;-)
        [Theory]
        [InlineData(null,             "")]
        [InlineData("198909097788",   "890909-7788")]
        [InlineData("19890909-7788",  "890909-7788")]
        [InlineData("8909097788",     "890909-7788")]
        [InlineData("0109097788",     "010909-7788")]
        [InlineData("990909-7788",    "990909-7788")]
        [InlineData("000909-7788",    "000909-7788")]
        [InlineData("19890909TF88",   "890909-TF88")]
        [InlineData("19890909-TF88",  "890909-TF88")]
        [InlineData("890909TF88",     "890909-TF88")]
        [InlineData("010909TF88",     "010909-TF88")]
        public void TestFormatAsShortPersonnummer(string input, string expected)
        {
            Assert.Equal(expected, input.FormatAsShortPersonnummer());
        }
    }
}
