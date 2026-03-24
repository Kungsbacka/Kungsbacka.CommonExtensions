using Xunit;

namespace Kungsbacka.CommonExtensions.Tests
{
    public class TestStringExtensions
    {
        [Fact]
        public void TestIndexOfUnescaped()
        {
            Assert.Equal(-1, ((string)null).IndexOfUnescaped('.'));  // #0
            Assert.Equal(-1, @""           .IndexOfUnescaped('.'));  // #1
            Assert.Equal(-1, @""           .IndexOfUnescaped('.'));  // #1
            Assert.Equal( 0, @"."          .IndexOfUnescaped('.'));  // #2
            Assert.Equal( 1, @"A."         .IndexOfUnescaped('.'));  // #3
            Assert.Equal(-1, @"\."         .IndexOfUnescaped('.'));  // #4
            Assert.Equal( 2, @"\\."        .IndexOfUnescaped('.'));  // #5
            Assert.Equal(-1, @"A\."        .IndexOfUnescaped('.'));  // #6
            Assert.Equal( 3, @"A\\."       .IndexOfUnescaped('.'));  // #7
            Assert.Equal( 4, @"\\\\."      .IndexOfUnescaped('.'));  // #8
            Assert.Equal( 5, @"A\\\\."     .IndexOfUnescaped('.'));  // #9
            Assert.Equal(-1, @"\\\."       .IndexOfUnescaped('.'));  // #10
            Assert.Equal(-1, @"A\\\."      .IndexOfUnescaped('.'));  // #11
            Assert.Equal(-1, @"AA\."       .IndexOfUnescaped('.'));  // #12
            Assert.Equal(-1, @"\"          .IndexOfUnescaped('.'));  // #13
            Assert.Equal( 6, @"A\.A\\."    .IndexOfUnescaped('.'));  // #14
        }

        [Fact]
        public void TestLastIndexOfUnescaped()
        {
            Assert.Equal(-1, ((string)null).LastIndexOfUnescaped('.'));  // #0
            Assert.Equal( 3, @"ABC.123"    .LastIndexOfUnescaped('.'));  // #1
            Assert.Equal(-1, @"ABC\.123"   .LastIndexOfUnescaped('.'));  // #2
            Assert.Equal( 7, @"ABC.123."   .LastIndexOfUnescaped('.'));  // #3
            Assert.Equal( 8, @"ABC\.123."  .LastIndexOfUnescaped('.'));  // #4
            Assert.Equal(-1, @"\."         .LastIndexOfUnescaped('.'));  // #5
            Assert.Equal( 0, @".\.\\\."    .LastIndexOfUnescaped('.'));  // #6
            Assert.Equal(-1, @""           .LastIndexOfUnescaped('.'));  // #7
            Assert.Equal( 0, @"."          .LastIndexOfUnescaped('.'));  // #8
            Assert.Equal( 0, @".\."        .LastIndexOfUnescaped('.'));  // #9
            Assert.Equal(-1, @"\"          .LastIndexOfUnescaped('.'));  // #10
            Assert.Equal( 0, @".\"         .LastIndexOfUnescaped('.'));  // #11
            Assert.Equal( 3, @"\\.."       .LastIndexOfUnescaped('.'));  // #12
            Assert.Equal( 4, @"\\..."      .LastIndexOfUnescaped('.'));  // #13
        }

        [Fact]
        public void TestRemoveDiacritics()
        {
            Assert.Equal(""      , ((string)null).RemoveDiacritic());  // #0
            Assert.Equal(""      , ""            .RemoveDiacritic());  // #1
            Assert.Equal("aaoAAO", "åäöÅÄÖ"      .RemoveDiacritic());  // #2
            Assert.Equal("nei´"  , "ñéí´"        .RemoveDiacritic());  // #3
            Assert.Equal("ßÆØ"   , "ßÆØ"         .RemoveDiacritic());  // #4
        }

        [Fact]
        public void TestRemoveRepeating()
        {
            Assert.Equal("", ((string)null).RemoveRepeating(new char[] { }));
            Assert.Equal("abbc", "aabbcc".RemoveRepeating(new char[] { 'a', 'c' }));
            Assert.Equal("-.-.-.-", "--.-.--..-".RemoveRepeating(new char[] { '.', '-' }));
            Assert.Equal(" ", "   ".RemoveRepeating(new char[] { ' ' }));
        }

        [Fact]
        public void TestGetNumberSuffix()
        {
            Assert.Equal(-1, ((string)null).GetNumberSuffix());
            Assert.Equal(123, "somestring123".GetNumberSuffix());
            Assert.Equal(-1, "".GetNumberSuffix());
            Assert.Equal(-1, "123somestring".GetNumberSuffix());
        }

        [Fact]
        public void TestGetProperFirstName()
        {
            string properName;

            properName = ((string)null).GetProperFirstName("00");
            Assert.Equal("", properName);  // #0

            properName = "Givenname".GetProperFirstName("10");
            Assert.Equal("Givenname", properName);  // #1

            properName = "Name1 Name2 Name3".GetProperFirstName("10");
            Assert.Equal("Name1", properName);  // #2

            properName = "Name1 Name2 Name3".GetProperFirstName("20");
            Assert.Equal("Name2", properName);  // #3

            properName = "Name1 Name2 Name3".GetProperFirstName("12");
            Assert.Equal("Name1 Name2", properName);  // #4

            properName = "Name1 Name2-Name3 Name4".GetProperFirstName("12");
            Assert.Equal("Name1 Name2", properName);  // #5

            properName = "Name1 Name2-Name3 Name4".GetProperFirstName("14");
            Assert.Equal("Name1 Name4", properName);  // #6

            properName = "Name1 Name2-Name3 Name4".GetProperFirstName("23");
            Assert.Equal("Name2-Name3", properName);  // #7

            properName = "Name1 Name2-Name3 Name4".GetProperFirstName("78");
            Assert.Equal("", properName);  // #8
        }

        [Fact]
        public void TestFormatAsEmailAddress()
        {
            Assert.Equal("", ((string)null).FormatAsEmailAddress());                                         // #0
            Assert.Equal("givenname.surname@example.com", "Givenname.Surname@Example.com".FormatAsEmailAddress());   // #1
            Assert.Equal("givenname.surname@example.com", "givénname.sûrname@example.cöm".FormatAsEmailAddress());   // #2
            Assert.Equal("givenname.surname@example.com", "givenname...surname@example.com".FormatAsEmailAddress()); // #3
            Assert.Equal("givenname.sur.name@example.com", "givenname.sur name@example.com".FormatAsEmailAddress()); // #4
            Assert.Equal("givenname.sur-name@example.com", "givenname.sur-name@example.com".FormatAsEmailAddress()); // #5
        }

        [Fact]
        public void TestFormatAsUsername()
        {
            Assert.Equal("", ((string)null).FormatAsUsername());
            Assert.Equal("givenname.surname", "Givenname.Surname".FormatAsUsername());
            Assert.Equal("givsur4", "givsur4".FormatAsUsername());
            Assert.Equal("givsur", "givsür".FormatAsUsername());
            Assert.Equal("givenname.surname", "givenname..surname".FormatAsUsername());
        }

        [Fact]
        public void TestFormatAsPersonnummer()
        {
            // Remember to update tests before new years 2099/2100 ;-)
            Assert.Equal("", ((string)null).FormatAsPersonnummer());                // #0
            Assert.Equal("198909097788", "198909097788".FormatAsPersonnummer());  // #1
            Assert.Equal("198909097788", "19890909-7788".FormatAsPersonnummer()); // #2
            Assert.Equal("198909097788", "8909097788".FormatAsPersonnummer());    // #3
            Assert.Equal("200109097788", "0109097788".FormatAsPersonnummer());    // #4
            Assert.Equal("199909097788", "990909-7788".FormatAsPersonnummer());   // #5
            Assert.Equal("200009097788", "000909-7788".FormatAsPersonnummer());   // #6
            Assert.Equal("19890909TF88", "19890909TF88".FormatAsPersonnummer());  // #7
            Assert.Equal("19890909TF88", "19890909-TF88".FormatAsPersonnummer()); // #8
            Assert.Equal("19890909TF88", "890909TF88".FormatAsPersonnummer());    // #9
            Assert.Equal("20010909TF88", "010909TF88".FormatAsPersonnummer());    // #10
        }

        [Fact]
        public void TestFormatAsShortPersonnummer()
        {
            // Remember to update tests before new years 2099/2100 ;-)
            Assert.Equal("", ((string)null).FormatAsPersonnummer());                              // #0
            Assert.Equal("890909-7788", "198909097788".FormatAsShortPersonnummer());  // #1
            Assert.Equal("890909-7788", "19890909-7788".FormatAsShortPersonnummer()); // #2
            Assert.Equal("890909-7788", "8909097788".FormatAsShortPersonnummer());    // #3
            Assert.Equal("010909-7788", "0109097788".FormatAsShortPersonnummer());    // #4
            Assert.Equal("990909-7788", "990909-7788".FormatAsShortPersonnummer());   // #5
            Assert.Equal("000909-7788", "000909-7788".FormatAsShortPersonnummer());   // #6
            Assert.Equal("890909-TF88", "19890909TF88".FormatAsShortPersonnummer());  // #7
            Assert.Equal("890909-TF88", "19890909-TF88".FormatAsShortPersonnummer()); // #8
            Assert.Equal("890909-TF88", "890909TF88".FormatAsShortPersonnummer());    // #9
            Assert.Equal("010909-TF88", "010909TF88".FormatAsShortPersonnummer());    // #10
        }
    }
}
