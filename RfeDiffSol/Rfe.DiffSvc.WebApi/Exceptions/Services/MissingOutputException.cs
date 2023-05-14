using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.DiffSvc.WebApi.BusinessObjects;



namespace Rfe.DiffSvc.WebApi.Exceptions.Services
{



    /// <summary>
    /// To be thrown when the "output-must-be-filled" integrity check fails.
    /// </summary>
    public class MissingOutputException : DiffServiceException
    {



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="causingID">ID of the causing diff.</param>
        public MissingOutputException(Guid causingID) : base(causingID)
        {
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="causingID">ID of the causing diff.</param>
        /// <param name="message">Error message.</param>
        public MissingOutputException(Guid causingID, string? message) : base(causingID, message)
        {
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="causingID">ID of the causing diff.</param>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception.</param>
        public MissingOutputException(Guid causingID, string? message, Exception? innerException) : base(causingID, message, innerException)
        {
        }



        // Add info about the problematic Diff object.
        public override string ToString()
        {
            return $"The Diff with ID={this.CausingID} has an empty output.    " + base.ToString();
        }



    }



}
