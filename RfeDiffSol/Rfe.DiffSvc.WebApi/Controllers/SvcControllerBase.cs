using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Text.Json;



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



        /// <summary>
        /// Converts a given object (result of an HTTP request) to JSON.
        /// Adds also HTTP status code information to the resulting string.
        /// Suitable for logging purposes.
        /// </summary>
        /// <param name="result">Result of an HTTP request to prepare for logging.</param>
        /// <returns>Returns a JSONised form of the given response data. Adds the HTTP status code, too.</returns>
        protected string Logify(ObjectResult result)
        {
            string objectAsJson = JsonSerializer.Serialize(result.Value);
            string logifiedStatusAndData = $"Status: {result.StatusCode}   Data: '{objectAsJson}'";
            return logifiedStatusAndData;
        }



        /// <summary>
        /// Converts a result of an HTTP request to a string suitable for logging.
        /// </summary>
        /// <param name="result">Result of an HTTP request to prepare for logging.</param>
        /// <returns>Returns the HTTP status code with some info ready to be logged.</returns>
        protected string Logify(StatusCodeResult result)
        {
            string logifiedStatus = $"Status: {result.StatusCode}   (No data)";
            return logifiedStatus;
        }



        /// <summary>
        /// Converts a given object to JSON.
        /// Suitable for logging purposes. Used to log requests rather than responses.
        /// </summary>
        /// <param name="data">Data object to prepare for logging.</param>
        /// <returns>Returns a JSONised form of the given data.</returns>
        protected string Logify(object data)
        {
            string dataAsJson = JsonSerializer.Serialize(data);
            string logifiedData = $"Data: '{dataAsJson}'";
            return logifiedData;
        }



    }



}
