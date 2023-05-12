using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Rfe.DiffSvc.WebApi.BusinessObjects
{



    /// <summary>
    /// Represents the result of the diff operation performed on two character streams.
    /// The result can be sent back to the client once they require it.
    /// </summary>
    public class DiffOutput
    {



        /// <summary>Overall result of the diff operation.</summary>
        public DiffResult Result { get; set; }

        /// <summary>If the "result" is "LdiR", i.e. the streams are equally long, but still different, this collection contains all the differing sections within the input streams.</summary>
        public StringSection[]? DiffSections { get; set; }



        /// <summary>
        /// Constructor.
        /// </summary>
        public DiffOutput()
        {
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="result">Result of the diff operation to use.</param>
        public DiffOutput(DiffResult result) : this(result, null)
        {
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="result">Result of the "diff" to use.</param>
        /// <param name="diffSections">Differing sections.</param>
        public DiffOutput(DiffResult result, StringSection[]? diffSections)
        {
            this.Result = result;
            this.DiffSections = diffSections;
        }



        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="toCopy">Object to make a copy of.</param>
        public DiffOutput(DiffOutput toCopy)
        {
            this.Result = toCopy.Result;
            this.DiffSections = new StringSection[toCopy.DiffSections.Length];
            Array.Copy(toCopy.DiffSections, this.DiffSections, toCopy.DiffSections.Length);
        }



        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>Returns a clone of this instance.</returns>
        public DiffOutput Clone()
        {
            return new DiffOutput(this);
        }



    }



}
