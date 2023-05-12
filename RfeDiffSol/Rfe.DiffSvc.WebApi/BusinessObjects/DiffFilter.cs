using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Rfe.DiffSvc.WebApi.BusinessObjects
{



    /// <summary>
    /// Represents filter criteria when searching for <see cref="Diff"/> objects in the repository.
    /// </summary>
    public class DiffFilter
    {

        /// <summary>True :-: search for those Diffs where the left input part has already been filled, false :-: search for those that have an empty left part, null :-: this does not matter.</summary>
        public bool? HasLeftInput { get; set; }

        /// <summary>True :-: search for those Diffs where the right input part has already been filled, false :-: search for those that have an empty right part, null :-: this does not matter.</summary>
        public bool? HasRightInput { get; set; }

        /// <summary>True :-: search for those Diffs where the result has already been filled, false :-: search for those that have an empty result, null :-: this does not matter.</summary>
        public bool? HasResult { get; set; }

        /// <summary>If not null, search for those Diffs where their result is one of the mentioned values (results can be multiple, use the bitwise-OR to combine them).</summary>
        public DiffResult? Results { get; set; }

    }



}
