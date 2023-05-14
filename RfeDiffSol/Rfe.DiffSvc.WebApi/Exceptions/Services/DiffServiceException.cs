using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.DiffSvc.WebApi.Interfaces.Services;



namespace Rfe.DiffSvc.WebApi.Exceptions.Services
{



    /// <summary>
    /// Base class for <see cref="IDiffService"/> related exceptions.
    /// </summary>
    public class DiffServiceException : Exception
    {



        /// <summary>Which diff ID caused the exception.</summary>
        public Guid CausingID { get; private set; }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="causingID">ID of the causing diff.</param>
        public DiffServiceException(Guid causingID) : base()
        {
            this.CausingID = causingID;
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="causingID">ID of the causing diff.</param>
        /// <param name="message">Error message.</param>
        public DiffServiceException(Guid causingID, string? message) : base(message)
        {
            this.CausingID = causingID;
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="causingID">ID of the causing diff.</param>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception.</param>
        public DiffServiceException(Guid causingID, string? message, Exception? innerException) : base(message, innerException)
        {
            this.CausingID = causingID;
        }



    }



}
