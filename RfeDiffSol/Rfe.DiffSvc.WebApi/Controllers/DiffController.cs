using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

//using System.Web.Http;

using Rfe.DiffSvc.WebApi.Interfaces.Services;
using Rfe.DiffSvc.WebApi.Helpers;
using Rfe.DiffSvc.WebApi.BusinessObjects;

using Rfe.DiffSvc.WebApi.Exceptions.Repos;
using Rfe.DiffSvc.WebApi.Exceptions.Services;
using System.Security.Cryptography.X509Certificates;

namespace Rfe.DiffSvc.WebApi.Controllers
{



    /// <summary>
    /// Contains endpoints for the RFE diff service.
    /// </summary>
    [ApiController]
    [Route("/v1/diff")]
    public class DiffController : SvcControllerBase
    {



        // Logging purposes dependency.
        private readonly ILogger<DiffController> _logger;

        // Diff service implementation.
        private readonly IDiffService _diffService;



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger">Logger object.</param>
        /// <param name="diffService">Diff service implementation.</param>
        public DiffController(ILogger<DiffController> logger, IDiffService diffService)
        {
            _logger = logger;
            _diffService = diffService;
        }



        // Implement the core functionality first.
        // TODO: Handle errors. DONE.
        // TODO: Validate input. DONE.
        // TODO: Base64 encoding (to/from).
        // TODO: Add logging. We could use AOP (Metalama) here (in order to keep things simple).

        // As to the validation of request parameters:
        // Since we use route constraints here, the only input to be concerned about is the input data in the request body.
        // This is related to the ".../left" and ".../right" endpoints.
        // See: https://learn.microsoft.com/en-us/aspnet/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2#route-constraints



        // GET /v1/diff/get-id
        // Gets an ID for a new diff.
        [HttpGet("get-id")]
        public IActionResult GetId()
        {

            // Log the request.
            LogInfoRequest();

            try
            {
                // Let the diff service generate a new id.
                Guid id = _diffService.GenerateId();

                // Wrap the ID in an (anonymous) object.
                var dataWithID = new { id = id.ToString() };

                // Return HTTP status code: 200 (OK)
                // Insert data into the response body.
                ObjectResult result = Ok(dataWithID);
                // Log the response.
                LogInfoResult(result);
                // Return the response.
                return result;
            }
            catch (Exception ex)
            {
                // Return HTTP status code: 500 (Internal Server Error)
                // To protect against possible hack attempts, do not specify the exact reason.
                StatusCodeResult result = InternalServerError();
                // Log the issue.
                LogErrResult(ex, result);
                // Return the response.
                return result;
            }

        }



        // POST /v1/diff/155a95ae-4132-470c-91b9-892a80cff59f/left
        // Posts left or right input data.
        [HttpPost("{id:guid}/{position:regex(^((left)|(right))$)}")]
        public IActionResult Post([FromRoute] Guid id, [FromBody] StreamInput streamInput, [FromRoute] string position)
        {

            // Log the request.
            LogInfoRequest(streamInput);

            // Check input data.
            // If the streamInput parameter is null or its data is null,
            // we will just return: 400 (Bad Request)
            // Note that streamInput.Input will be null in cases where the input JSON is missing the "input" property, for example.
            if ((streamInput == null) || (streamInput.Input == null))
            {
                // Return HTTP status code: 400 (Bad Request)
                BadRequestResult result = BadRequest();
                LogErrResult(result);
                return result;
            }

            try
            {
                // Parse the given diff operand position.
                DiffOperandPosition positionFromEnum = ConvertHelper.ToDiffOperandPosition(position);

                // Try to store data in the repo.
                _diffService.SaveInput(id, streamInput, positionFromEnum);

                // Wrap ID and position in an anonymous object.
                var dataWithParams = new { id = id.ToString(), position = positionFromEnum.ToString() };

                // Return HTTP status code: 201 (Created)
                ObjectResult result = Created(this.Request.Path, dataWithParams);
                LogInfoResult(result);
                return result;
            }
            catch (NotFoundException ex)
            {
                // This means that the id passed in the request does not exist.

                // Wrap the exception message together with parameters into an object to return.
                var errorDataWithParams = new { error = ex.Message, id = id.ToString(), position = position };
                // Return HTTP status code: 404 (Not Found)
                ObjectResult result = NotFound(errorDataWithParams);
                LogErrResult(ex, result);
                return result;
            }
            catch (InputAlreadySetException ex)
            {
                // This means inconsistent data:
                // For example, a POST request is made to store the "left input", however, left input data has already been stored in one of the previous requests.

                // Wrap the exception message together with parameters into an object to return.
                var errorDataWithParams = new { error = ex.Message, id = id.ToString(), position = position };
                // Return HTTP status code: 409 (Conflict)
                ObjectResult result = Conflict(errorDataWithParams);
                LogErrResult(ex, result);
                return result;
            }
            catch (Exception ex)
            {
                // Return HTTP status code: 500 (Internal Server Error)
                // To protect against possible hack attempts, do not specify the exact reason.
                StatusCodeResult result = InternalServerError();
                LogErrResult(ex, result);
                return result;
            }

        }



        // GET /v1/diff/155a95ae-4132-470c-91b9-892a80cff59f
        // Gets the result of the diff applied to "left" vs "right".
        [HttpGet("{id:guid}")]
        public IActionResult Get([FromRoute] Guid id)
        {

            // Log the request.
            LogInfoRequest();

            try
            {
                // TODO: Try to invent something more efficient/elegant.
                // For the time being:
                // Call the diff calculation here.
                _diffService.CalculateDiff(id);

                // Get an output (result) for the requested id.
                DiffOutput output = _diffService.GetOutput(id);

                // Return HTTP status code: 200 (OK)
                // Insert output into the response body.
                // TODO: Fix the current behaviour: When converting the "output" object into JSON, the DiffResult value is encoded as an integer instead of its string representation (name of the constant).
                ObjectResult result = Ok(output);
                LogInfoResult(result);
                return result;
            }
            catch (NotFoundException ex)
            {
                // This means that the id passed in the request does not exist.

                // Wrap the exception message together with the parameter into an object to return.
                var errorDataWithParams = new { error = ex.Message, id = id.ToString() };
                // Return HTTP status code: 404 (Not Found)
                ObjectResult result = NotFound(errorDataWithParams);
                LogErrResult(ex, result);
                return result;
            }
            catch (DiffServiceException ex)
            {
                // This means inconsistent data:
                // For example, the client requests a diff result, but no output data is available yet.

                // Wrap the exception message together with the parameter into an object to return.
                var errorDataWithParams = new { error = ex.Message, id = id.ToString() };
                // Return HTTP status code: 409 (Conflict)
                ObjectResult result = Conflict(errorDataWithParams);
                LogErrResult(ex, result);
                return result;
            }
            catch (Exception ex)
            {
                // Return HTTP status code: 500 (Internal Server Error)
                // To protect against possible hack attempts, do not specify the exact reason.
                StatusCodeResult result = InternalServerError();
                LogErrResult(ex, result);
                return result;
            }

        }



        // The following methods help to log requests and responses.

        // Log the request.
        //_logger.LogInformation("Request   :   {0} {1} {2}/{3}", HttpContext.TraceIdentifier, "GET", "/v1/diff", "get-id");
        private void LogInfoRequest()
        {
            _logger.LogInformation("Request   :   {0} {1} {2}", this.HttpContext.TraceIdentifier, this.Request.Method.ToUpper(), this.Request.Path);
        }

        // Log the request including some data (probably taken from the request body).
        //_logger.LogInformation("Request   :   {0} {1} {2}/{3}/{4}   Input:'{5}'", HttpContext.TraceIdentifier, "POST", "/v1/diff", id, position, ((streamInput != null) ? (streamInput.Input) : ("<N/A>")) );
        private void LogInfoRequest(object bodyData)
        {
            _logger.LogInformation("Request   :   {0} {1} {2}   - Body {3}", this.HttpContext.TraceIdentifier, this.Request.Method.ToUpper(), this.Request.Path, ((bodyData != null) ? (Logify(bodyData)) : ("<N/A>")) );
        }

        // Log the response (with some additional data).
        //_logger.LogInformation("Response  :   {0} {1}", HttpContext.TraceIdentifier, Logify(result));
        private void LogInfoResult(ObjectResult result)
        {
            _logger.LogInformation("Response  :   {0} {1}", this.HttpContext.TraceIdentifier, Logify(result));
        }

        // Log the response (just the HTTP status code).
        //_logger.LogInformation("Response  :   {0} {1}", HttpContext.TraceIdentifier, Logify(result));
        private void LogInfoResult(StatusCodeResult result)
        {
            _logger.LogInformation("Response  :   {0} {1}", this.HttpContext.TraceIdentifier, Logify(result));
        }

        // Log an error (with some data).
        //_logger.LogError(ex, "Error     :   {0} {1}", HttpContext.TraceIdentifier, Logify(result));
        private void LogErrResult(Exception ex, ObjectResult result)
        {
            _logger.LogError(ex, "Error     :   {0} {1}", this.HttpContext.TraceIdentifier, Logify(result));
        }

        // Log an error (just the status code).
        //_logger.LogError(ex, "Error     :   {0} {1}", HttpContext.TraceIdentifier, Logify(result));
        private void LogErrResult(Exception ex, StatusCodeResult result)
        {
            _logger.LogError(ex, "Error     :   {0} {1}", this.HttpContext.TraceIdentifier, Logify(result));
        }

        // Log an error (with some data). Outside try-catch (no exception).
        //_logger.LogError("Error     :   {0} {1}", HttpContext.TraceIdentifier, Logify(result));
        private void LogErrResult(ObjectResult result)
        {
            _logger.LogError("Error     :   {0} {1}", this.HttpContext.TraceIdentifier, Logify(result));
        }

        // Log an error (just the status code). Outside try-catch (no exception).
        //_logger.LogError("Error     :   {0} {1}", HttpContext.TraceIdentifier, Logify(result));
        private void LogErrResult(StatusCodeResult result)
        {
            _logger.LogError("Error     :   {0} {1}", this.HttpContext.TraceIdentifier, Logify(result));
        }



    }



}
