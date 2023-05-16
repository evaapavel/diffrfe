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
                return true;
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
