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
using Rfe.DiffSvc.ApiTest.Helpers;



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

        //// Output - result of the "diff" operation:
        //// LeqR - "Left is equal to Right". (The strings are identical.)
        //// LgtR - "Left is greater than Right". (The left string is longer than the right one.)
        //// LltR - "Left is less than Right". (The left string is shorter than the right one.)
        //// LdiR - "Left is different from Right". (The strings have the same length, but they differ in some parts.)
        ////private string comparisonResult;
        //private string diffResult;

        //// Output - parts of the input that are different in "left" vs "right".
        ////private List<Difference> differentParts;
        //private ListOfDifferences differentParts;

        // Expected output - result of the "diff" operation.
        private string diffResultExpected;

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
            //this.hostPartOfUrl = "http://localhost:54485";
            this.hostPartOfUrl = "http://localhost:5000";

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
        /// A "positive" test.
        /// </summary>
        [Test]
        public async Task SimpleWorkflowWithSameInputsTest()
        {

            // Prepare data.
            this.diffID = Guid.Empty;

            // Input data.
            this.leftInput =
                ""
                + "I see skies of blue and clouds of white."
                + LF
                + "The bright blessed day, the dark sacred night."
                + LF
                + "I see And I think to myself what a wonderful world."
                ;
            this.rightInput = this.leftInput;

            // Expected output data.
            this.diffResultExpected = "LeqR";
            this.differentPartsExpected = new ListOfDifferences();

            // Get a diff ID.
            await GetIdTestBlock(true);

            // Use the returned diff ID (diffIDInResponse) for subsequent requests.
            this.diffID = this.diffIDInResponse;

            // Post left input data.
            await PostLeftInputTestBlock(true);

            // Post right input data.
            await PostRightInputTestBlock(true);

            // Get result.
            await GetOutputTestBlock(true);

        }



        /// <summary>
        /// Tests the entire workflow from getting an id till retrieving the diff of left vs right input data.
        /// This test should result in "LgtR" (the left text stream is longer than the right one).
        /// A "positive" test.
        /// </summary>
        [Test]
        public async Task SimpleWorkflowWithLeftInputLongerTest()
        {

            // Prepare data.
            this.diffID = Guid.Empty;

            // Input data.
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

            // Expected output data.
            this.diffResultExpected = "LgtR";
            this.differentPartsExpected = new ListOfDifferences();

            // Get a diff ID.
            await GetIdTestBlock(true);

            // Use the returned diff ID (diffIDInResponse) for subsequent requests.
            this.diffID = this.diffIDInResponse;

            // Post left input data.
            await PostLeftInputTestBlock(true);

            // Post right input data.
            await PostRightInputTestBlock(true);

            // Get result.
            await GetOutputTestBlock(true);

        }



        /// <summary>
        /// Tests the entire workflow from getting an id till retrieving the diff of left vs right input data.
        /// This test should result in "LltR" (the left text stream is shorter than the right one).
        /// A "positive" test.
        /// </summary>
        [Test]
        public async Task SimpleWorkflowWithLeftInputShorterTest()
        {

            // Prepare data.
            this.diffID = Guid.Empty;

            // Input data.
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

            // Expected output data.
            this.diffResultExpected = "LltR";
            this.differentPartsExpected = new ListOfDifferences();

            // Get a diff ID.
            await GetIdTestBlock(true);

            // Use the returned diff ID (diffIDInResponse) for subsequent requests.
            this.diffID = this.diffIDInResponse;

            // Post left input data.
            await PostLeftInputTestBlock(true);

            // Post right input data.
            await PostRightInputTestBlock(true);

            // Get result.
            await GetOutputTestBlock(true);

        }



        /// <summary>
        /// Tests the entire workflow from getting an id till retrieving the diff of left vs right input data.
        /// This test should result in "LdiR" (the left text stream has the same length as the right one, but they differ in some characters).
        /// A "positive" test.
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

            // Expected output data (diff result).
            this.diffResultExpected = "LdiR";

            // Expected output (diff sections).
            this.differentPartsExpected = new ListOfDifferences();
            this.differentPartsExpected.AddPart(replacement1Start, replacement1.Length);
            this.differentPartsExpected.AddPart(replacement2Start, replacement2.Length);

            // Start communication with the service.

            // Get a diff ID.
            await GetIdTestBlock(true);

            // Use the returned diff ID (diffIDInResponse) for subsequent requests.
            this.diffID = this.diffIDInResponse;

            // Post left input data.
            await PostLeftInputTestBlock(true);

            // Post right input data.
            await PostRightInputTestBlock(true);

            // Get result.
            await GetOutputTestBlock(true);

        }



        /// <summary>
        /// Tries to violate the usual workflow which should go from getting an id till retrieving the diff of left vs right input data.
        /// The test just receives a communication ID and asks for diff results immediately.
        /// A "negative" test.
        /// </summary>
        [Test]
        public async Task WorkflowViolationTest()
        {

            // Prepare data.
            this.diffID = Guid.Empty;

            // We don't need any particular input or expected output data.

            // Get a diff ID.
            await GetIdTestBlock(true);

            // Use the returned diff ID (diffIDInResponse) for subsequent requests.
            this.diffID = this.diffIDInResponse;

            // Post left input data.
            //await PostLeftInputTestBlock(true);

            // Post right input data.
            //await PostRightInputTestBlock(true);

            // Get result.
            //await GetOutputTestBlock(true);
            // Skip "positive" asserts.
            await GetOutputTestBlock(false);

            // Make sure the HTTP response status code matches with what we expected.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.Conflict, "diff/<ID>: 409 Conflict expected");

        }



        /// <summary>
        /// Tries to use a non-existing communication ID.
        /// The test "invents" a diff ID and sends it to the server via POSTing left input data.
        /// A "negative" test.
        /// </summary>
        [Test]
        public async Task InvalidDiffIDTest()
        {

            // Prepare data.
            this.diffID = Guid.Empty;

            // Input data.
            this.leftInput =
                ""
                + "I see skies of blue and clouds of white."
                + LF
                + "The bright blessed day, the dark sacred night."
                + LF
                + "I see And I think to myself what a wonderful world."
                ;
            this.rightInput = this.leftInput;

            // We don't need any expected output data.

            // Get a diff ID.
            //await GetIdTestBlock(true);

            // Use the returned diff ID (diffIDInResponse) for subsequent requests.
            //this.diffID = this.diffIDInResponse;
            // Use an arbitrary ID for the subsequent request.
            this.diffID = Guid.NewGuid();

            // Post left input data.
            //await PostLeftInputTestBlock(true);
            // Skip "positive" asserts.
            await PostLeftInputTestBlock(false);

            // Make sure the HTTP response status code matches with what we expected.
            Assert.That(this.statusCodeInResponse == HttpStatusCode.NotFound, "diff/<ID>/left: 404 Not Found expected");

            // Post right input data.
            //await PostRightInputTestBlock(true);

            // Get result.
            //await GetOutputTestBlock(true);

        }



        // **************************************************************************************************
        // **************************************************************************************************
        // **************************************************************************************************



        // Building tests out of blocks.
        // This is the "GET diff/get-id" block.
        // No particular assumptions here.
        private async Task GetIdTestBlock(bool checkPositiveResults)
        {
            // Get the ID of a new Diff.
            await CallGenerateIdAsync();

            if (checkPositiveResults)
            {
                // Make sure we received a diff ID.
                Assert.That(this.statusCodeInResponse == HttpStatusCode.OK, "diff/get-id: 200 OK expected");
                Assert.That(this.diffIDInResponse != Guid.Empty, "diff/get-id: non-empty diff ID expected");
            }
        }



        // Building tests out of blocks.
        // This is the "POST diff/<ID>/left" block.
        // Assumes: diffID and leftInput are set.
        private async Task PostLeftInputTestBlock(bool checkPositiveResults)
        {
            // Post the left input.
            await CallPostLeftInputAsync();

            if (checkPositiveResults)
            {
                // Make sure the left input has been stored.
                Assert.That(this.statusCodeInResponse == HttpStatusCode.Created, "diff/<ID>/left: 201 Created expected");
                Assert.That(this.diffIDInResponse == this.diffID, "diff/<ID>/left: the communication ID ({0}) expected", this.diffID);
                Assert.That(this.positionInReponse.Equals("left", StringComparison.InvariantCultureIgnoreCase), "diff/<ID>/left: operand position 'left' expected");
            }
        }



        // Building tests out of blocks.
        // This is the "POST diff/<ID>/right" block.
        // Assumes: diffID and rightInput are set.
        private async Task PostRightInputTestBlock(bool checkPositiveResults)
        {
            // Post the right input.
            await CallPostRightInputAsync();

            if (checkPositiveResults)
            {
                // Make sure the right input has been stored.
                Assert.That(this.statusCodeInResponse == HttpStatusCode.Created, "diff/<ID>/right: 201 Created expected");
                Assert.That(this.diffIDInResponse == this.diffID, "diff/<ID>/right: the communication ID ({0}) expected", this.diffID);
                Assert.That(this.positionInReponse.Equals("right", StringComparison.InvariantCultureIgnoreCase), "diff/<ID>/right: operand position 'right' expected");
            }
        }



        // Building tests out of blocks.
        // This is the "GET diff/<ID>" block.
        // Assumes: diffID is set.
        // Assumes: diffResultExpected and differentPartsExpected are set.
        private async Task GetOutputTestBlock(bool checkPositiveResults)
        {
            // Get the comparison result.
            await CallGetComparisonResultAsync();

            if (checkPositiveResults)
            {
                // Make sure the result matches with what we expected.
                Assert.That(this.statusCodeInResponse == HttpStatusCode.OK, "diff/<ID>: 200 OK expected");
                Assert.That(this.diffResultInResponse == this.diffResultExpected, "diff/<ID>: {0} ({1}) expected", this.diffResultExpected, MapDiffResultToHumanReadableString(this.diffResultExpected));;
                //if (this.differentPartsExpected.IsMissing || this.differentPartsExpected.IsEmpty)
                if (this.differentPartsExpected.IsMissingOrEmpty)
                {
                    Assert.That(this.differentPartsInResponse.IsMissingOrEmpty, "diff/<ID>: empty diffSections expected");
                }
                else
                {
                    Assert.That(ListOfDifferences.AreEqualAfterNormalization(this.differentPartsExpected, this.differentPartsInResponse), "diff/<ID>: particular diffSections ({0}) expected", this.differentPartsExpected);
                }
            }
        }



        // Gets a human readable string for a given diff result.
        private string MapDiffResultToHumanReadableString(string diffResult)
        {
            switch (diffResult)
            {
                case "LeqR":
                    return "input strings are equal";

                case "LgtR":
                    return "left input string longer than right";

                case "LltR":
                    return "left input string shorter than right";

                case "LdiR":
                    return "left and right input strings have the same length and yet differ in some parts";
            }
            throw new NotSupportedException($"This diff result is not supported here: {diffResult}");
        }



        // **************************************************************************************************
        // **************************************************************************************************
        // **************************************************************************************************



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
            
            // OLD: Send plain JSON over the net.
            // // HTTP content parts.
            // (MediaTypeHeaderValue mediaType, JsonSerializerOptions options) = PrepareHttpContentPartsForJson();
            // // Prepare the HTTP content itself.
            // HttpContent content = JsonContent.Create<InputWrapper>(inputWrapper, mediaType, options);
            //HttpContent content = PrepareHttpContentForJson<InputWrapper>(inputWrapper);
            
            // NEW: Encode the JSON data using Base64 encoding.
            // (MediaTypeHeaderValue mediaType, JsonSerializerOptions options) = PrepareHttpContentPartsForJsonInBase64();
            // string inputJson = Serialize<InputWrapper>(inputWrapper);
            // string inputBase64 = EncodingHelper.Base64Encode(inputJson);
            // HttpContent content = JsonContent.Create<string>(inputBase64, mediaType, options);
            HttpContent content = PrepareHttpContentForJsonInBase64<InputWrapper>(inputWrapper);

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



        // Helper to convert an object to a JSON string.
        private string Serialize<TInput>(TInput data)
        {
            // Prepare options.
            //JsonSerializerDefaults jsonSerializerDefaults = new JsonSerializerDefaults();
            JsonSerializerOptions options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            
            // Get the result.
            return Serialize<TInput>(data, options);
        }
        private string Serialize<TInput>(TInput data, JsonSerializerOptions options)
        {
            // Use options from the parameter.

            // Serialize the given object.
            string jsonData = JsonSerializer.Serialize<TInput>(data, options);
            
            // Return the result.
            return jsonData;
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
            //JsonSerializerDefaults jsonSerializerDefaults = new JsonSerializerDefaults();
            //JsonSerializerOptions options = new JsonSerializerOptions(jsonSerializerDefaults);
            JsonSerializerOptions options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
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
            //JsonSerializerDefaults jsonSerializerDefaults = new JsonSerializerDefaults();
            //JsonSerializerOptions options = new JsonSerializerOptions(jsonSerializerDefaults);
            JsonSerializerOptions options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            // Return the parts.
            return (mediaTypeHeaderValue, options);
        }


        // Helper for creating a JSON body of an HTTP request.
        // Results:
        // MediaTypeHeaderValue - Content-Type related info
        // JsonSerializerOptions - Serializing options for JSON
        // string - Value for Content-Encoding HTTP header
        private (MediaTypeHeaderValue, JsonSerializerOptions, string) PrepareHttpContentPartsForJsonInBase64()
        {
            // Prepare media type.
            //MediaTypeHeaderValue mediaTypeHeaderValue = new MediaTypeHeaderValue("text/plain; charset=iso-8859-5");
            MediaTypeHeaderValue mediaTypeHeaderValue = new MediaTypeHeaderValue("application/json");
            //MediaTypeHeaderValue mediaTypeHeaderValue = new MediaTypeHeaderValue("application/custom");
            
            // Prepare options.
            //JsonSerializerDefaults jsonSerializerDefaults = new JsonSerializerDefaults();
            //JsonSerializerOptions options = new JsonSerializerOptions(jsonSerializerDefaults);
            JsonSerializerOptions options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            
            // Prepare a value for Content-Encoding.
            //string contentEncodingValue = "Content-Encoding: base64";
            // Just the value, do not include the header name.
            //string contentEncodingValue = null;
            string contentEncodingValue = "base64";
            
            // Return the parts.
            return (mediaTypeHeaderValue, options, contentEncodingValue);
        }



        // Helps to prepare HTTP content for "plain JSON request bodies".
        // The "data" parameter is the data to put into the request body.
        private HttpContent PrepareHttpContentForJson<TInput>(TInput data)
        {
            // HTTP content parts.
            (MediaTypeHeaderValue mediaType, JsonSerializerOptions options) = PrepareHttpContentPartsForJson();
            
            // Prepare the HTTP content itself.
            HttpContent content = JsonContent.Create<TInput>(data, mediaType, options);
            
            // Return the result.
            return content;
        }



        // Helps to prepare HTTP content for "request bodies where JSON data is encoded as a Base64 string".
        // The "data" parameter is the data to put into the request body.
        private HttpContent PrepareHttpContentForJsonInBase64<TInput>(TInput data)
        {
            // HTTP content parts.
            //(MediaTypeHeaderValue mediaType, JsonSerializerOptions options) = PrepareHttpContentPartsForJson();
            (MediaTypeHeaderValue mediaType, JsonSerializerOptions options, string contentEncodingValue) = PrepareHttpContentPartsForJsonInBase64();
            
            // Prepare the HTTP content itself.
            //HttpContent content = JsonContent.Create<TInput>(data, mediaType, options);
            //string inputJson = Serialize<TInput>(data);
            string inputJson = Serialize<TInput>(data, options);
            string inputBase64 = EncodingHelper.Base64Encode(inputJson);
            //HttpContent content = JsonContent.Create<string>(inputBase64, mediaType, options);
            HttpContent content = new StringContent(inputBase64, Encoding.Default, mediaType);
            
            // Add Content-Encoding if needed.
            if ( ! string.IsNullOrEmpty(contentEncodingValue) )
            {
                content.Headers.ContentEncoding.Add(contentEncodingValue);
            }
            
            // Return the result.
            return content;
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
