using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Rfe.DiffSvc.WebApi.BusinessObjects
{



    /// <summary>
    /// Represents a part of a text string.
    /// It's a sort of the string's substring, but rather it defines the substring with its beginning (offset) and the number of its characters (length).
    /// </summary>
    //public class StringSection
    public struct StringSection
    {

        /// <summary>Index of the starting character of this string section.</summary>
        public int Offset { get; set; }

        /// <summary>Number of characters within this string section.</summary>
        public int Length { get; set; }

    }



}
