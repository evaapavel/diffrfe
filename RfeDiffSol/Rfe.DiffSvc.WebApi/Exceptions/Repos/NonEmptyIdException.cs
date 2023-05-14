using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.DiffSvc.WebApi.BusinessObjects;



namespace Rfe.DiffSvc.WebApi.Exceptions.Repos
{



    /// <summary>
    /// Represents an exceptional status where an item (a "diff") does have a particular ID set, but this shouldn't be.
    /// </summary>
    public class NonEmptyIdException : DiffRepoException
    {



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="diffWithNonEmptyId">The Diff object with a non-empty ID.</param>
        public NonEmptyIdException(Diff diffWithNonEmptyId) : base(diffWithNonEmptyId)
        {
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="diffWithNonEmptyId">The Diff object with a non-empty ID.</param>
        /// <param name="message">Error message.</param>
        public NonEmptyIdException(Diff diffWithNonEmptyId, string? message) : base(diffWithNonEmptyId, message)
        {
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="diffWithNonEmptyId">The Diff object with a non-empty ID.</param>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception.</param>
        public NonEmptyIdException(Diff diffWithNonEmptyId, string? message, Exception? innerException) : base(diffWithNonEmptyId, message, innerException)
        {
        }



        // Add info about the problematic Diff object.
        public override string ToString()
        {
            return "Diff with an ID unexpectedly set: " + this.CausingDiff + "    " + base.ToString();
        }



    }



}
