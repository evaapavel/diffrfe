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
        // TODO: Validate input.
        // TODO: Base64 encoding (to/from).
        // TODO: Add logging. We could use AOP (Metalama) here (in order to keep things simple).



        // GET /v1/diff/get-id
        // Gets an ID for a new diff.
        [HttpGet("get-id")]
        public IActionResult GetId()
        {
            try
            {
                // Let the diff service generate a new id.
                Guid id = _diffService.GenerateId();

                // Wrap the ID in an (anonymous) object.
                var dataWithID = new { id = id.ToString() };

                // Return HTTP status code: 200 (OK)
                // Insert data into the response body.
                return Ok(dataWithID);
            }
            catch (Exception)
            {
                // Return HTTP status code: 500 (Internal Server Error)
                // To protect against possible hack attempts, do not specify the exact reason.
                return InternalServerError();
            }
        }



        // POST /v1/diff/155a95ae-4132-470c-91b9-892a80cff59f/left
        // Posts left or right input data.
        [HttpPost("{id:guid}/{position:regex(^((left)|(right))$)}")]
        public IActionResult Post([FromRoute] Guid id, [FromBody] StreamInput streamInput, [FromRoute] string position)
        {
            try
            {
                // Parse the given diff operand position.
                DiffOperandPosition positionFromEnum = ConvertHelper.ToDiffOperandPosition(position);

                // Try to store data in the repo.
                _diffService.SaveInput(id, streamInput, positionFromEnum);

                // Wrap ID and position in an anonymous object.
                var dataWithParams = new { id = id.ToString(), position = positionFromEnum.ToString() };

                // Return HTTP status code: 201 (Created)
                return Created(this.Request.Path, dataWithParams);
            }
            catch (NotFoundException ex)
            {
                // This means that the id passed in the request does not exist.

                // Wrap the exception message together with parameters into an object to return.
                var errorDataWithParams = new { error = ex.Message, id = id.ToString(), position = position };
                // Return HTTP status code: 404 (Not Found)
                return NotFound(errorDataWithParams);
            }
            catch (InputAlreadySetException ex)
            {
                // This means inconsistent data:
                // For example, a POST request is made to store the "left input", however, left input data has already been stored in one of the previous requests.

                // Wrap the exception message together with parameters into an object to return.
                var errorDataWithParams = new { error = ex.Message, id = id.ToString(), position = position };
                // Return HTTP status code: 409 (Conflict)
                return Conflict(errorDataWithParams);
            }
            catch (Exception)
            {
                // Return HTTP status code: 500 (Internal Server Error)
                // To protect against possible hack attempts, do not specify the exact reason.
                return InternalServerError();
            }
        }



        // GET /v1/diff/155a95ae-4132-470c-91b9-892a80cff59f
        // Gets the result of the diff applied to "left" vs "right".
        [HttpGet("{id:guid}")]
        public IActionResult Get([FromRoute] Guid id)
        {
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
                return Ok(output);
            }
            catch (NotFoundException ex)
            {
                // This means that the id passed in the request does not exist.

                // Wrap the exception message together with the parameter into an object to return.
                var errorDataWithParams = new { error = ex.Message, id = id.ToString() };
                // Return HTTP status code: 404 (Not Found)
                return NotFound(errorDataWithParams);
            }
            catch (DiffServiceException ex)
            {
                // This means inconsistent data:
                // For example, the client requests a diff result, but no output data is available yet.

                // Wrap the exception message together with the parameter into an object to return.
                var errorDataWithParams = new { error = ex.Message, id = id.ToString() };
                // Return HTTP status code: 409 (Conflict)
                return Conflict(errorDataWithParams);
            }
            catch (Exception)
            {
                // Return HTTP status code: 500 (Internal Server Error)
                // To protect against possible hack attempts, do not specify the exact reason.
                return InternalServerError();
            }
        }



    }



}
