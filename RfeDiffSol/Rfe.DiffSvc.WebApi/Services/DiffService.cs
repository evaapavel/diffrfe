using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.DiffSvc.WebApi.Interfaces.Services;
using Rfe.DiffSvc.WebApi.Interfaces.Repos;
using Rfe.DiffSvc.WebApi.BusinessObjects;



namespace Rfe.DiffSvc.WebApi.Services
{



    /// <summary>
    /// Basic implementation of the <see cref="IDiffService"/> interface.
    /// Provides core functionality to the entire RFE diff service.
    /// </summary>
    public class DiffService : IDiffService
    {



        // ---------------------------------------------------------------------------------------------------------------
        // Documentation of the public methods to be implemented is provided in the definition of the IDiffService interface.
        // ---------------------------------------------------------------------------------------------------------------



        // Repository dependency.
        private readonly IDiffRepo _diffRepo;



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="diffRepo">A repository object.</param>
        public DiffService(IDiffRepo diffRepo)
        {
            _diffRepo = diffRepo;
        }



        // Get an ID for a new diff.
        public Guid GenerateId()
        {
            return Guid.Empty;
        }



        // Save left/right input data to the repo.
        public void SaveInput(Guid id, StreamInput streamInput, DiffOperandPosition position)
        {
        }



        // Calculate a diff.
        public void CalculateDiff(Guid id)
        {
        }



        // Get output of a diff.
        public DiffOutput GetOutput(Guid id)
        {
            return null;
        }



        // Get Diff data.
        public Diff GetDiff(Guid id)
        {
            return null;
        }



        // Save Diff data.
        public Diff SaveDiff(Diff diff)
        {
            return null;
        }



    }



}
