using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.DiffSvc.WebApi.BusinessObjects;



namespace Rfe.DiffSvc.WebApi.Exceptions
{



    /// <summary>
    /// Represents an exceptional status where an item (a "diff") has no particular ID (its ID is empty).
    /// </summary>
    public class IdNotSetException : DiffException
    {



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="diffWithoutId">The Diff object that is missing a non-empty ID.</param>
        public IdNotSetException(Diff diffWithoutId) : base(diffWithoutId)
        {
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="diffWithoutId">The Diff object that is missing a non-empty ID.</param>
        /// <param name="message">Error message.</param>
        public IdNotSetException(Diff diffWithoutId, string? message) : base(diffWithoutId, message)
        {
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="diffWithoutId">The Diff object that is missing a non-empty ID.</param>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception.</param>
        public IdNotSetException(Diff diffWithoutId, string? message, Exception? innerException) : base(diffWithoutId, message, innerException)
        {
        }



        // Add info about the problematic Diff object.
        public override string ToString()
        {
            return "Diff with no ID: " + this.CausingDiff + "    " + base.ToString();
        }



    }



}
