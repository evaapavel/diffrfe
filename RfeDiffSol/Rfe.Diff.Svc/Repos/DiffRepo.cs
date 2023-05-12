using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.DiffSvc.WebApi.Interfaces.Repos;



namespace Rfe.DiffSvc.WebApi.Repos
{



    /// <summary>
    /// In-memory implementation of the <see cref="IDiffRepo"/> repository interface.
    /// This class should behave as a singleton. It's not necessary to implement the singleton pattern. This is handled by the DI service container.
    /// However, it's our responsibility to implement the repo "thread-safe".
    /// </summary>
    public class DiffRepo : IDiffRepo
    {
    }



}
