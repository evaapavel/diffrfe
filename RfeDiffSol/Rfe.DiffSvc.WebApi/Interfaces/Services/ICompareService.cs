using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.DiffSvc.WebApi.BusinessObjects;



namespace Rfe.DiffSvc.WebApi.Interfaces.Services
{



    /// <summary>
    /// Defines methods that would be useful for string comparison purposes.
    /// </summary>
    public interface ICompareService
    {



        /// <summary>
        /// Compares two strings and returns a comparison result with possible differing sections.
        /// <list type="bullet">
        /// <item>If the comparison result is a positive number, then it means the first string is longer than the second one.</item>
        /// <item>If the comparison result is a negative number, then the first string is shorter than the second.</item>
        /// <item>
        ///     If the comparison result is zero, you need to consult the other output parameter:
        ///     <list type="number">
        ///     <item>If <paramref name="differingSections"/> is null or empty, then the strings are identical (equal).</item>
        ///     <item>If the differingSections collection is NOT empty, then each its item refers to one "differing" section within the input strings.</item>
        ///     </list>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="first">First string to compare.</param>
        /// <param name="second">Second string to compare.</param>
        /// <param name="compareResult">Result of the comparison.</param>
        /// <param name="differingSections"></param>
        void CompareAndFindDiffs(string first, string second, out int compareResult, out List<StringSection> differingSections);



    }



}
