using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.DiffSvc.WebApi.BusinessObjects;



namespace Rfe.DiffSvc.WebApi.Exceptions
{



    /// <summary>
    /// Represents an exceptional status where an item (a "diff") with a particular ID does not exist in the repo.
    /// </summary>
    public class NotFoundException : DiffException
    {



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="missingDiff">The missing Diff object.</param>
        public NotFoundException(Diff missingDiff) : base(missingDiff)
        {
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="missingDiff">The missing Diff object.</param>
        /// <param name="message">Error message.</param>
        public NotFoundException(Diff missingDiff, string? message) : base(missingDiff, message)
        {
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="missingDiff">The missing Diff object.</param>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception.</param>
        public NotFoundException(Diff missingDiff, string? message, Exception? innerException) : base(missingDiff, message, innerException)
        {
        }



        // Add info about the missing Diff object.
        public override string ToString()
        {
            return "Missing Diff: " + this.CausingDiff + "    " + base.ToString();
        }



    }



}
