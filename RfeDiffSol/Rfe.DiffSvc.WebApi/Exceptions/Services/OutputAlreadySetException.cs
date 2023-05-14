using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.DiffSvc.WebApi.BusinessObjects;



namespace Rfe.DiffSvc.WebApi.Exceptions.Services
{



    /// <summary>
    /// To be thrown when the "output-must-be-empty-on-calculating-diff" integrity check fails.
    /// </summary>
    public class OutputAlreadySetException : DiffServiceException
    {



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="causingID">ID of the causing diff.</param>
        public OutputAlreadySetException(Guid causingID) : base(causingID)
        {
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="causingID">ID of the causing diff.</param>
        /// <param name="message">Error message.</param>
        public OutputAlreadySetException(Guid causingID, string? message) : base(causingID, message)
        {
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="causingID">ID of the causing diff.</param>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception.</param>
        public OutputAlreadySetException(Guid causingID, string? message, Exception? innerException) : base(causingID, message, innerException)
        {
        }



        // Add info about the problematic Diff object.
        public override string ToString()
        {
            return $"The Diff with ID={this.CausingID} has the output already set.    " + base.ToString();
        }



    }



}
