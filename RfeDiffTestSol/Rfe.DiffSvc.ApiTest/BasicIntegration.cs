using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;

using System.Text.Json;
using System.Threading.Tasks;
using Rfe.DiffSvc.ApiTest.BusinessObjects;



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



        // Used to indicate that a HTTP status code variable has not been initialized yet.
        private const HttpStatusCode EmptyStatusCode = (HttpStatusCode) 0;

        // Linux line ending sequence.
        private const string LF = "\n";



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
        //private string comparisonResult;
        private string diffResult;

        // Output - parts of the input that are different in "left" vs "right".
        //private List<Difference> differentParts;
        private ListOfDifferences differentParts;

        // Expected output - parts of the input that differ.
        private ListOfDifferences differentPartsExpected;



        // Status code taken from the HTTP response.
        //private int statusCodeInResponse;
        private HttpStatusCode statusCodeInResponse;

        // Diff ID stated in the HTTP response.
        private Guid diffIDInResponse;

        // String representation of the input position. Data taken from the HTTP response.
        private string positionInReponse;

        // Diff result taken from the HTTP response.
        private string diffResultInResponse;

        // Diff parts sections taken from the HTTP response.
        private ListOfDifferences differentPartsInResponse;



        // Used to make HTTP requests and get HTTP responses from the REST API to test.
        private HttpClient httpClient;

        // Host with API to connect to.
        private string hostPartOfUrl;

        // Common part of paths.
        private string commonRoutePrefix;



        /// <summary>
        /// Set up method. Called before each particular test.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // TODO: Put the host URL part to configuration.
            // For the time being, we use it "hard-coded".
            this.hostPartOfUrl = "http://localhost:54485";

            // TODO: Put the common URL part to configuration.
            // For the time being, we use it "hard-coded".
            //this.commonRoutePrefix = "/v1/diff";
            this.commonRoutePrefix = "v1/diff";

            // Prepare a HTTP client.
            this.httpClient = new HttpClient();
            //this.httpClient.Open();
        }



        /// <summary>
        /// Tear down method. Called after each particular test.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            this.httpClient.Dispose();
        }



        //[Test]
        //public void SimpleWorkflowTest()
        /// <summary>
        /// Tests the entire workflow from getting an id till retrieving the diff of left vs right input data.
        /// This test should result in "LeqR" (the text streams are equal).
        /// </summary>
        [Test]
        public async Task SimpleWorkflowWithSameInputsTest()
        {

            // Prepare data.
            this.diffID = Guid.Empty;
            
            this.leftInput =
                ""
                + "I see skies of blue and clouds of white."
                + LF
                + "The bright blessed day, the dark sacred night."
                + LF
                + "I see And I think to myself what a wonderful world."
                ;

            this.rightInput = this.leftInput;

            // Get the ID of a new Diff.
            //CallGenerateId();
            //CallGenerateIdAsync();
            await CallGenerateIdAsync();

            // Make sure we received a diff ID.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.OK, "diff/get-id: 200 OK expected");
            Assert.That(this.diffIDInResponse != Guid.Empty, "diff/get-id: non-empty diff ID expected");

            // Use the returned diff ID (diffIDInResponse) for subsequent requests.
            this.diffID = this.diffIDInResponse;

            // Post the left input.
            //CallPostLeftInput();
            await CallPostLeftInputAsync();

            // Make sure the left input has been stored.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.Created, "diff/<ID>/left: 201 Created expected");
            Assert.That(this.diffIDInResponse == this.diffID, "diff/<ID>/left: the communication ID ({0}) expected", this.diffID);
            Assert.That(this.positionInReponse.Equals("left", StringComparison.InvariantCultureIgnoreCase), "diff/<ID>/left: operand position 'left' expected");

            // Post the right input.
            //CallPostRightInput();
            await CallPostRightInputAsync();

            // Make sure the right input has been stored.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.Created, "diff/<ID>/right: 201 Created expected");
            Assert.That(this.diffIDInResponse == this.diffID, "diff/<ID>/right: the communication ID ({0}) expected", this.diffID);
            Assert.That(this.positionInReponse.Equals("right", StringComparison.InvariantCultureIgnoreCase), "diff/<ID>/right: operand position 'right' expected");

            // Get the comparison result.
            //CallGetComparisonResult();
            await CallGetComparisonResultAsync();

            // Make sure the result matches with what we expected.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.OK, "diff/<ID>: 200 OK expected");
            Assert.That(this.diffResultInResponse == "LeqR", "diff/<ID>: LeqR (input strings are equal) expected");
            Assert.That(this.differentPartsInResponse.IsMissing || this.differentPartsInResponse.IsEmpty, "diff/<ID>: empty diffSections expected");

        }



        /// <summary>
        /// Tests the entire workflow from getting an id till retrieving the diff of left vs right input data.
        /// This test should result in "LgtR" (the left text stream is longer than the right one).
        /// </summary>
        [Test]
        public async Task SimpleWorkflowWithLeftInputLongerTest()
        {

            // Prepare data.
            this.diffID = Guid.Empty;

            this.leftInput =
                ""
                + "I see skies of blue and clouds of white."
                + LF
                + "The bright blessed day, the dark sacred night."
                + LF
                + "I see And I think to myself what a wonderful world."
                ;

            this.rightInput =
                ""
                + "I see skies of blue and clouds of white."
                + LF
                //+ "The bright blessed day, the dark sacred night."
                //+ LF
                + "I see And I think to myself what a wonderful world."
                ;

            // Get the ID of a new Diff.
            await CallGenerateIdAsync();

            // Make sure we received a diff ID.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.OK, "diff/get-id: 200 OK expected");
            Assert.That(this.diffIDInResponse != Guid.Empty, "diff/get-id: non-empty diff ID expected");

            // Use the returned diff ID (diffIDInResponse) for subsequent requests.
            this.diffID = this.diffIDInResponse;

            // Post the left input.
            await CallPostLeftInputAsync();

            // Make sure the left input has been stored.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.Created, "diff/<ID>/left: 201 Created expected");
            Assert.That(this.diffIDInResponse == this.diffID, "diff/<ID>/left: the communication ID ({0}) expected", this.diffID);
            Assert.That(this.positionInReponse.Equals("left", StringComparison.InvariantCultureIgnoreCase), "diff/<ID>/left: operand position 'left' expected");

            // Post the right input.
            await CallPostRightInputAsync();

            // Make sure the right input has been stored.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.Created, "diff/<ID>/right: 201 Created expected");
            Assert.That(this.diffIDInResponse == this.diffID, "diff/<ID>/right: the communication ID ({0}) expected", this.diffID);
            Assert.That(this.positionInReponse.Equals("right", StringComparison.InvariantCultureIgnoreCase), "diff/<ID>/right: operand position 'right' expected");

            // Get the comparison result.
            await CallGetComparisonResultAsync();

            // Make sure the result matches with what we expected.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.OK, "diff/<ID>: 200 OK expected");
            Assert.That(this.diffResultInResponse == "LgtR", "diff/<ID>: LgtR (left input string longer than right) expected");
            Assert.That(this.differentPartsInResponse.IsMissing || this.differentPartsInResponse.IsEmpty, "diff/<ID>: empty diffSections expected");

        }



        /// <summary>
        /// Tests the entire workflow from getting an id till retrieving the diff of left vs right input data.
        /// This test should result in "LltR" (the left text stream is shorter than the right one).
        /// </summary>
        [Test]
        public async Task SimpleWorkflowWithLeftInputShorterTest()
        {

            // Prepare data.
            this.diffID = Guid.Empty;

            this.leftInput =
                ""
                + "I see skies of blue and clouds of white."
                ;

            this.rightInput =
                ""
                + "I see skies of blue and clouds of white."
                + LF
                + "The bright blessed day, the dark sacred night."
                + LF
                + "I see And I think to myself what a wonderful world."
                ;

            // Get the ID of a new Diff.
            await CallGenerateIdAsync();

            // Make sure we received a diff ID.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.OK, "diff/get-id: 200 OK expected");
            Assert.That(this.diffIDInResponse != Guid.Empty, "diff/get-id: non-empty diff ID expected");

            // Use the returned diff ID (diffIDInResponse) for subsequent requests.
            this.diffID = this.diffIDInResponse;

            // Post the left input.
            await CallPostLeftInputAsync();

            // Make sure the left input has been stored.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.Created, "diff/<ID>/left: 201 Created expected");
            Assert.That(this.diffIDInResponse == this.diffID, "diff/<ID>/left: the communication ID ({0}) expected", this.diffID);
            Assert.That(this.positionInReponse.Equals("left", StringComparison.InvariantCultureIgnoreCase), "diff/<ID>/left: operand position 'left' expected");

            // Post the right input.
            await CallPostRightInputAsync();

            // Make sure the right input has been stored.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.Created, "diff/<ID>/right: 201 Created expected");
            Assert.That(this.diffIDInResponse == this.diffID, "diff/<ID>/right: the communication ID ({0}) expected", this.diffID);
            Assert.That(this.positionInReponse.Equals("right", StringComparison.InvariantCultureIgnoreCase), "diff/<ID>/right: operand position 'right' expected");

            // Get the comparison result.
            await CallGetComparisonResultAsync();

            // Make sure the result matches with what we expected.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.OK, "diff/<ID>: 200 OK expected");
            Assert.That(this.diffResultInResponse == "LltR", "diff/<ID>: LltR (left input string shorter than right) expected");
            Assert.That(this.differentPartsInResponse.IsMissing || this.differentPartsInResponse.IsEmpty, "diff/<ID>: empty diffSections expected");

        }



        /// <summary>
        /// Tests the entire workflow from getting an id till retrieving the diff of left vs right input data.
        /// This test should result in "LdiR" (the left text stream has the same length as the right one, but they differ in some characters).
        /// </summary>
        [Test]
        public async Task SimpleWorkflowWithDifferentInputsOfSameLengthTest()
        {

            // Prepare data.

            // Initialize the diff ID.
            this.diffID = Guid.Empty;

            // Common input string at the beginning.
            StringBuilder sb = new StringBuilder(
                ""
                + "I see skies of blue and clouds of white."
                + LF
                + "The bright blessed day, the dark sacred night."
                + LF
                + "I see And I think to myself what a wonderful world."
                );

            // Left input.
            this.leftInput = sb.ToString();

            // Replace some chars in the common input in order for the left/right to diverge:

            // Replace "see" with "git".
            string replacement1 = "git";
            int replacement1Start = 2;
            ReplaceInputPart(sb, replacement1Start, replacement1);

            // Replace 2nd line with something completely different but with the same length.
            int firstLineLength = ("I see skies of blue and clouds of white." + LF).Length;
            int replacement2Start = firstLineLength;
            //ReplaceInputPart(sb, firstLineLength, "The dark sacred night, the bright blessed day.");
            //ReplaceInputPart(sb, firstLineLength, "Our_dark_sacr3d_night,_the_bright_blessed_day!");
            string replacement2 = "Our_dark_sacr3d_night,_the_bright_blessed_day!";
            ReplaceInputPart(sb, replacement2Start, replacement2);

            // Right input.
            this.rightInput = sb.ToString();

            // Expected output (diff sections).
            this.differentPartsExpected = new ListOfDifferences();
            this.differentPartsExpected.AddPart(replacement1Start, replacement1.Length);
            this.differentPartsExpected.AddPart(replacement2Start, replacement2.Length);

            // Start communication with the service.

            // Get the ID of a new Diff.
            await CallGenerateIdAsync();

            // Make sure we received a diff ID.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.OK, "diff/get-id: 200 OK expected");
            Assert.That(this.diffIDInResponse != Guid.Empty, "diff/get-id: non-empty diff ID expected");

            // Use the returned diff ID (diffIDInResponse) for subsequent requests.
            this.diffID = this.diffIDInResponse;

            // Post the left input.
            await CallPostLeftInputAsync();

            // Make sure the left input has been stored.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.Created, "diff/<ID>/left: 201 Created expected");
            Assert.That(this.diffIDInResponse == this.diffID, "diff/<ID>/left: the communication ID ({0}) expected", this.diffID);
            Assert.That(this.positionInReponse.Equals("left", StringComparison.InvariantCultureIgnoreCase), "diff/<ID>/left: operand position 'left' expected");

            // Post the right input.
            await CallPostRightInputAsync();

            // Make sure the right input has been stored.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.Created, "diff/<ID>/right: 201 Created expected");
            Assert.That(this.diffIDInResponse == this.diffID, "diff/<ID>/right: the communication ID ({0}) expected", this.diffID);
            Assert.That(this.positionInReponse.Equals("right", StringComparison.InvariantCultureIgnoreCase), "diff/<ID>/right: operand position 'right' expected");

            // Get the comparison result.
            await CallGetComparisonResultAsync();

            // Make sure the result matches with what we expected.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.OK, "diff/<ID>: 200 OK expected");
            Assert.That(this.diffResultInResponse == "LdiR", "diff/<ID>: LdiR (left and right input strings have the same length and yet differ in some parts) expected");
            Assert.That(ListOfDifferences.AreEqualAfterNormalization(this.differentPartsExpected, this.differentPartsInResponse), "diff/<ID>: particular diffSections ({0}) expected", this.differentPartsExpected);

        }



        /// <summary>
        /// Tries to violate the usual workflow which should go from getting an id till retrieving the diff of left vs right input data.
        /// The test just receives a communication ID and asks for diff results immediately.
        /// </summary>
        [Test]
        public async Task WorkflowViolationTest()
        {

            // Prepare data.
            this.diffID = Guid.Empty;

            this.leftInput =
                ""
                + "I see skies of blue and clouds of white."
                + LF
                + "The bright blessed day, the dark sacred night."
                + LF
                + "I see And I think to myself what a wonderful world."
                ;

            this.rightInput = this.leftInput;

            // Get the ID of a new Diff.
            await CallGenerateIdAsync();

            // Make sure we received a diff ID.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.OK, "diff/get-id: 200 OK expected");
            Assert.That(this.diffIDInResponse != Guid.Empty, "diff/get-id: non-empty diff ID expected");

            // Use the returned diff ID (diffIDInResponse) for subsequent requests.
            this.diffID = this.diffIDInResponse;

            //// Post the left input.
            //await CallPostLeftInputAsync();

            //// Make sure the left input has been stored.
            //Assert.That(this.statusCodeInResponse == HttpStatusCode.Created, "diff/<ID>/left: 201 Created expected");
            //Assert.That(this.diffIDInResponse == this.diffID, "diff/<ID>/left: the communication ID ({0}) expected", this.diffID);
            //Assert.That(this.positionInReponse.Equals("left", StringComparison.InvariantCultureIgnoreCase), "diff/<ID>/left: operand position 'left' expected");

            //// Post the right input.
            //await CallPostRightInputAsync();

            //// Make sure the right input has been stored.
            //Assert.That(this.statusCodeInResponse == HttpStatusCode.Created, "diff/<ID>/right: 201 Created expected");
            //Assert.That(this.diffIDInResponse == this.diffID, "diff/<ID>/right: the communication ID ({0}) expected", this.diffID);
            //Assert.That(this.positionInReponse.Equals("right", StringComparison.InvariantCultureIgnoreCase), "diff/<ID>/right: operand position 'right' expected");

            // Get the comparison result.
            await CallGetComparisonResultAsync();

            // Make sure the result matches with what we expected.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.Conflict, "diff/<ID>: 409 Conflict expected");
            //Assert.That(this.diffResultInResponse == "LeqR", "diff/<ID>: LeqR (input strings are equal) expected");
            //Assert.That(this.differentPartsInResponse.IsMissing || this.differentPartsInResponse.IsEmpty, "diff/<ID>: empty diffSections expected");

        }



        /// <summary>
        /// Tries to use a non-existing communication ID.
        /// The tests "invents" a diff ID and sends it to the server via POSTing left input data.
        /// </summary>
        [Test]
        public async Task InvalidDiffIDTest()
        {

            // Prepare data.
            this.diffID = Guid.Empty;

            this.leftInput =
                ""
                + "I see skies of blue and clouds of white."
                + LF
                + "The bright blessed day, the dark sacred night."
                + LF
                + "I see And I think to myself what a wonderful world."
                ;

            this.rightInput = this.leftInput;

            //// Get the ID of a new Diff.
            //await CallGenerateIdAsync();

            //// Make sure we received a diff ID.
            //Assert.That(this.statusCodeInResponse == HttpStatusCode.OK, "diff/get-id: 200 OK expected");
            //Assert.That(this.diffIDInResponse != Guid.Empty, "diff/get-id: non-empty diff ID expected");

            //// Use the returned diff ID (diffIDInResponse) for subsequent requests.
            //this.diffID = this.diffIDInResponse;
            // Use an arbitrary ID for the subsequent request.
            this.diffID = Guid.NewGuid();

            // Post the left input.
            await CallPostLeftInputAsync();

            // Make sure the left input has been stored.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.NotFound, "diff/<ID>/left: 404 Not Found expected");
            //Assert.That(this.diffIDInResponse == this.diffID, "diff/<ID>/left: the communication ID ({0}) expected", this.diffID);
            //Assert.That(this.positionInReponse.Equals("left", StringComparison.InvariantCultureIgnoreCase), "diff/<ID>/left: operand position 'left' expected");

            //// Post the right input.
            //await CallPostRightInputAsync();

            //// Make sure the right input has been stored.
            //Assert.That(this.statusCodeInResponse == HttpStatusCode.Created, "diff/<ID>/right: 201 Created expected");
            //Assert.That(this.diffIDInResponse == this.diffID, "diff/<ID>/right: the communication ID ({0}) expected", this.diffID);
            //Assert.That(this.positionInReponse.Equals("right", StringComparison.InvariantCultureIgnoreCase), "diff/<ID>/right: operand position 'right' expected");

            //// Get the comparison result.
            //await CallGetComparisonResultAsync();

            //// Make sure the result matches with what we expected.
            //Assert.That(this.statusCodeInResponse == HttpStatusCode.OK, "diff/<ID>: 200 OK expected");
            //Assert.That(this.diffResultInResponse == "LeqR", "diff/<ID>: LeqR (input strings are equal) expected");
            //Assert.That(this.differentPartsInResponse.IsMissing || this.differentPartsInResponse.IsEmpty, "diff/<ID>: empty diffSections expected");

        }



        // GET <host>/v1/diff/get-id
        // Generate a "communication" id for subsequent requests.
        //private void CallGenerateId()
        //private async void CallGenerateIdAsync()
        private async Task CallGenerateIdAsync()
        {

            // Clear output values.
            //this.statusCodeInResponse = (HttpStatusCode) 0;
            this.statusCodeInResponse = EmptyStatusCode;
            this.diffIDInResponse = Guid.Empty;

            // Prepare a path for the request.
            string path = $"{this.hostPartOfUrl}/{this.commonRoutePrefix}/get-id";

            // Send the request.
            HttpResponseMessage response = await this.httpClient.GetAsync(path);

            // Wait for the response.
            string jsonData = await response.Content.ReadAsStringAsync();

            // Deserialize the response.
            //JsonSerializerDefaults jsonSerializerDefaults = new JsonSerializerDefaults();
            //JsonSerializerOptions options = new JsonSerializerOptions(jsonSerializerDefaults);
            //// Be tolerant to case differences in property names.
            //options.PropertyNameCaseInsensitive = true;
            ////dynamic dataObj = JsonSerializer.Deserialize(jsonData);
            //IdWrapper wrapper = JsonSerializer.Deserialize<IdWrapper>(jsonData, options);
            IdWrapper wrapper = Deserialize<IdWrapper>(jsonData);

            // Store results in member field(s) if necessary.
            this.statusCodeInResponse = response.StatusCode;
            //this.diffID = new Guid(wrapper.Id);
            this.diffIDInResponse = new Guid(wrapper.Id);

        }



        // POST <host>/v1/diff/<ID>/left
        // Place 1st text stream for "diff" (left input).
        //private void CallPostLeftInput()
        private async Task CallPostLeftInputAsync()
        {
            await CallPostInputAsync(this.leftInput, "left");
        }



        // POST <host>/v1/diff/<ID>/right
        // Place 2nd text stream for "diff" (right input).
        //private void CallPostRightInput()
        private async Task CallPostRightInputAsync()
        {
            await CallPostInputAsync(this.rightInput, "right");
        }



        // POST <host>/v1/diff/<ID>/left
        // POST <host>/v1/diff/<ID>/right
        // The two endpoints are almost the same. Let's parameterize the POST.
        // The parameter inputData is the data to put in the request body.
        // The parameter operandPosition is either "left" or "right", depending on the input ordinal number of the "diff" operation (by convention: 1st is left, 2nd is right).
        private async Task CallPostInputAsync(string inputData, string operandPosition)
        {

            // Clear output values.
            //this.statusCodeInResponse = (HttpStatusCode) 0;
            this.statusCodeInResponse = EmptyStatusCode;
            this.diffIDInResponse = Guid.Empty;
            this.positionInReponse = null;

            // Prepare a path for the request.
            //string path = $"{this.hostPartOfUrl}/{this.commonRoutePrefix}/{this.diffID}/left";
            string path = $"{this.hostPartOfUrl}/{this.commonRoutePrefix}/{this.diffID}/{operandPosition}";

            // Prepare the body of the request.
            // Wrap input data first.
            //InputWrapper inputWrapper = new InputWrapper { Input = this.leftInput };
            InputWrapper inputWrapper = new InputWrapper { Input = inputData };
            // Build the HttpContent.
            //HttpContent content = new HttpContent();
            //HttpContent content = new JsonContent();
            //MediaTypeHeaderValue mediaTypeHeaderValue = new MediaTypeHeaderValue("text/plain; charset=iso-8859-5");
            //MediaTypeHeaderValue mediaTypeHeaderValue = new MediaTypeHeaderValue("application/json");
            //JsonSerializerDefaults jsonSerializerDefaults = new JsonSerializerDefaults();
            //JsonSerializerOptions options = new JsonSerializerOptions(jsonSerializerDefaults);
            //options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            //HttpContent content = JsonContent.Create<InputWrapper>(inputWrapper, mediaTypeHeaderValue, options);
            // HTTP content parts.
            (MediaTypeHeaderValue mediaType, JsonSerializerOptions options) = PrepareHttpContentPartsForJson();
            // Prepare the HTTP content itself.
            HttpContent content = JsonContent.Create<InputWrapper>(inputWrapper, mediaType, options);

            // Send the request.
            //HttpResponseMessage response = await this.httpClient.GetAsync(path);
            HttpResponseMessage response = await this.httpClient.PostAsync(path, content);

            // Wait for the response.
            string jsonData = await response.Content.ReadAsStringAsync();

            // Deserialize the response.
            IdAndPositionWrapper wrapper = Deserialize<IdAndPositionWrapper>(jsonData);

            // Store results in member field(s) if necessary.
            this.statusCodeInResponse = response.StatusCode;
            this.diffIDInResponse = new Guid(wrapper.Id);
            this.positionInReponse = wrapper.Position;

        }



        // GET <host>/v1/diff/<ID>
        // Get the "diff" output.
        //private void CallGetComparisonResult()
        private async Task CallGetComparisonResultAsync()
        {

            // Clear output values.
            this.statusCodeInResponse = EmptyStatusCode;
            this.diffResultInResponse = null;
            this.differentPartsInResponse = null;

            // Prepare a path for the request.
            string path = $"{this.hostPartOfUrl}/{this.commonRoutePrefix}/{this.diffID}";

            // Send the request.
            HttpResponseMessage response = await this.httpClient.GetAsync(path);

            // Wait for the response.
            string jsonData = await response.Content.ReadAsStringAsync();

            // Deserialize the response.
            ResultAndDiffSectionsWrapper wrapper = Deserialize<ResultAndDiffSectionsWrapper>(jsonData);

            // Store results in member field(s) if necessary.
            this.statusCodeInResponse = response.StatusCode;
            this.diffResultInResponse = wrapper.Result;
            this.differentPartsInResponse = new ListOfDifferences(wrapper.DiffSections);

        }



        // Helper to deserialize the body of a HTTP response.
        //private TResult Deserialize<out TResult>(string jsonData)
        private TResult Deserialize<TResult>(string jsonData)
        {
            // TEMPLATE:
            //JsonSerializerDefaults jsonSerializerDefaults = new JsonSerializerDefaults();
            //JsonSerializerOptions options = new JsonSerializerOptions(jsonSerializerDefaults);
            //// Be tolerant to case differences in property names.
            //options.PropertyNameCaseInsensitive = true;
            ////dynamic dataObj = JsonSerializer.Deserialize(jsonData);
            //IdWrapper wrapper = JsonSerializer.Deserialize<IdWrapper>(jsonData, options);

            // DO SOMETHING:
            // Prepare options for the deserializer.
            JsonSerializerDefaults jsonSerializerDefaults = new JsonSerializerDefaults();
            JsonSerializerOptions options = new JsonSerializerOptions(jsonSerializerDefaults);
            // Be tolerant to case differences in property names.
            options.PropertyNameCaseInsensitive = true;
            TResult result = JsonSerializer.Deserialize<TResult>(jsonData, options);

            // Return the result.
            return result;
        }


        // Helper for creating a JSON body of an HTTP request.
        private (MediaTypeHeaderValue, JsonSerializerOptions) PrepareHttpContentPartsForJson()
        {
            // Prepare media type and options.
            //MediaTypeHeaderValue mediaTypeHeaderValue = new MediaTypeHeaderValue("text/plain; charset=iso-8859-5");
            MediaTypeHeaderValue mediaTypeHeaderValue = new MediaTypeHeaderValue("application/json");
            JsonSerializerDefaults jsonSerializerDefaults = new JsonSerializerDefaults();
            JsonSerializerOptions options = new JsonSerializerOptions(jsonSerializerDefaults);
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            // Return the parts.
            return (mediaTypeHeaderValue, options);
        }



        // Helper to change parts of a (huge) string.
        // Assume we won't get outside of the index boundaries.
        private void ReplaceInputPart(StringBuilder stringBuilder, int startIndex, string replacingText)
        {
            for (int i = 0; i < replacingText.Length; i++)
            {
                stringBuilder[startIndex + i] = replacingText[i];
            }
        }



    }



}
