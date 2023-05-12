using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.DiffSvc.WebApi.BusinessObjects;



namespace Rfe.DiffSvc.WebApi.Interfaces.Services
{



    /// <summary>
    /// Defines methods needed to implement the entire diff service, i.e.:
    /// <list type="bullet">
    /// <item>A method to generate a new unique ID.</item>
    /// <item>A method to store an input character stream.</item>
    /// <item>A method to get a particular <see cref="Diff"/> object from the in-memory repo. </item>
    /// <item>A method to calculate the diff between 1st stream (left) and 2nd stream (right).</item>
    /// <item>A method to return the result of the diff operation.</item>
    /// </list>
    /// </summary>
    public interface IDiffService
    {
    }



}
