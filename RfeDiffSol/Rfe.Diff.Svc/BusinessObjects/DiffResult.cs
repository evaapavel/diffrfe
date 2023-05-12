using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Rfe.DiffSvc.WebApi.BusinessObjects
{



    /// <summary>
    /// Represents the main part of the result of a diff operation performed on two input text streams.
    /// The diff may result in:
    /// <list type="bullet">
    /// <item>The text streams are identical (meaning they are of the same length and they do not differ in any character).</item>
    /// <item>1st stream (left) is longer than 2nd stream (right).</item>
    /// <item>1st stream is shorter than 2nd one.</item>
    /// <item>The streams are equally long, but there are some character differences in them.</item>
    /// </list>
    /// </summary>
    public enum DiffResult
    {

        /// <summary>The strings are equal (identical).</summary>
        LeqR,

        /// <summary>Left string is longer than Right string.</summary>
        LgtR,

        /// <summary>Left string is shorter than Right string.</summary>
        LltR,

        /// <summary>The strings have the same length, but they differ in some characters.</summary>
        LdiR

    }



}
