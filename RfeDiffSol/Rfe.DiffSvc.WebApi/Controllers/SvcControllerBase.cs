using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Rfe.DiffSvc.WebApi.Controllers
{



    /// <summary>
    /// Adds useful stuff to the API controller base.
    /// </summary>
    public abstract class SvcControllerBase : ControllerBase
    {



        /// <summary>
        /// Gets an Intenal Server Error HTTP status (500).
        /// </summary>
        /// <returns>Returns the HTTP status code: 500 (Internal Server Error).</returns>
        [NonAction]
        public virtual StatusCodeResult InternalServerError()
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }



        /// <summary>
        /// Gets an Intenal Server Error HTTP status (500) with more explanation of the error.
        /// </summary>
        /// <param name="message">Message to include in the status code.</param>
        /// <returns>Returns the HTTP status code: 500 (Internal Server Error) with the message provided in the parameter.</returns>
        [NonAction]
        public virtual ObjectResult InternalServerError(string message)
        {
            ObjectResult result = new ObjectResult(message);
            result.StatusCode = StatusCodes.Status500InternalServerError;
            return result;
        }




    }



}
