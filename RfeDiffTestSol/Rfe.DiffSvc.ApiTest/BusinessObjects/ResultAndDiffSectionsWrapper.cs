using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Rfe.DiffSvc.ApiTest.BusinessObjects
{



    /// <summary>
    /// Wraps an API response with a diff result and a list of different parts in the two input streams.
    /// </summary>
    public class ResultAndDiffSectionsWrapper
    {



        /// <summary>Text result of the comparison (LeqR, LgtR, LltR, or LdiR).</summary>
        public string Result { get; set; }

        /// <summary>Diff sections for the "LdiR" case.</summary>
        public List<Difference> DiffSections { get; set; }



    }



}
