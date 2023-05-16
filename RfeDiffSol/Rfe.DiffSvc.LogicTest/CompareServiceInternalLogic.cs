using NUnit.Framework;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Rfe.DiffSvc.WebApi.Interfaces.Services;
using Rfe.DiffSvc.WebApi.Services;
using Rfe.DiffSvc.WebApi.BusinessObjects;



namespace Rfe.DiffSvc.LogicTest
{



    /// <summary>
    /// Unit-tests methods of the CompareService class.
    /// </summary>
    [TestFixture]
    public class CompareServiceInternalLogic
    {



        // Compare service to test.
        private CompareService _compareService;



        // Before each test.
        [SetUp]
        public void Setup()
        {
            _compareService = new CompareService();
        }



        // After each test.
        [TearDown]
        public void Teardown()
        {
            _compareService = null;
        }



        // **************************************************************************************************
        // **************************************************************************************************
        // **************************************************************************************************



        // Test a "LeqR" case.
        [Test]
        public void CompareAndFindDiffsWithTwoEqualStringsTest()
        {
            string first = "Má nejdražší Marie, rozhodl jsem se vyznati Vám lásku. Mé řečnické dovednosti jsou však dosti omezené. Proto Vám raději píši tento dopis...";
            string second = first;

            int compareResult;
            List<StringSection> differingSections;

            _compareService.CompareAndFindDiffs(first, second, out compareResult, out differingSections);

            Assert.Zero(compareResult, "CompareAndFindDiffs() - Expected result: 'strings are same', actual compare result: {0}", compareResult);
            //Assert.That(differingSections == null || differingSections.Count == 0, "CompareAndFindDiffs() - Expected number of diffs found: zero, actual: {0}", differingSections.Count);
            Assert.That(differingSections == null || differingSections.Count == 0, "CompareAndFindDiffs() - Expected number of diffs found: zero, actual: {0}", GetCountOfItems(differingSections));
        }



        // Test a "LgtR" case.
        [Test]
        public void CompareAndFindDiffsWithFirstLongerThanSecondTest()
        {
            string first = "Tohle je dosti dlouhý text, že?";
            string second = "Toto ne.";

            int compareResult;
            List<StringSection> differingSections;

            _compareService.CompareAndFindDiffs(first, second, out compareResult, out differingSections);

            Assert.Greater(compareResult, 0, "CompareAndFindDiffs() - Expected result: 'first is longer than second', actual compare result: {0}", compareResult);
            //Assert.That(differingSections == null || differingSections.Count == 0, "CompareAndFindDiffs() - Expected number of diffs found: zero, actual: {0}", differingSections.Count);
            Assert.That(differingSections == null || differingSections.Count == 0, "CompareAndFindDiffs() - Expected number of diffs found: zero, actual: {0}", GetCountOfItems(differingSections));
        }



        // Test a "LltR" case.
        [Test]
        public void CompareAndFindDiffsWithSecondLongerThanFirstTest()
        {
            string first = "Tohle je dosti dlouhý text, že?";
            string second = "Toto je však ještě o mnooooohoooooo delší textík, čili by měl vyhrát, není-liž pravda?";

            int compareResult;
            List<StringSection> differingSections;

            _compareService.CompareAndFindDiffs(first, second, out compareResult, out differingSections);

            Assert.Less(compareResult, 0, "CompareAndFindDiffs() - Expected result: 'first is shorter than second', actual compare result: {0}", compareResult);
            //Assert.That(differingSections == null || differingSections.Count == 0, "CompareAndFindDiffs() - Expected number of diffs found: zero, actual: {0}", differingSections.Count);
            Assert.That(differingSections == null || differingSections.Count == 0, "CompareAndFindDiffs() - Expected number of diffs found: zero, actual: {0}", GetCountOfItems(differingSections));
        }



        // Test a "LdiR" case.
        // Four short sections different.
        [Test]
        public void CompareAndFindDiffsWithTwoStringsEqualLengthTwoDiffSectionsTest()
        {
            //                     10        20        30        40        50        60        70        80        90        100       110       120       130
            //           0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678
            string s1 = "Má nejdražší Marie, rozhodl jsem se vyznati Vám lásku. Mé řečnické dovednosti jsou však dosti omezené. Proto Vám raději píši tento dopis...";
            //                               ddddddd                 dd                                ddddddddddddddddddddddd                              dddddddd
            //                               1234567                 12                                12345678901234567890123                              12345678
            string s2 = "Má nejdražší Marie, ROZHODL jsem se vyznati jim lásku. Mé řečnické dovednosti nejsou tuto nijak valaš. Proto Vám raději píši tento ...sipod";

            int expectDifferingSectionsCount = 4;
            List<StringSection> expectDifferingSections = new List<StringSection>();
            // "rozhodl" vs "ROZHODL"
            expectDifferingSections.Add(new StringSection(20, 7));
            // "Vá" vs "ji"
            expectDifferingSections.Add(new StringSection(44, 2));
            // "jsou však dosti omezené" vs "nejsou tuto nijak valaš"
            expectDifferingSections.Add(new StringSection(78, 23));
            // "dopis..." vs "...sipod"
            expectDifferingSections.Add(new StringSection(131, 8));

            int compareResult;
            List<StringSection> differingSections;

            _compareService.CompareAndFindDiffs(s1, s2, out compareResult, out differingSections);

            Assert.Zero(compareResult, "CompareAndFindDiffs() - Expected result: 'strings are of same length, but still different', actual compare result: {0}", compareResult);
            Assert.IsNotNull(differingSections);
            Assert.AreEqual(expectDifferingSectionsCount, differingSections.Count, "CompareAndFindDiffs() - Expected number of diffs found: {0}, actual: {1}", expectDifferingSectionsCount, differingSections.Count);

            expectDifferingSections.Sort();
            differingSections.Sort();

            CollectionAssert.AreEqual(expectDifferingSections, differingSections, "CompareAndFindDiffs() - Expected diffs: {0}, actual: {1}", DifferingSectionsToString(expectDifferingSections), DifferingSectionsToString(differingSections));
        }



        // Test a "LdiR" case.
        // The strings, although having the same length, are completely different from each other. They differ in "each and every" character.
        [Test]
        public void CompareAndFindDiffsWithTwoStringsEqualLengthButTotallyDifferentCharsTest()
        {
            string s1 = "abcdefghijklmnopqrstuvwxyz";
            string s2 = "zyxwvutsrqponmlkjihgfedcba";

            int expectDifferingSectionsCount = 1;
            List<StringSection> expectDifferingSections = new List<StringSection>();
            expectDifferingSections.Add(new StringSection(0, 26));

            int compareResult;
            List<StringSection> differingSections;

            _compareService.CompareAndFindDiffs(s1, s2, out compareResult, out differingSections);

            Assert.Zero(compareResult, "CompareAndFindDiffs() - Expected result: 'strings are of same length, but still different', actual compare result: {0}", compareResult);
            Assert.IsNotNull(differingSections);
            Assert.AreEqual(expectDifferingSectionsCount, differingSections.Count, "CompareAndFindDiffs() - Expected number of diffs found: {0}, actual: {1}", expectDifferingSectionsCount, differingSections.Count);

            expectDifferingSections.Sort();
            differingSections.Sort();

            CollectionAssert.AreEqual(expectDifferingSections, differingSections, "CompareAndFindDiffs() - Expected diffs: {0}, actual: {1}", DifferingSectionsToString(expectDifferingSections), DifferingSectionsToString(differingSections));
        }



        // **************************************************************************************************
        // **************************************************************************************************
        // **************************************************************************************************



        // Converts a given list of diff sections into a string.
        private string DifferingSectionsToString(List<StringSection> differingSections)
        {
            return differingSections.Select(ds => ds.ToString()).Aggregate((acc, dsStr) => acc + ", " + dsStr);
        }



        // Helper method to get the number of items, but only in case the given parameter refers to a real object.
        // If the collection is null, we shall return a different number in order to show that status. We will return -1.
        //private int GetCountOfItems<T>(ICollection<T> collection)
        private int GetCountOfItems(ICollection collection)
        {
            if (collection == null)
            {
                return -1;
            }
            return collection.Count;
        }



    }



}
