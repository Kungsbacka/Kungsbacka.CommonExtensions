using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kungsbacka.CommonExtensions.Tests
{
    [TestClass]
    public class TestStringExtensions
    {
        [TestMethod]
        public void TestIndexOfUnescaped()
        {
            Assert.AreEqual(-1, ((string)null).IndexOfUnescaped('.'), "#0");
            Assert.AreEqual(-1, @""           .IndexOfUnescaped('.'), "#1");
            Assert.AreEqual(-1, @""           .IndexOfUnescaped('.'), "#1");
            Assert.AreEqual( 0, @"."          .IndexOfUnescaped('.'), "#2");
            Assert.AreEqual( 1, @"A."         .IndexOfUnescaped('.'), "#3");
            Assert.AreEqual(-1, @"\."         .IndexOfUnescaped('.'), "#4");
            Assert.AreEqual( 2, @"\\."        .IndexOfUnescaped('.'), "#5");
            Assert.AreEqual(-1, @"A\."        .IndexOfUnescaped('.'), "#6");
            Assert.AreEqual( 3, @"A\\."       .IndexOfUnescaped('.'), "#7");
            Assert.AreEqual( 4, @"\\\\."      .IndexOfUnescaped('.'), "#8");
            Assert.AreEqual( 5, @"A\\\\."     .IndexOfUnescaped('.'), "#9");
            Assert.AreEqual(-1, @"\\\."       .IndexOfUnescaped('.'), "#10");
            Assert.AreEqual(-1, @"A\\\."      .IndexOfUnescaped('.'), "#11");
            Assert.AreEqual(-1, @"AA\."       .IndexOfUnescaped('.'), "#12");
            Assert.AreEqual(-1, @"\"          .IndexOfUnescaped('.'), "#13");
            Assert.AreEqual( 6, @"A\.A\\."    .IndexOfUnescaped('.'), "#14");
        }

        [TestMethod]
        public void TestLastIndexOfUnescaped()
        {
            Assert.AreEqual(-1, ((string)null).LastIndexOfUnescaped('.'), "#0");
            Assert.AreEqual( 3, @"ABC.123"    .LastIndexOfUnescaped('.'), "#1");
            Assert.AreEqual(-1, @"ABC\.123"   .LastIndexOfUnescaped('.'), "#2");
            Assert.AreEqual( 7, @"ABC.123."   .LastIndexOfUnescaped('.'), "#3");
            Assert.AreEqual( 8, @"ABC\.123."  .LastIndexOfUnescaped('.'), "#4");
            Assert.AreEqual(-1, @"\."         .LastIndexOfUnescaped('.'), "#5");
            Assert.AreEqual( 0, @".\.\\\."    .LastIndexOfUnescaped('.'), "#6");
            Assert.AreEqual(-1, @""           .LastIndexOfUnescaped('.'), "#7");
            Assert.AreEqual( 0, @"."          .LastIndexOfUnescaped('.'), "#8");
            Assert.AreEqual( 0, @".\."        .LastIndexOfUnescaped('.'), "#9");
            Assert.AreEqual(-1, @"\"          .LastIndexOfUnescaped('.'), "#10");
            Assert.AreEqual( 0, @".\"         .LastIndexOfUnescaped('.'), "#11");
            Assert.AreEqual( 3, @"\\.."       .LastIndexOfUnescaped('.'), "#12");
            Assert.AreEqual( 4, @"\\..."      .LastIndexOfUnescaped('.'), "#13");
        }

        [TestMethod]
        public void TestRemoveDiacritics()
        {
            Assert.AreEqual(""      , ((string)null).RemoveDiacritic(), false, "#0");
            Assert.AreEqual(""      , ""            .RemoveDiacritic(), false, "#1");
            Assert.AreEqual("aaoAAO", "åäöÅÄÖ"      .RemoveDiacritic(), false, "#2");
            Assert.AreEqual("nei´"  , "ñéí´"        .RemoveDiacritic(), false, "#3");
            Assert.AreEqual("ßÆØ"   , "ßÆØ"         .RemoveDiacritic(), false, "#4");
        }

        [TestMethod]
        public void TestRemoveRepeating()
        {
            Assert.AreEqual("", ((string)null).RemoveRepeating(new char[] { }));
            Assert.AreEqual("abbc", "aabbcc".RemoveRepeating(new char[] { 'a', 'c' }));
            Assert.AreEqual("-.-.-.-", "--.-.--..-".RemoveRepeating(new char[] { '.', '-' }));
            Assert.AreEqual(" ", "   ".RemoveRepeating(new char[] { ' ' }));
        }

        [TestMethod]
        public void TestGetNumberSuffix()
        {
            Assert.AreEqual(-1, ((string)null).GetNumberSuffix());
            Assert.AreEqual(123, "somestring123".GetNumberSuffix());
            Assert.AreEqual(-1, "".GetNumberSuffix());
            Assert.AreEqual(-1, "123somestring".GetNumberSuffix());
        }

        [TestMethod]
        public void TestGetProperFirstName()
        {
            string properName;

            properName = ((string)null).GetProperFirstName("00");
            Assert.AreEqual("", properName, false, "#0");

            properName = "Givenname".GetProperFirstName("10");
            Assert.AreEqual("Givenname", properName, false, "#1");

            properName = "Name1 Name2 Name3".GetProperFirstName("10");
            Assert.AreEqual("Name1", properName, false, "#2");

            properName = "Name1 Name2 Name3".GetProperFirstName("20");
            Assert.AreEqual("Name2", properName, false, "#3");

            properName = "Name1 Name2 Name3".GetProperFirstName("12");
            Assert.AreEqual("Name1 Name2", properName, false, "#4");
            
            properName = "Name1 Name2-Name3 Name4".GetProperFirstName("12");
            Assert.AreEqual("Name1 Name2", properName, false, "#5");

            properName = "Name1 Name2-Name3 Name4".GetProperFirstName("14");
            Assert.AreEqual("Name1 Name4", properName, false, "#6");

            properName = "Name1 Name2-Name3 Name4".GetProperFirstName("23");
            Assert.AreEqual("Name2-Name3", properName, false, "#7");

            properName = "Name1 Name2-Name3 Name4".GetProperFirstName("78");
            Assert.AreEqual("", properName, false, "#8");
        }

        [TestMethod]
        public void TestFormatAsEmailAddress()
        {
            Assert.AreEqual("", ((string)null).FormatAsEmailAddress(), false, "#0");
            Assert.AreEqual("givenname.surname@example.com", "Givenname.Surname@Example.com".FormatAsEmailAddress(),   "#1");
            Assert.AreEqual("givenname.surname@example.com", "givénname.sûrname@example.cöm".FormatAsEmailAddress(),   "#2");
            Assert.AreEqual("givenname.surname@example.com", "givenname...surname@example.com".FormatAsEmailAddress(), "#3");
            Assert.AreEqual("givenname.sur.name@example.com", "givenname.sur name@example.com".FormatAsEmailAddress(), "#4");
            Assert.AreEqual("givenname.sur-name@example.com", "givenname.sur-name@example.com".FormatAsEmailAddress(), "#5");
        }

        [TestMethod]
        public void TestFormatAsUsername()
        {
            Assert.AreEqual("", ((string)null).FormatAsUsername());
            Assert.AreEqual("givenname.surname", "Givenname.Surname".FormatAsUsername());
            Assert.AreEqual("givsur4", "givsur4".FormatAsUsername());
            Assert.AreEqual("givsur", "givsür".FormatAsUsername());
            Assert.AreEqual("givenname.surname", "givenname..surname".FormatAsUsername());
        }

        [TestMethod]
        public void TestFormatAsPersonnummer()
        {
            // Remember to update tests before new years 2099/2100 ;-)
            Assert.AreEqual("", ((string)null).FormatAsPersonnummer(),              "#0");
            Assert.AreEqual("198909097788", "198909097788".FormatAsPersonnummer(),  "#1");
            Assert.AreEqual("198909097788", "19890909-7788".FormatAsPersonnummer(), "#2");
            Assert.AreEqual("198909097788", "8909097788".FormatAsPersonnummer(),    "#3");
            Assert.AreEqual("200109097788", "0109097788".FormatAsPersonnummer(),    "#4");
            Assert.AreEqual("199909097788", "990909-7788".FormatAsPersonnummer(),   "#5");
            Assert.AreEqual("200009097788", "000909-7788".FormatAsPersonnummer(),   "#6");
            Assert.AreEqual("19890909TF88", "19890909TF88".FormatAsPersonnummer(),  "#7");
            Assert.AreEqual("19890909TF88", "19890909-TF88".FormatAsPersonnummer(), "#8");
            Assert.AreEqual("19890909TF88", "890909TF88".FormatAsPersonnummer(),    "#9");
            Assert.AreEqual("20010909TF88", "010909TF88".FormatAsPersonnummer(),    "#10");
        }

        [TestMethod]
        public void TestFormatAsShortPersonnummer()
        {
            // Remember to update tests before new years 2099/2100 ;-)
            Assert.AreEqual("", ((string)null).FormatAsPersonnummer(),                  "#0");
            Assert.AreEqual("890909-7788", "198909097788".FormatAsShortPersonnummer(),  "#1");
            Assert.AreEqual("890909-7788", "19890909-7788".FormatAsShortPersonnummer(), "#2");
            Assert.AreEqual("890909-7788", "8909097788".FormatAsShortPersonnummer(),    "#3");
            Assert.AreEqual("010909-7788", "0109097788".FormatAsShortPersonnummer(),    "#4");
            Assert.AreEqual("990909-7788", "990909-7788".FormatAsShortPersonnummer(),   "#5");
            Assert.AreEqual("000909-7788", "000909-7788".FormatAsShortPersonnummer(),   "#6");
            Assert.AreEqual("890909-TF88", "19890909TF88".FormatAsShortPersonnummer(),  "#7");
            Assert.AreEqual("890909-TF88", "19890909-TF88".FormatAsShortPersonnummer(), "#8");
            Assert.AreEqual("890909-TF88", "890909TF88".FormatAsShortPersonnummer(),    "#9");
            Assert.AreEqual("010909-TF88", "010909TF88".FormatAsShortPersonnummer(),    "#10");
        }
    }
}
