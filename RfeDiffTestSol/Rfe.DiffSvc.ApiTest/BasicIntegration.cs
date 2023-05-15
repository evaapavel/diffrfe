using NUnit.Framework;
using System;

namespace Rfe.DiffSvc.ApiTest
{



    /// <summary>
    /// Defines a couple of methods to perform
    /// the integration testing on endpoints
    /// offered in the Diff RFE ASP.NET Core Web API.
    /// </summary>
    [TestFixture]
    public class BasicIntegration
    {



        // ID of a Diff object (an object used on the web API server side) to work with during tests.
        private Guid diffID;

        // Input data "on the left".
        private string leftInput;

        // Input data "on the right".
        private string rightInput;

        // Output - result of the "diff" operation:
        // LeqR - "Left is equal to Right". (The strings are identical.)
        // LgtR - "Left is greater than Right". (The left string is longer than the right one.)
        // LltR - "Left is less than Right". (The left string is shorter than the right one.)
        // LdiR - "Left is different from Right". (The strings have the same length, but they differ in some parts.)
        private string comparisonResult;



        /// <summary>
        /// Set up method. Called before each particular test.
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }




        /// <summary>
        /// Tests the entire workflow from getting an id till retrieving the diff of left vs right input data.
        /// </summary>
        [Test]
        public void SimpleWorkflowTest()
        {

            // Prepare data.


            // Get the ID of a new Diff.

            // Make sure we've gotten one.

            // Post the left input.

            // Make sure the left input has been stored.

            // Post the right input.

            // Make sure the right input has been stored.

            // Get the comparison result.

            // Make sure the result matches what we expected.

        }



    }



}
