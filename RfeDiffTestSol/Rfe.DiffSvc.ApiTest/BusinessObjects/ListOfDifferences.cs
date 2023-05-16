using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Rfe.DiffSvc.ApiTest.BusinessObjects
{



    /// <summary>
    /// Represents a list of different parts in the input strings (left vs right).
    /// Instances of this class can differentiate between "the list is missing" (null) and "the list is empty" (there is a list, but there are no items in it).
    /// </summary>
    public class ListOfDifferences
    {



        // Underlying data structure.
        private List<Difference> differences;



        /// <summary>
        /// Constructor.
        /// </summary>
        public ListOfDifferences()
        {
            // Missing list.
            this.differences = null;
        }



        ///// <summary>
        ///// Constructor.
        ///// </summary>
        ///// <param name="makeEmpty">True :-: make the inner list empty at the beginning, false :-: make the inner list "missing" from the beginning.</param>
        //public ListOfDifferences(bool makeEmpty)
        //{
        //    if (makeEmpty)
        //    {
        //        this.differences = new List<Difference>();
        //    }
        //    else
        //    {
        //        this.differences = null;
        //    }
        //}



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="differences">List of differences to initialize this instance with.</param>
        public ListOfDifferences(List<Difference> differences)
        {
            this.differences = differences;
        }



        ///// <summary>
        ///// Copy constructor.
        ///// </summary>
        ///// <param name="copyFrom">Other instance to copy data from.</param>
        //public ListOfDifferences(ListOfDifferences copyFrom)
        //{
        //    this.differences = copyFrom.differences;
        //}



        /// <summary>
        /// Adds a different part to the list of diffs.
        /// </summary>
        /// <param name="offset">Starting index.</param>
        /// <param name="length">Length of diff section.</param>
        public void AddPart(int offset, int length)
        {
            AddPart(new Difference(offset, length));
        }



        /// <summary>
        /// Adds a different part to the list of diffs.
        /// </summary>
        /// <param name="differentPart">Different part to add.</param>
        public void AddPart(Difference differentPart)
        {
            if (this.differences == null)
            {
                this.differences = new List<Difference>();
            }
            this.differences.Add(differentPart);
        }



        /// <summary>Gets the list of different parts stored in this instance.</summary>
        public List<Difference> DifferentParts
        {
            get { return this.differences; }
        }



        /// <summary>Gets true :-: the inner list is null, false :-: the inner list exists, no matter how many items it has.</summary>
        public bool IsMissing
        {
            get { return (this.differences == null); }
        }



        /// <summary>Gets true :-: the inner list is empty, false :-: otherwise (the list is either completely missing, or has some items).</summary>
        public bool IsEmpty
        {
            get { return ((this.differences != null) && (this.differences.Count == 0)); }
        }



        /// <summary>Gets the number of items in the inner list. In case the list is missing, the value of this property is -1.</summary>
        public int Count
        {
            get { return ((this.differences != null) ? (this.differences.Count) : (-1)); }
        }



        /// <summary>
        /// Normalizes the inner list in order to simplify comparisons.
        /// </summary>
        public void Normalize()
        {
            if (this.differences != null)
            {
                this.differences.Sort();
            }
        }



        // String representation of this instace.
        // "JSON"-ize its data.
        public override string ToString()
        {
            if (this.differences == null)
            {
                return "null";
            }
            if (this.differences.Count == 0)
            {
                return "[]";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            sb.Append( this.differences.Select(d => d.ToString()).Aggregate((acc, diffStr) => acc + ", " + diffStr) );
            sb.Append(" ]");
            return sb.ToString();
        }



        /// <summary>
        /// Compares given diff lists. Before the comparison starts, the lists are normalized.
        /// This means that this operation is NOT READONLY. It changes the input lists.
        /// Assumes both first and second are objects (should not be null).
        /// </summary>
        /// <param name="first">First diff list to be compared.</param>
        /// <param name="second">Second diff list to be comared against the first one.</param>
        /// <returns>Returns true :-: the (normalized) lists are equal, false :-: the lists differ.</returns>
        //public static bool HaveSamePartsAfterNormalization(ListOfDifferences first, ListOfDifferences second)
        public static bool AreEqualAfterNormalization(ListOfDifferences first, ListOfDifferences second)
        {

            // Normalize the lists.
            first.Normalize();
            second.Normalize();

            // Compare them.

            // Missing lists.
            if (first.IsMissing && second.IsMissing)
            {
                // If both are missing, then they're equal.
                return true;
            }
            if (first.IsMissing || second.IsMissing)
            {
                // If one is missing and the other is not, then they're NOT equal.
                return false;
            }

            // (Empty lists can be handled together with non-empty ones).

            // Do compare.
            for (int i = 0; i < first.DifferentParts.Count; i++)
            {
                // If any diff part in the first is different from the corresponding one in the second, the diff lists are NOT equal.
                if ( ! first.DifferentParts[i].Equals(second.DifferentParts[i]) )
                {
                    return false;
                }
            }

            // It seems that the lists are equal.
            return true;

        }



    }



}
