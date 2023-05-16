using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.DiffSvc.WebApi.Interfaces.Services;
using Rfe.DiffSvc.WebApi.BusinessObjects;



namespace Rfe.DiffSvc.WebApi.Services
{



    /// <summary>
    /// Implements methods to compare two text strings.
    /// </summary>
    public class CompareService : ICompareService
    {



        // ---------------------------------------------------------------------------------------------------------------
        // Documentation of the public methods to be implemented is provided in the definition of the ICompareService interface.
        // ---------------------------------------------------------------------------------------------------------------



        // Compare two strings and return comparison info about the results.
        public void CompareAndFindDiffs(string first, string second, out int compareResult, out List<StringSection> differingSections)
        {

            // Compare lengths of the input streams first.
            int lengthDifference = first.Length - second.Length;

            // If the lengths are different, we're done.
            if (lengthDifference != 0)
            {
                compareResult = lengthDifference;
                differingSections = null;
                return;
            }

            // In case the lengths are equal, start comparing the strings character by character.
            // Build a list of different sections "on-the-fly".
            List<StringSection> diffSectionList = FindDiffSections(first, second);

            // If the difference list is empty, then the inputs are identical (totally equal).
            // If there is at least one "diff section", then return a "equal lengths, but differences" status.

            // Here: We're done.
            // We say: The strings have the same length.
            compareResult = 0;
            // It's the responsibility of the caller to examine the other output parameter (null, empty, or any diff sections).
            // If null or empty, then the strings are equal.
            // If the collection contains some items, then the strings (although with the same length) differ in some characters (character sections).
            if ( (diffSectionList != null) && (diffSectionList.Count > 0) )
            {
                // The strings differ.
                differingSections = diffSectionList;
            }
            else
            {
                // The strings are equal.
                differingSections = null;
            }

        }



        // Finds "diff sections" within the two given strings.
        // Assumes the strings have the same length.
        private List<StringSection> FindDiffSections(string first, string second)
        {

#if DEBUG
            // Integrity check: The lengths must be same.
            if (first.Length != second.Length)
            {
                throw new Exception($"Cannot diff two strings with different lengths (the lengths of first: {first.Length}, that of second: {second.Length})");
            }
#endif

            // Prepare a result.
            List<StringSection> diffSectionList = new List<StringSection>();

            // Compare chars one by one.
            int diffSectionOffset = -1;
            int diffSectionLength = -1;
            bool inDiffSection = false;
            for (int i = 0; i < first.Length; i++)
            {
                // The diff calculation depends on 2 pieces of state information:
                // 1) Whether we're currently in a diff section.
                // 2) Whether the current characters (in the first string and in the second 
                if (!inDiffSection)
                {
                    if (first[i] == second[i])
                    {
                        // The chars on the current position are equal.
                        // The chars on the previous position were equal, too.
                        // ---> Do nothing.
                    }
                    else
                    {
                        // The chars on the current position DIFFER.
                        // The chars on the previous position were equal, though.
                        // ---> A "diff section" starts here and now.
                        diffSectionOffset = i;
                        diffSectionLength = 1;
                        inDiffSection = true;
                    }
                }
                else
                {
                    if (first[i] == second[i])
                    {
                        // The chars on the current position are equal.
                        // The chars on the previous position were NOT equal.
                        // ---> The "diff section" has ended at the immediately previous position.
                        StringSection diffSection = new StringSection { Offset = diffSectionOffset, Length = diffSectionLength };
                        diffSectionList.Add(diffSection);

                        // Set a new state info.
                        diffSectionOffset = -1;
                        diffSectionLength = -1;
                        inDiffSection = false;
                    }
                    else
                    {
                        // The chars on the current position DIFFER.
                        // The chars on the previous position were different, too.
                        // ---> The "diff section" continues.
                        diffSectionLength++;
                    }
                }
            }

            // Once we get out of the loop, it is necessary to check whether we were "in diff section" when processing the very last character of the two input strings.
            if (inDiffSection)
            {
                // We WERE in a "diff section" when quitting the loop.
                // The chars on the previous position were NOT equal.
                // ---> The "diff section" has ended at the last character of both strings.
                StringSection diffSection = new StringSection { Offset = diffSectionOffset, Length = diffSectionLength };
                diffSectionList.Add(diffSection);

                // This is not necessary any more when we got out of the loop:
                //// Set a new state info.
                //diffSectionOffset = -1;
                //diffSectionLength = -1;
                //inDiffSection = false;
            }

            // Return the result.
            return diffSectionList;

        }



    }



}
