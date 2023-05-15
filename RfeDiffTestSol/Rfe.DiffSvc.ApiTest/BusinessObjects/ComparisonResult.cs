using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Rfe.DiffSvc.ApiTest.BusinessObjects
{



    /// <summary>
    /// Represents the result of comparing the string "on the left" against the string "on the right".
    /// </summary>
    public class ComparisonResult
    {



        /// <summary>Text result of the comparison (LeqR, LgtR, LltR, or LdiR).</summary>
        public string Result { get; set; }

        /// <summary>Diff sections for the "LdiR" case.</summary>
        public List<Difference> DiffSections { get; set; }



        /// <summary>
        /// Performs a normalization of the data content of this instance.
        /// It should sort the diff sections in order for the Equals operation to be simple.
        /// </summary>
        public void Normalize()
        {
            if (this.DiffSections != null)
            {
                this.DiffSections.Sort();
            }
        }



        // Decides whether this equals to the other object.
        public override bool Equals(object obj)
        {
            ComparisonResult crObj = obj as ComparisonResult;

            if (crObj == null)
            {
                return false;
            }

            if (this.Result != crObj.Result)
            {
                return false;
            }

            if ((this.DiffSections == null) && (crObj.DiffSections == null))
            {
                return true;
            }

            if ( ((this.DiffSections == null) && (crObj.DiffSections != null)) || ((this.DiffSections != null) && (crObj.DiffSections == null)) )
            {
                return false;
            }

            // Here - both DiffSections shouldn't be null.
            // Assume that both DiffSections are normalized.
            for (int i = 0; i < this.DiffSections.Count; i++)
            {
                if ( ! this.DiffSections[i].Equals(crObj.DiffSections[i]) )
                {
                    return false;
                }
            }

            // OK. Everything is matching.
            return true;
        }



    }



}
