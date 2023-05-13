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
    /// <item>A method to calculate the diff between 1st stream (left) and 2nd stream (right).</item>
    /// <item>A method to return the result of the diff operation.</item>
    /// <item>A method to get a particular <see cref="Diff"/> object from the in-memory repo. </item>
    /// <item>A method to save a particular <see cref="Diff"/> object to the in-memory repo. </item>
    /// </list>
    /// </summary>
    public interface IDiffService
    {



        /// <summary>
        /// Gets an ID to be used for subsequent one-particular-diff-related requests.
        /// </summary>
        /// <returns>Returns a new ID to be used when posting input data and getting output data for a particular diff.</returns>
        Guid GenerateId();



        /// <summary>
        /// Saves one input for a particular diff given its ID.
        /// </summary>
        /// <param name="id">ID of the diff the data is posted for.</param>
        /// <param name="streamInput">Input stream data itself.</param>
        /// <param name="position">Tell whether this is 1st text stream (left), or 2nd one (right).</param>
        void SaveInput(Guid id, StreamInput streamInput, DiffOperandPosition position);



        /// <summary>
        /// Forces to calculate the diff between the left input and the right input.
        /// Results are stored in the corresponding Diff object.
        /// </summary>
        /// <param name="id">ID of the diff to calculate.</param>
        void CalculateDiff(Guid id);



        /// <summary>
        /// Gets the output of a particular diff given its ID.
        /// </summary>
        /// <param name="id">ID of the diff to get the output for.</param>
        /// <returns>Returns the output of the given diff.</returns>
        DiffOutput GetOutput(Guid id);



        /// <summary>
        /// Gets a whole <see cref="Diff"/> object (including its inputs and output) given its ID.
        /// </summary>
        /// <param name="id">ID of the Diff to get.</param>
        /// <returns>Returns a Diff object requested.</returns>
        Diff GetDiff(Guid id);



        /// <summary>
        /// Saves complete <see cref="Diff"/> data to the repo.
        /// Depending on the ID property of the given Diff object, this method may result
        /// either in a CREATE operation (in case the ID is "empty"),
        /// or in an UPDATE operation (if the ID refers to an existing object in the database already).
        /// </summary>
        /// <param name="diff">The Diff object to save.</param>
        /// <returns>Returns (possibly) another Diff object with the same data, but with the ID always set (in cases where new data was sent to the database - INSERT).</returns>
        Diff SaveDiff(Diff diff);



    }



}
