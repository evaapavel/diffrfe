using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;

using Rfe.DiffSvc.WebApi.Helpers;


using Rfe.DiffSvc.WebApi.Interfaces.Repos;
using Rfe.DiffSvc.WebApi.BusinessObjects;
using Rfe.DiffSvc.WebApi.Exceptions;



namespace Rfe.DiffSvc.WebApi.Repos
{



    /// <summary>
    /// In-memory implementation of the <see cref="IDiffRepo"/> repository interface.
    /// This class should behave as a singleton. It's not necessary to implement the singleton pattern. This is handled by the DI service container.
    /// However, it's our responsibility to implement the repo "thread-safe".
    /// </summary>
    public class DiffRepo : IDiffRepo
    {



        // Documentation to the public methods that have to be implemented is provided in the definition of the IDiffRepo interface.



        // In-memory storage of the Diff objects.
        private IDictionary<Guid, Diff> _allDiffs;



        // Lock object to make the repo methods thread safe.
        private readonly object _lockObject;



        /// <summary>
        /// Constructor.
        /// </summary>
        public DiffRepo()
        {
            _allDiffs = new Dictionary<Guid, Diff>();
            //_allDiffs = new ConcurrentDictionary<Guid, Diff>();

            _lockObject = new object();
        }



        // READ all
        public IList<Diff> GetList()
        {
            IList<Diff> diffs = new List<Diff>();

            lock (_lockObject)
            {
                // Clone all the items in the in-memory database and put them into the resulting list.

                //CopyDiffCollectionToList(_allDiffs.Values, diffs);
                //IList<Diff> diffsToUse = _allDiffs.Values;
                ICollection<Diff> diffsToUse = _allDiffs.Values;
                CopyDiffCollectionToList(diffsToUse, diffs);
            }

            return diffs;
        }



        // READ some
        public IList<Diff> FindList(DiffFilter criteria)
        {
            IList<Diff> diffs = new List<Diff>();

            lock (_lockObject)
            {
                // Prepare a query based on the given criteria.
                IQueryable<Diff> query = BuildQuery(_allDiffs.Values.AsQueryable<Diff>(), criteria);

                // Apply the query and store the results.
                IList<Diff> diffsFound = query.ToList<Diff>();

                // Clone the objects matching the given criteria and use the clones to populate the resulting list.
                CopyDiffCollectionToList(diffsFound, diffs);
            }

            return diffs;
        }



        // Is there any?
        public bool Exists(DiffFilter criteria)
        {
            bool isExisting = false;

            lock (_lockObject)
            {
                // Prepare a query based on the given criteria.
                IQueryable<Diff> query = BuildQuery(_allDiffs.Values.AsQueryable<Diff>(), criteria);

                // Apply the query and check whether there is at least one item matching the criteria.
                isExisting = query.Any<Diff>();
            }

            return isExisting;
        }



        // READ one
        public Diff Load(Diff diff)
        {
            Diff diffToReturn = null;

            lock (_lockObject)
            {

                // Check for the existence of an object with the given ID.
                if ( ! _allDiffs.ContainsKey(diff.ID) )
                {
                    //throw new InvalidOperationException($"There's no item with the requested ID: '{diff.ID}'");
                    throw new NotFoundException(diff, $"There's no item with the requested ID: '{diff.ID}'");
                }

                // Access the requested object.
                Diff diffFound = _allDiffs[diff.ID];

                // Return a clone of the found object.
                diffToReturn = CloneForOutput(diffFound);

            }

            return diffToReturn;
        }



        // UPDATE
        public void Store(Diff diff)
        {
            lock (_lockObject)
            {

                // Make sure the ID of the diff passed in the parameter is not empty.
                if (diff.ID == Guid.Empty)
                {
                    throw new IdNotSetException(diff, "Unable to store data without a valid ID.");
                }

                // Check for the existence of an object with the given ID.
                if (!_allDiffs.ContainsKey(diff.ID))
                {
                    //throw new InvalidOperationException($"There's no item with the requested ID: '{diff.ID}'");
                    throw new NotFoundException(diff, $"There's no item with the requested ID: '{diff.ID}'");
                }

                // Access the requested object.
                Diff diffFound = _allDiffs[diff.ID];

#if DEBUG
                // This should normally not happen.
                if (object.ReferenceEquals(diff, diffFound))
                {
                    throw new Exception($"The parameter and the repo-object should NOT be the same, but they are: {diff}");
                }
#endif

                // Copy data from the parameter to the "repo" object.
                CopyDataFromFront(diff, diffFound, false);

            }
        }



        // INSERT
        public Diff Add(Diff diff)
        {
            Diff diffToReturn = null;

            lock (_lockObject)
            {

                // ID of the new object should be empty.
                if (diff.ID != Guid.Empty)
                {
                    throw new NonEmptyIdException(diff, "Cannot insert an item whose ID has already been set.");
                }

                // Create a new object using the given data (create a clone of the given object).
                Diff diffToCreate = new Diff(diff);

                // Add a new unique ID to the new object.
                diffToCreate.ID = Guid.NewGuid();

                // Add the object to the repo.
                _allDiffs[diffToCreate.ID] = diffToCreate;

                // Return a clone of the new object.
                diffToReturn = CloneForOutput(diffToCreate);

            }

            return diffToReturn;
        }



        // DELETE
        public void Remove(Diff diff)
        {
            lock (_lockObject)
            {

                // Check for the existence of an object with the given ID.
                if (!_allDiffs.ContainsKey(diff.ID))
                {
                    throw new NotFoundException(diff, $"There's no item with the requested ID: '{diff.ID}'");
                }

                // It is not necessary to "access" the object we're just about to remove from the repo:
                // // Access the requested object.
                // Diff diffFound = _allDiffs[diff.ID];

                // Remove the object from the repo.
                _allDiffs.Remove(diff.ID);

            }
        }



        // Copy items of the given source collection to the destination.
        // Make "deep" clones of the items being copied.
        private void CopyDiffCollectionToList(IEnumerable<Diff> source, IList<Diff> dest)
        {
            //((List<Diff>) diffs).AddRange(_allDiffs.Values);
            //foreach (KeyValuePair<Guid, Diff> entry in _allDiffs)
            //{
            ////    diffs.Add(entry.Value);
            //    diffs.Add(entry.Value.Clone());
            //}
            //foreach (Diff diff in _allDiffs.Values)
            foreach (Diff diff in source)
            {
                //diffs.Add(diff);
                //diffs.Add(diff.Clone());
                //dest.Add(diff.Clone());
                dest.Add(CloneForOutput(diff));
            }
        }



        // Clones any diff item from the repo in order to be able to return it to the front end.
        private Diff CloneForOutput(Diff diffFromRepo)
        {
            return diffFromRepo.Clone();
        }



        // Copies data from the source diff (from the "front" layer of the app) to the dest diff (to the "repo" object).
        private void CopyDataFromFront(Diff source, Diff dest, bool includingID)
        {

            // ID
            // Don't know whether this is useful for something...
            if (includingID)
            {
                dest.ID = source.ID;
            }

            // Left
            if (source.Left != null)
            {
                dest.Left = source.Left.Clone();
            }
            else
            {
                dest.Left = null;
            }

            // Right
            if (source.Right != null)
            {
                dest.Right = source.Right.Clone();
            }
            else
            {
                dest.Right = null;
            }

            // Output
            if (source.Output != null)
            {
                dest.Output = source.Output.Clone();
            }
            else
            {
                dest.Output = null;
            }

        }



        // Helps to dynamically build a query when searching for some items in the repo.
        private IQueryable<Diff> BuildQuery(IQueryable<Diff> query, DiffFilter criteria)
        {

            // Handle "HasLeftInput".
            if (criteria.HasLeftInput != null)
            {
                if (criteria.HasLeftInput.Value)
                {
                    query = query.Where(d => d.Left != null);
                }
                else
                {
                    query = query.Where(d => d.Left == null);
                }
            }

            // Handle "HasRightInput".
            if (criteria.HasRightInput != null)
            {
                if (criteria.HasRightInput.Value)
                {
                    query = query.Where(d => d.Right != null);
                }
                else
                {
                    query = query.Where(d => d.Right == null);
                }
            }

            // Handle "HasOutput".
            if (criteria.HasOutput != null)
            {
                if (criteria.HasOutput.Value)
                {
                    query = query.Where(d => d.Output != null);
                }
                else
                {
                    query = query.Where(d => d.Output == null);
                }
            }

            // Handle "Results".
            if (criteria.Results != null)
            {
                // //Expression<Func<Diff, bool>> predicate = new Expression<Func<Diff, bool>>();
                // //predicate = predicate.And
                // //Expression<Func<Diff, bool>> predicate = query.W
                //Func<Diff, bool> pred1 = d => (d.Output != null) && (d.Output.Result == DiffResult.LeqR);
                // //Expression<Func<Diff, bool>> expr1 = pred1;
                //Expression<Func<Diff, bool>> expr1 = d => (d.Output != null) && (d.Output.Result == DiffResult.LeqR);

                Expression<Func<Diff, bool>> exprOutputExist = d => (d.Output != null);

                Expression<Func<Diff, bool>> exprResultLeqR = d => (d.Output.Result == DiffResult.LeqR);
                Expression<Func<Diff, bool>> exprResultLgtR = d => (d.Output.Result == DiffResult.LgtR);
                Expression<Func<Diff, bool>> exprResultLltR = d => (d.Output.Result == DiffResult.LltR);
                Expression<Func<Diff, bool>> exprResultLdiR = d => (d.Output.Result == DiffResult.LdiR);

                Expression<Func<Diff, bool>> exprResult = d => false;

                if ((criteria.Results & DiffResult.LeqR) > 0)
                {
                    exprResult = exprResult.Or(exprResultLeqR);
                }

                if ((criteria.Results & DiffResult.LgtR) > 0)
                {
                    exprResult = exprResult.Or(exprResultLgtR);
                }

                if ((criteria.Results & DiffResult.LltR) > 0)
                {
                    exprResult = exprResult.Or(exprResultLltR);
                }

                if ((criteria.Results & DiffResult.LdiR) > 0)
                {
                    exprResult = exprResult.Or(exprResultLdiR);
                }

                Expression<Func<Diff, bool>> exprFinal = exprOutputExist.And(exprResult);

                query = query.Where(exprFinal);
            }

            // Return the result.
            return query;

        }



    }



}
