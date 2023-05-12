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



        /// <summary>1st input stream to compare.</summary>
        public StreamInput Left { get; set; }

        /// <summary>2nd input stream to compare.</summary>
        public StreamInput Right { get; set; }

        /// <summary>Comparison result (the "diff").</summary>
        public DiffResult Result { get; set; }



    }



}
