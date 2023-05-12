using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.Diff.Svc.Interfaces.Services;
using Rfe.Diff.Svc.Interfaces.Repos;



namespace Rfe.Diff.Svc.Services
{



    /// <summary>
    /// Basic implementation of the <see cref="IDiffService"/> interface.
    /// Provides core functionality to the entire RFE diff service.
    /// </summary>
    public class DiffService : IDiffService
    {



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



    }



}
