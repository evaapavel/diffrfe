using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.DiffSvc.WebApi.BusinessObjects;



namespace Rfe.DiffSvc.WebApi.Exceptions.Services
{



    /// <summary>
    /// To be thrown when the "input-must-be-empty-on-posting-data" integrity check fails.
    /// </summary>
    public class InputAlreadySetException : DiffServiceException
    {



        /// <summary>Position of the input data that's causing problems.</summary>
        public DiffOperandPosition Position { get; private set; }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="causingID">ID of the causing diff.</param>
        /// <param name="position">Position of the problematic input data.</param>
        public InputAlreadySetException(Guid causingID, DiffOperandPosition position) : base(causingID)
        {
            this.Position = position;
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="causingID">ID of the causing diff.</param>
        /// <param name="position">Position of the problematic input data.</param>
        /// <param name="message">Error message.</param>
        public InputAlreadySetException(Guid causingID, DiffOperandPosition position, string? message) : base(causingID, message)
        {
            this.Position = position;
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="causingID">ID of the causing diff.</param>
        /// <param name="position">Position of the problematic input data.</param>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception.</param>
        public InputAlreadySetException(Guid causingID, DiffOperandPosition position, string? message, Exception? innerException) : base(causingID, message, innerException)
        {
            this.Position = position;
        }



        // Add info about the problematic Diff object.
        public override string ToString()
        {
            return $"The Diff with ID={this.CausingID} has the {this.Position} input already set.    " + base.ToString();
        }



    }



}
