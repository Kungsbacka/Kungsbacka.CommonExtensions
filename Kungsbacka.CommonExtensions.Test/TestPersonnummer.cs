using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Kungsbacka.CommonExtensions.Tests
{

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    internal class TestResult
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public string LongForm { get; set; }
        public string ShortForm { get; set; }
        public string Original { get; set; }
        public bool IsTemporary { get; set; }
        public DateTime BirthDate { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TestResult result &&
                   LongForm == result.LongForm &&
                   ShortForm == result.ShortForm &&
                   Original == result.Original &&
                   IsTemporary == result.IsTemporary &&
                   BirthDate == result.BirthDate;
        }
    }

    [TestClass]
    public class TestPersonnummer
    {
        readonly IReadOnlyDictionary<string, TestResult> ValidTestCases = new Dictionary<string, TestResult>()
        {
            { "194912232594",   new TestResult() {Original = "194912232594" , LongForm = "194912232594", ShortForm = "491223-2594", IsTemporary = false, BirthDate = DateTime.ParseExact("19491223", "yyyyMMdd", null) } },
            { "19491223TF94",   new TestResult() {Original = "19491223TF94" , LongForm = "19491223TF94", ShortForm = "491223-TF94", IsTemporary = true , BirthDate = DateTime.ParseExact("19491223", "yyyyMMdd", null) } },
            { "19491223-2594",  new TestResult() {Original = "19491223-2594", LongForm = "194912232594", ShortForm = "491223-2594", IsTemporary = false, BirthDate = DateTime.ParseExact("19491223", "yyyyMMdd", null) } },
            { "19491223-TF94",  new TestResult() {Original = "19491223-TF94", LongForm = "19491223TF94", ShortForm = "491223-TF94", IsTemporary = true , BirthDate = DateTime.ParseExact("19491223", "yyyyMMdd", null) } },
            { "4912232594",     new TestResult() {Original = "4912232594"   , LongForm = "194912232594", ShortForm = "491223-2594", IsTemporary = false, BirthDate = DateTime.ParseExact("19491223", "yyyyMMdd", null) } },
            { "491223TF94",     new TestResult() {Original = "491223TF94"   , LongForm = "19491223TF94", ShortForm = "491223-TF94", IsTemporary = true , BirthDate = DateTime.ParseExact("19491223", "yyyyMMdd", null) } },
            { "491223-2594",    new TestResult() {Original = "491223-2594"  , LongForm = "194912232594", ShortForm = "491223-2594", IsTemporary = false, BirthDate = DateTime.ParseExact("19491223", "yyyyMMdd", null) } },
            { "491223-TF94",    new TestResult() {Original = "491223-TF94"  , LongForm = "19491223TF94", ShortForm = "491223-TF94", IsTemporary = true , BirthDate = DateTime.ParseExact("19491223", "yyyyMMdd", null) } },
            { "194912832591",   new TestResult() {Original = "194912832591" , LongForm = "194912832591", ShortForm = "491283-2591", IsTemporary = false, BirthDate = DateTime.ParseExact("19491223", "yyyyMMdd", null) } },
        };

        [TestMethod]
        public void TestValid()
        {
            foreach (string key in ValidTestCases.Keys)
            {
                Personnummer personnummer = new Personnummer(key);
                TestResult actual = new TestResult()
                {
                    Original = personnummer.Original,
                    LongForm = personnummer.LongForm,
                    ShortForm = personnummer.ShortForm,
                    IsTemporary = personnummer.IsTemporary,
                    BirthDate = personnummer.BirthDate
                };
                Assert.IsTrue(ValidTestCases[key].Equals(actual), key);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull()
        {
            new Personnummer(null);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestEmpty()
        {
            new Personnummer("          ");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestBad1()
        {
            new Personnummer("123456789");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestBad2()
        {
            new Personnummer("12345-6789");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestBad3()
        {
            new Personnummer("abcdefghijkl");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestBad4()
        {
            new Personnummer("491223tf94");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestBadCheckDigit()
        {
            new Personnummer("194912232584");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestBadBirthDate()
        {
            new Personnummer("8013015212");
        }

        [TestMethod]
        public void TestFutureBirthDate()
        {
            Personnummer personnummer = new Personnummer("801301015218");
            Assert.AreEqual(personnummer.BirthDate, DateTime.Parse("8013-01-1"));
        }
    }
}
