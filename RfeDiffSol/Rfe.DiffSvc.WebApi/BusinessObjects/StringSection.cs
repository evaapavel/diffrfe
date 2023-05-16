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
    public struct StringSection : IComparable<StringSection>
    {



        /// <summary>Index of the starting character of this string section.</summary>
        public int Offset { get; set; }

        /// <summary>Number of characters within this string section.</summary>
        public int Length { get; set; }



        ///// <summary>
        ///// Constructor.
        ///// </summary>
        //public StringSection()
        //{
        //}



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="offset">Starting index of this section.</param>
        /// <param name="length">Number of characters in this section.</param>
        public StringSection(int offset, int length)
        {
            this.Offset = offset;
            this.Length = length;
        }



        // Implement the IComparable<T> interface in order to simply sort collections of items of this type.
        public int CompareTo(StringSection other)
        {
            //if (other == null)
            //{
            //    return +1;
            //}

            if (this.Offset != other.Offset)
            {
                return this.Offset - other.Offset;
            }

            return this.Length - other.Length;
        }



        // Gets a string representation of this instance.
        public override string ToString()
        {
            //return $"{{offset: {this.Offset}, length: {this.Length}}}";
            return $"({this.Offset}, {this.Length})";
        }



    }



}
