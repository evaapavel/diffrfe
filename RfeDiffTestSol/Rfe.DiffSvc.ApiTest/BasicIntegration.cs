using NUnit.Framework;
using System;
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



        // Diff ID stated in the HTTP response.
        private Guid diffIDInResponse;

        // String representation of the input position. Data taken from the HTTP response.
        private string positionInReponse;



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
        public async Task SimpleWorkflowTest()
        {

            // Prepare data.
            this.diffID = Guid.Empty;
            this.leftInput =
                ""
                + "I see skies of blue and clouds of white."
                + "The bright blessed day, the dark sacred night."
                + "I see And I think to myself what a wonderful world."
                ;

            this.rightInput = this.leftInput;

            // Get the ID of a new Diff.
            //CallGenerateId();
            //CallGenerateIdAsync();
            await CallGenerateIdAsync();

            // Make sure we've gotten one.

            // Post the left input.
            //CallPostLeftInput();
            await CallPostLeftInputAsync();

            // Make sure the left input has been stored.

            // Post the right input.
            CallPostRightInput();

            // Make sure the right input has been stored.

            // Get the comparison result.
            CallGetComparisonResult();

            // Make sure the result matches what we expected.

        }



        // GET <host>/v1/diff/get-id
        // Generate a "communication" id for subsequent requests.
        //private void CallGenerateId()
        //private async void CallGenerateIdAsync()
        private async Task CallGenerateIdAsync()
        {

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
            this.diffID = new Guid(wrapper.Id);

        }



        // POST <host>/v1/diff/<ID>/left
        // Place 1st text stream for "diff" (left input).
        //private void CallPostLeftInput()
        private async Task CallPostLeftInputAsync()
        {

            // Prepare a path for the request.
            string path = $"{this.hostPartOfUrl}/{this.commonRoutePrefix}/{this.diffID}/left";

            // Prepare the body of the request.
            // Wrap input data first.
            InputWrapper inputWrapper = new InputWrapper { Input = this.leftInput };
            // Build the HttpContent.
            //HttpContent content = new HttpContent();
            //HttpContent content = new JsonContent();
            //MediaTypeHeaderValue mediaTypeHeaderValue = new MediaTypeHeaderValue("text/plain; charset=iso-8859-5");
            MediaTypeHeaderValue mediaTypeHeaderValue = new MediaTypeHeaderValue("application/json");
            JsonSerializerDefaults jsonSerializerDefaults = new JsonSerializerDefaults();
            JsonSerializerOptions options = new JsonSerializerOptions(jsonSerializerDefaults);
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            // Prepare the HTTP content itself.
            HttpContent content = JsonContent.Create<InputWrapper>(inputWrapper, mediaTypeHeaderValue, options);

            // Send the request.
            //HttpResponseMessage response = await this.httpClient.GetAsync(path);
            HttpResponseMessage response = await this.httpClient.PostAsync(path, content);

            // Wait for the response.
            string jsonData = await response.Content.ReadAsStringAsync();

            // Deserialize the response.
            IdAndPositionWrapper wrapper = Deserialize<IdAndPositionWrapper>(jsonData);

            // Store results in member field(s) if necessary.
            this.diffIDInResponse = new Guid(wrapper.Id);
            this.positionInReponse = wrapper.Position;

        }



        // POST <host>/v1/diff/<ID>/right
        // Place 2nd text stream for "diff" (right input).
        private void CallPostRightInput()
        {
        }



        // GET <host>/v1/diff/<ID>
        // Get the "diff" output.
        private void CallGetComparisonResult()
        {
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



    }



}
