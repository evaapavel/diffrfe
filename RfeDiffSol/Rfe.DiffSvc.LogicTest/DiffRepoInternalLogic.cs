using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;

using Rfe.DiffSvc.WebApi.BusinessObjects;
using Rfe.DiffSvc.WebApi.Interfaces.Repos;
using Rfe.DiffSvc.WebApi.Repos;



namespace Rfe.DiffSvc.LogicTest
{



    /// <summary>
    /// Unit-tests methods of the DiffRepo class.
    /// </summary>
    [TestFixture]
    public class DiffRepoInternalLogic
    {



        // Repo object to test.
        // We will not use the interface here.
        // We add some "helper" methods (such as Clear() or GetCount()) into the DiffRepo class.
        private DiffRepo _diffRepo;



        // Before each test.
        [SetUp]
        public void Setup()
        {
            _diffRepo = new DiffRepo();
            //_diffRepo.Clear();
        }



        // After each test.
        [TearDown]
        public void Teardown()
        {
            _diffRepo.Clear();
            _diffRepo = null;
        }



        // **************************************************************************************************
        // **************************************************************************************************
        // **************************************************************************************************



        // Tests the GetList method.
        [Test]
        public void GetListTest()
        {
            PrepareRepoWithThreeItems();

            int expect = 3;
            int actual = _diffRepo.GetCount();

            Assert.AreEqual(expect, actual, "GetList() - number of items expected: {0}, actually: {1}", expect, actual);
        }



        // Tests the FindList method.
        [Test]
        public void FindListTest()
        {
            PrepareRepoWithSevenItems();

            int expectNoResultYet = 4;
            DiffFilter filterNoResultYet = new DiffFilter { HasOutput = false };
            IList<Diff> diffsNoResultYet = _diffRepo.FindList(filterNoResultYet);
            int actualNoResultYet = diffsNoResultYet.Count;

            Assert.AreEqual(expectNoResultYet, actualNoResultYet, "FindList() - number of items expected: {0}, actually: {1}", expectNoResultYet, actualNoResultYet);
            //CollectionAssert.AreEqual();
            //CollectionAssert.AreEqual(diffsNoResultYet.Select(d => d.Output), "".PadRight(expectNoResultYet).Select(ch => (DiffOutput) null));
            foreach (Diff diff in diffsNoResultYet)
            {
                Assert.That(diff.Output == null);
            }

            int expectResultLltR = 2;
            DiffFilter filterResultLltR = new DiffFilter { HasOutput = true, Results = DiffResult.LltR };
            IList<Diff> diffsResultLltR = _diffRepo.FindList(filterResultLltR);
            int actualResultLltR = diffsResultLltR.Count;

            Assert.AreEqual(expectResultLltR, actualResultLltR, "FindList() - number of items expected: {0}, actually: {1}", expectResultLltR, actualResultLltR);
            foreach (Diff diff in diffsNoResultYet)
            {
                Assert.That(diff.Output != null);
                Assert.AreEqual(diff.Output.Result, DiffResult.LltR, "FindList() - all items should have their result set to LltR");
            }
        }



        // Tests the Exists method.
        [Test]
        public void ExistsTest()
        {
            PrepareRepoWithSevenItems();

            DiffFilter filterLeftInputSet = new DiffFilter { HasLeftInput = true };
            bool actualLeftInputSet = _diffRepo.Exists(filterLeftInputSet);

            Assert.True(actualLeftInputSet, "Exists() - there should be at least one item with left input set");


            DiffFilter filterInconsistentDiffsWithEmptyLeftInput = new DiffFilter { HasLeftInput = false, HasOutput = true };
            DiffFilter filterInconsistentDiffsWithEmptyRightInput = new DiffFilter { HasRightInput = false, HasOutput = true };

            bool actualInconsistentDiffsWithEmptyLeftInput = _diffRepo.Exists(filterInconsistentDiffsWithEmptyLeftInput);
            bool actualInconsistentDiffsWithEmptyRightInput = _diffRepo.Exists(filterInconsistentDiffsWithEmptyRightInput);

            bool actualInconsistentDiffsWithEmptyLeftOrRightInput = actualInconsistentDiffsWithEmptyLeftInput || actualInconsistentDiffsWithEmptyRightInput;

            Assert.False(actualInconsistentDiffsWithEmptyLeftOrRightInput, "Exists() - there should not exist inconsistent Diffs");
        }



        // Tests the Load method.
        [Test]
        public void LoadTest()
        {
            PrepareEmptyRepo();

            string leftInput = "Hi Johnny!";
            Diff diffToAdd = new Diff(Guid.Empty, new StreamInput(leftInput), null, null);

            Diff diffAdded = _diffRepo.Add(diffToAdd);

            Guid diffID = diffAdded.ID;

            Diff idWrapper = new Diff { ID = diffID };

            Diff diffLoaded = _diffRepo.Load(idWrapper);

            Assert.AreEqual(diffID, diffLoaded.ID, "Load() - expected ID: {0}, actual ID: {1}", diffID, diffLoaded.ID);
            Assert.IsNotNull(diffLoaded.Left);
            Assert.AreEqual(leftInput, diffLoaded.Left.Input, "Load() - expected Left: {0}, actual: {1}", leftInput, diffLoaded.Left.Input);
            Assert.IsNull(diffLoaded.Right);
            Assert.IsNull(diffLoaded.Output);
        }



        // Tests the Store method.
        [Test]
        public void StoreTest()
        {
            PrepareEmptyRepo();

            string leftInput = "Bye Chris!";
            string rightInputOriginal = "Bye-bye bro...";
            string rightInputReplacing = "Hi bro!";

            Diff diffToAdd = new Diff(Guid.Empty, new StreamInput(leftInput), new StreamInput(rightInputOriginal), null);

            Diff diffAdded = _diffRepo.Add(diffToAdd);

            Guid diffID = diffAdded.ID;

            Diff idWrapper = new Diff { ID = diffID };

            Diff diffLoaded = _diffRepo.Load(idWrapper);

            Assert.That(diffLoaded.Right != null && diffLoaded.Right.Input == rightInputOriginal);

            diffLoaded.Right = new StreamInput(rightInputReplacing);

            _diffRepo.Store(diffLoaded);

            Diff idWrapperSecond = new Diff { ID = diffID };

            Diff diffLoadedSecond = _diffRepo.Load(idWrapperSecond);

            Assert.IsNotNull(diffLoadedSecond.Right);
            Assert.AreEqual(rightInputReplacing, diffLoadedSecond.Right.Input, "Store() - expected Right: {0}, actual: {1}", rightInputReplacing, diffLoadedSecond.Right.Input);
        }



        // Tests the Add method.
        [Test]
        public void AddTest()
        {
            PrepareRepoWithThreeItems();

            Diff fourthToAdd = new Diff(Guid.Empty, new StreamInput("Ahoj"), new StreamInput("Ahoj"), new DiffOutput(DiffResult.LeqR));
            Diff fifthToAdd = new Diff(Guid.Empty, new StreamInput("Nazdar"), null, null);

            _diffRepo.Add(fourthToAdd);
            _diffRepo.Add(fifthToAdd);

            int expectCount = 5;
            int actualCunt = _diffRepo.GetCount();

            Assert.AreEqual(expectCount, actualCunt, "Add() - expected {0} items after Add, actual: {1}", expectCount, actualCunt);
        }



        // Tests the Remove method.
        [Test]
        public void RemoveTest()
        {
            PrepareRepoWithThreeItems();

            IList<Diff> diffs = _diffRepo.GetList();
            List<Guid> ids = diffs.Select(d => d.ID).ToList();

            int expectBeforeRemoves = 3;
            int actualBeforeRemoves = _diffRepo.GetCount();

            Assert.AreEqual(expectBeforeRemoves, actualBeforeRemoves, "Before Remove() - expected {0} items, but found just {1}", expectBeforeRemoves, actualBeforeRemoves);

            foreach (Guid id in ids)
            {
                Diff idWrapper = new Diff { ID = id };
                _diffRepo.Remove(idWrapper);
            }

            int expectAfterRemoves = 0;
            int actualAfterRemoves = _diffRepo.GetCount();

            Assert.AreEqual(expectAfterRemoves, actualAfterRemoves, "After Remove() - expected {0} items, but found just {1}", expectAfterRemoves, actualAfterRemoves);
        }



        // **************************************************************************************************
        // **************************************************************************************************
        // **************************************************************************************************



        // Clears the repo.
        private void PrepareEmptyRepo()
        {
            _diffRepo.Clear();
        }



        // Prepares a repo with 3 Diff objects.
        private void PrepareRepoWithThreeItems()
        {
            _diffRepo.Clear();

            Diff first = new Diff(Guid.Empty, new StreamInput("Adolf má rád psy."), new StreamInput("Karel má rád psy."), null);

            // 01234567890123456789
            // .......d.d......dddd
            // 01234569870123459876
            Diff second = new Diff(Guid.Empty, new StreamInput("01234567890123456789"), new StreamInput("01234569870123459876"), new DiffOutput(DiffResult.LdiR, new StringSection[] { new StringSection(7, 1), new StringSection(9, 1), new StringSection(16, 4) }));

            Diff third = new Diff(Guid.Empty, new StreamInput("Adolf má rád psy."), new StreamInput("Adolf má rád psy baskervillské."), new DiffOutput(DiffResult.LltR));

            _diffRepo.Add(first);
            _diffRepo.Add(second);
            _diffRepo.Add(third);
        }



        // Prepares a repo with 7 Diff objects.
        private void PrepareRepoWithSevenItems()
        {
            PrepareRepoWithThreeItems();

            Diff fourth = new Diff(Guid.Empty, new StreamInput("Ema má maso."), new StreamInput("Ema má maso."), null);

            Diff fifth = new Diff(Guid.Empty, new StreamInput("Jedna, dva..."), new StreamInput("Nula, jedna, dva..."), new DiffOutput(DiffResult.LltR));

            Diff sixth = new Diff(Guid.Empty, null, new StreamInput("Příliš žluťoučký kůň úpěl ďábelské ódy."), null);

            Diff seventh = new Diff(Guid.Empty, null, null, null);

            _diffRepo.Add(fourth);
            _diffRepo.Add(fifth);
            _diffRepo.Add(sixth);
            _diffRepo.Add(seventh);
        }



    }



}
