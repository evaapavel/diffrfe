using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.DiffSvc.WebApi.Interfaces.Services;
using Rfe.DiffSvc.WebApi.Interfaces.Repos;
using Rfe.DiffSvc.WebApi.BusinessObjects;
using Rfe.DiffSvc.WebApi.Exceptions.Services;



namespace Rfe.DiffSvc.WebApi.Services
{



    /// <summary>
    /// Basic implementation of the <see cref="IDiffService"/> interface.
    /// Provides core functionality to the entire RFE diff service.
    /// Note: If a "regular" database were used by the service, then methods in this class should be wrapped in a db transaction.
    /// </summary>
    public class DiffService : IDiffService
    {



        // ---------------------------------------------------------------------------------------------------------------
        // Documentation of the public methods to be implemented is provided in the definition of the IDiffService interface.
        // ---------------------------------------------------------------------------------------------------------------



        // Repository dependency.
        private readonly IDiffRepo _diffRepo;

        // Compare service dependency.
        private readonly ICompareService _compareService;



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="diffRepo">A repository object.</param>
        /// <param name="compareService">A service exposing a method to compare strings.</param>
        public DiffService(IDiffRepo diffRepo, ICompareService compareService)
        {
            _diffRepo = diffRepo;
            _compareService = compareService;
        }



        // **************************************************************************************************
        // **************************************************************************************************
        // **************************************************************************************************



        // Get an ID for a new diff.
        public Guid GenerateId()
        {

            // Prepare an empty Diff.
            Diff diff = new Diff();

            // Try to store it in the repo.
            Diff diffToReturn = _diffRepo.Add(diff);

            // Use the ID of the object returned by the "add" operation as a result.
            return diffToReturn.ID;

        }



        // Save left/right input data to the repo.
        public void SaveInput(Guid id, StreamInput streamInput, DiffOperandPosition position)
        {

            // Get a Diff from the repo (using the given ID).
            Diff wrapper = new Diff { ID = id };
            Diff diff = _diffRepo.Load(wrapper);

            // Integrity check: The input stream on the given position (left/right) must be empty for the given ID.
            //switch (position)
            //{
            //    case DiffOperandPosition.Left:
            //        if (diff.Left != null)
            //        {
            //            throw new InputAlreadySetException(id, position, $"The left input data should have been empty for ID={id}");
            //        }
            //        break;

            //    case DiffOperandPosition.Right:
            //        if (diff.Right != null)
            //        {
            //            throw new InputAlreadySetException(id, position, $"The right input data should have been empty for ID={id}");
            //        }
            //        break;
            //}
            if (diff.Input[position] != null)
            {
                throw new InputAlreadySetException(id, position, $"The {position.ToString().ToLower()} input data should have been empty for ID={id}");
            }

#if DEBUG
            // Integrity check: The output must be empty too.
            if (diff.Output != null)
            {
                throw new Exception($"Cannot set input data for a Diff which has results already (ID={id}).");
            }
#endif

            // Store the input data in the respective part of the Diff object loaded.
            //switch (position)
            //{
            //    case DiffOperandPosition.Left:
            //        diff.Left = streamInput;
            //        break;

            //    case DiffOperandPosition.Right:
            //        diff.Right = streamInput;
            //        break;
            //}
            diff.Input[position] = streamInput;

            // Save the Diff back to the repo.
            _diffRepo.Store(diff);

        }



        // Calculate a diff.
        public void CalculateDiff(Guid id)
        {

            // Get a Diff from the repo.
            Diff wrapper = new Diff { ID = id };
            Diff diff = _diffRepo.Load(wrapper);

            // Integrity check: Both the Left and Right input data must be filled before we can launch the calculation of diff.
            if (diff.Left == null)
            {
                throw new MissingInputException(id, DiffOperandPosition.Left, $"The left input stream of the Diff ID={id} has not been set.");
            }
            if (diff.Right == null)
            {
                throw new MissingInputException(id, DiffOperandPosition.Right, $"The right input stream of the Diff ID={id} has not been set.");
            }

            // Integrity check: The output must be empty.
            if (diff.Output != null)
            {
                throw new OutputAlreadySetException(id, $"The diff for the Diff ID={id} has already been calculated.");
            }

            // Do calculate the diff.
            DiffOutput output = DoCalculateDiff(diff.Left, diff.Right);

            // Store the result into the Diff object.
            diff.Output = output;

            // Save the result back to the repo.
            _diffRepo.Store(diff);

        }



        // Get output of a diff.
        public DiffOutput GetOutput(Guid id)
        {

            // Get a Diff from the repo.
            Diff wrapper = new Diff { ID = id };
            Diff diff = _diffRepo.Load(wrapper);

            // Integrity check: Both the Left and Right input data as well as the output data must be filled before we can return an output.
            if (diff.Left == null)
            {
                throw new MissingInputException(id, DiffOperandPosition.Left, $"The left input stream of the Diff ID={id} has not been set.");
            }
            if (diff.Right == null)
            {
                throw new MissingInputException(id, DiffOperandPosition.Right, $"The right input stream of the Diff ID={id} has not been set.");
            }
            if (diff.Output == null)
            {
                throw new MissingOutputException(id, $"The diff calculation for the Diff ID={id} has not been completed yet.");
            }

            // Return the diff output.
            return diff.Output;

        }



        // Get Diff data.
        // This is just helper method. Not required for the current assignment.
        // It actually wraps the functionality implemented in the repo.
        public Diff GetDiff(Guid id)
        {
            // Get a requested Diff from the repo.
            Diff wrapper = new Diff { ID = id };
            Diff diff = _diffRepo.Load(wrapper);

            // return the Diff back to the caller.
            return diff;
        }



        // Save Diff data.
        // This is just helper method. Not required for the current assignment.
        // It actually wraps the functionality implemented in the repo.
        public Diff SaveDiff(Diff diff)
        {
            // Prepare a return value.
            Diff diffToReturn = null;

            // Choose the "save" method: Either CREATE or UPDATE.
            if (diff.ID == Guid.Empty)
            {
                // The ID has not been filled yet.
                // ---> CREATE
                diffToReturn = _diffRepo.Add(diff);
            }
            else
            {
                // An existing ID has been passed in the parameter.
                // ---> UPDATE
                _diffRepo.Store(diff);
                diffToReturn = diff;
            }

            // Return the result.
            return diffToReturn;
        }



        // **************************************************************************************************
        // **************************************************************************************************
        // **************************************************************************************************



        // This is the ultimate implementation of the diff operation.
        private DiffOutput DoCalculateDiff(StreamInput left, StreamInput right)
        {

            // Prepare a resulting object.
            DiffOutput output = new DiffOutput();

            // Use the compare service to compare inputs.
            int compareResult;
            List<StringSection> diffSectionList;
            _compareService.CompareAndFindDiffs(left.Input, right.Input, out compareResult, out diffSectionList);

            // "Decode" the comparison results.

            // Check for the status: Left greater than Right.
            if (compareResult > 0)
            {
                output.Result = DiffResult.LgtR;
                return output;
            }

            // Check for the status: Left less than Right.
            if (compareResult < 0)
            {
                output.Result = DiffResult.LltR;
                return output;
            }

            // In case the lengths are equal, we have to check the list of diff sections.

            // If the difference list is null or empty, then the inputs are identical (totally equal).
            // If there is at least one "diff section", then return a "equal lengths, but differences" status.

            // Check for the status: Left equal to Right.
            if ( (diffSectionList == null) || (diffSectionList.Count == 0) )
            {
                output.Result = DiffResult.LeqR;
                return output;
            }

            // Well, the only remaining option here is: Left different from Right (although having the same length).
            output.Result = DiffResult.LdiR;
            output.DiffSections = diffSectionList.ToArray<StringSection>();

            // Return the result.
            return output;

        }



    }



}
