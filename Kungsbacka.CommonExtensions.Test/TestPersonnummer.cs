using Xunit;
using System;

namespace Kungsbacka.CommonExtensions.Tests
{
    public class TestPersonnummer
    {
        [Theory]
        [InlineData("194912232594",  "194912232594", "491223-2594", false, "19491223")]
        [InlineData("19491223TF94",  "19491223TF94", "491223-TF94", true,  "19491223")]
        [InlineData("19491223-2594", "194912232594", "491223-2594", false, "19491223")]
        [InlineData("19491223-TF94", "19491223TF94", "491223-TF94", true,  "19491223")]
        [InlineData("4912232594",    "194912232594", "491223-2594", false, "19491223")]
        [InlineData("491223TF94",    "19491223TF94", "491223-TF94", true,  "19491223")]
        [InlineData("491223-2594",   "194912232594", "491223-2594", false, "19491223")]
        [InlineData("491223-TF94",   "19491223TF94", "491223-TF94", true,  "19491223")]
        [InlineData("194912832591",  "194912832591", "491283-2591", false, "19491223")]
        public void TestValid(string input, string longForm, string shortForm, bool isTemporary, string birthDate)
        {
            Personnummer personnummer = new(input);
            Assert.Equal(input,      personnummer.Original);
            Assert.Equal(longForm,   personnummer.LongForm);
            Assert.Equal(shortForm,  personnummer.ShortForm);
            Assert.Equal(isTemporary, personnummer.IsTemporary);
            Assert.Equal(DateTime.ParseExact(birthDate, "yyyyMMdd", null), personnummer.BirthDate);
        }

        [Fact]
        public void TestNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Personnummer(null));
        }

        [Theory]
        [InlineData("          ")]
        [InlineData("123456789")]
        [InlineData("12345-6789")]
        [InlineData("abcdefghijkl")]
        [InlineData("491223tf94")]
        [InlineData("194912232584")]
        [InlineData("8013015212")]
        public void TestInvalidFormat(string input)
        {
            Assert.Throws<FormatException>(() => new Personnummer(input));
        }

        [Fact]
        public void TestFutureBirthDate()
        {
            Personnummer personnummer = new("801301015218");
            Assert.Equal(DateTime.Parse("8013-01-1"), personnummer.BirthDate);
        }
    }
}
