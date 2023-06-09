﻿using System;
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
    [Flags]
    public enum DiffResult
    {

        /// <summary>An "empty" value.</summary>
        None = 0,

        /// <summary>The strings are equal (identical): "Left equal to Right"</summary>
        LeqR = 1,

        /// <summary>Left string is longer than Right string: "Left greater than Right"</summary>
        LgtR = 2,

        /// <summary>Left string is shorter than Right string: "Left less than Right"</summary>
        LltR = 4,

        /// <summary>The strings have the same length, but they differ in some characters: "Left different from Right"</summary>
        LdiR = 8

    }



}
