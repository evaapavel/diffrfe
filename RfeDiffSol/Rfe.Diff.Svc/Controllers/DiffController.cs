using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using Rfe.Diff.Svc.Interfaces.Services;



namespace Rfe.Diff.Svc.Controllers
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



    }



}
