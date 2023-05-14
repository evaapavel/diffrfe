using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using Rfe.DiffSvc.WebApi.Interfaces.Services;
using Rfe.DiffSvc.WebApi.Helpers;
using Rfe.DiffSvc.WebApi.BusinessObjects;


namespace Rfe.DiffSvc.WebApi.Controllers
{



    /// <summary>
    /// Contains endpoints for the RFE diff service.
    /// </summary>
    [ApiController]
    [Route("/v1/diff")]
    public class DiffController : ControllerBase
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
        // TODO: Handle errors and validate input.
        // TODO: Base64 encoding (to/from).



        // GET /v1/diff/get-id
        // Gets an ID for a new diff.
        [HttpGet("get-id")]
        public IActionResult GetId()
        {

            // Let the diff service generate a new id.
            Guid id = _diffService.GenerateId();

            // Wrap the ID in an (anonymous) object.
            var dataWithID = new { id = id.ToString() };

            // Return HTTP status code: 200 (OK)
            // Insert data into the response body.
            return Ok(dataWithID);

        }



        // POST /v1/diff/155a95ae-4132-470c-91b9-892a80cff59f/left
        // Posts a left or right input data.
        [HttpPost("{id:guid}/{position:regex(^((left)|(right))$)}")]
        public IActionResult Post([FromRoute] Guid id, [FromBody] StreamInput streamInput, [FromRoute] string position)
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



        // GET /v1/diff/155a95ae-4132-470c-91b9-892a80cff59f
        // Gets the result of the diff applied to "left" vs "right".
        [HttpGet("{id:guid}")]
        public IActionResult Get([FromRoute] Guid id)
        {

            // TODO: Try to invent something more efficient/elegant.
            // For the time being:
            // Call the diff calculation here.
            _diffService.CalculateDiff(id);

            // Get an output (result) for the requested id.
            DiffOutput output = _diffService.GetOutput(id);

            // Return HTTP status code: 200 (OK)
            // Insert output into the response body.
            return Ok(output);

        }



    }



}
