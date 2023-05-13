using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.DiffSvc.WebApi.BusinessObjects;
using Rfe.DiffSvc.WebApi.Interfaces.Repos;



namespace Rfe.DiffSvc.WebApi.Exceptions
{



    /// <summary>
    /// A base class for exceptions related to the <see cref="IDiffRepo"/> interface implementations.
    /// </summary>
    public class DiffRepoException : Exception
    {



        /// <summary>A Diff object with the ID that is causing the issue.</summary>
        public Diff CausingDiff { get; private set; }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="causingDiff">The causing Diff object.</param>
        public DiffRepoException(Diff causingDiff) : base()
        {
            this.CausingDiff = causingDiff;
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="causingDiff">The causing Diff object.</param>
        /// <param name="message">Error message.</param>
        public DiffRepoException(Diff causingDiff, string? message) : base(message)
        {
            this.CausingDiff = causingDiff;
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="causingDiff">The causing Diff object.</param>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception.</param>
        public DiffRepoException(Diff causingDiff, string? message, Exception? innerException) : base(message, innerException)
        {
            this.CausingDiff = causingDiff;
        }



        //// Add info about the missing Diff object.
        //public override string ToString()
        //{
        //    return "MissingDiff: " + this.MissingDiff + "    " + base.ToString();
        //}



    }



}
