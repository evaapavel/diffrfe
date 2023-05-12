using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Rfe.DiffSvc.WebApi.BusinessObjects
{



    /// <summary>
    /// Represents input data. The data gets sent by the client to the service repository.
    /// </summary>
    public class StreamInput
    {



        /// <summary>Input character stream to test for diff with the other.</summary>
        public string Input { get; set; }



        /// <summary>
        /// Constructor.
        /// </summary>
        public StreamInput()
        {
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="input">Input text to use.</param>
        public StreamInput(string input)
        {
            this.Input = input;
        }



        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="toCopy">Object to make a copy of.</param>
        public StreamInput(StreamInput toCopy)
        {
            this.Input = toCopy.Input;
        }



        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>Returns a clone of this instance.</returns>
        public StreamInput Clone()
        {
            return new StreamInput(this);
        }



    }



}
