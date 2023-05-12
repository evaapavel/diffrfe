using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Rfe.DiffSvc.WebApi.BusinessObjects
{



    /// <summary>
    /// Encapsulates the inputs and the output of the diff operation.
    /// Instances of this class are stored in an in-memory database.
    /// </summary>
    public class Diff
    {



        /// <summary>ID of this particular object in the repo.</summary>
        public Guid ID { get; set; }

        /// <summary>1st input stream to compare.</summary>
        public StreamInput Left { get; set; }

        /// <summary>2nd input stream to compare.</summary>
        public StreamInput Right { get; set; }

        /// <summary>Comparison result (the "diff").</summary>
        public DiffOutput Output { get; set; }



        /// <summary>
        /// Constructor.
        /// </summary>
        public Diff()
        {
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">ID to use for the instance.</param>
        /// <param name="left">Left input text stream.</param>
        /// <param name="right">Right input text stream.</param>
        /// <param name="output">Comparison result (result of the "diff" operation).</param>
        public Diff(Guid id, StreamInput left, StreamInput right, DiffOutput output)
        {
            this.ID = id;
            this.Left = left;
            this.Right = right;
            this.Output = output;
        }



        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="toCopy">Object to make a copy of.</param>
        public Diff(Diff toCopy)
        {
            this.ID = toCopy.ID;
            if (toCopy.Left != null)
            {
                this.Left = toCopy.Left.Clone();
            }
            else
            {
                this.Left = null;
            }
            if (toCopy.Right != null)
            {
                this.Right = toCopy.Right.Clone();
            }
            else
            {
                this.Right = null;
            }
            if (toCopy.Output != null)
            {
                this.Output = toCopy.Output.Clone();
            }
            else
            {
                this.Output = null;
            }
        }



        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>Returns a clone of this instance.</returns>
        public Diff Clone()
        {
            return new Diff(this);
        }



        // Use ID for ToString().
        public override string ToString()
        {
            return this.ID.ToString();
        }



    }



}
