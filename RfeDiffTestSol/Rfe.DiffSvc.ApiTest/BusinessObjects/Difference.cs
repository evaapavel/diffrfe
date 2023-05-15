using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Rfe.DiffSvc.ApiTest.BusinessObjects
{



    /// <summary>
    /// Represents a piece of input strings that is different on left vs right.
    /// </summary>
    public class Difference : IComparable<Difference>
    {



        /// <summary>Start index of a section with different characters.</summary>
        public int Offset { get; set; }

        /// <summary>Number of characters in the difference section.</summary>
        public int Length { get; set; }



        /// <summary>
        /// Constructor.
        /// </summary>
        public Difference()
        {
        }



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="offset">Start index of the section where Left differs from Right.</param>
        /// <param name="length">Length of the section where Left differs from Right.</param>
        public Difference(int offset, int length)
        {
            this.Offset = offset;
            this.Length = length;
        }



        // String representation of this instance.
        public override string ToString()
        {
            return $"{{ \"offset\": {this.Offset}, \"length\": {this.Length} }}";
        }



        // Hash function for keys in hash tables, dictionaries etc.
        public override int GetHashCode()
        {
            //return this.Offset ^ this.Length;
            return this.Offset.GetHashCode() ^ this.Length.GetHashCode();
        }



        // Decides whether this instance is equal to another object (in terms of their *content*).
        public override bool Equals(object obj)
        {
            Difference objAsDifference = obj as Difference;

            if (objAsDifference == null)
            {
                return false;
            }

            return ((this.Offset == objAsDifference.Offset) && (this.Length == objAsDifference.Length));
        }



        // Used in lists of Difference objects where we want to sort such lists.
        public int CompareTo(Difference other)
        {
            // Something goes AFTER nothing.
            if (other == null)
            {
                // If the other is null, we consider this to be AFTER the other.
                return +1;
            }

            // Primary information lies in the offsets.
            int offsetDiff = this.Offset - other.Offset;
            if (offsetDiff != 0)
            {
                return offsetDiff;
            }

            // Lengths are less important than offsets.
            int lengthDiff = this.Length - other.Length;
            return lengthDiff;
        }



    }



}
