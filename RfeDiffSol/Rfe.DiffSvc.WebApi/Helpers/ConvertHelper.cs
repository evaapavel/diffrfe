using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.DiffSvc.WebApi.BusinessObjects;



namespace Rfe.DiffSvc.WebApi.Helpers
{



    /// <summary>
    /// Exposes static methods for data conversions.
    /// </summary>
    public static class ConvertHelper
    {



        /// <summary>
        /// Converts a given string into a <see cref="DiffOperandPosition"/> enum value.
        /// </summary>
        /// <param name="positionAsString">A DiffOperandPosition as a string.</param>
        /// <returns>Returns an instance of DiffOperandPosition corresponding to the given string.</returns>
        public static DiffOperandPosition ToDiffOperandPosition(string positionAsString)
        {
            // TODO: Integrity check - make sure the given constant exists in the enum.

            // Parse the string. Try to get a corresponding value from values in the DiffOperandPosition enum.
            // Disregard the upper/lower" case.
            DiffOperandPosition position = (DiffOperandPosition)Enum.Parse(typeof(DiffOperandPosition), positionAsString, true);
            return position;
        }



    }



}
