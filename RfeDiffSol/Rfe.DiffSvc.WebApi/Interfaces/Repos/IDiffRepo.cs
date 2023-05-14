using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rfe.DiffSvc.WebApi.BusinessObjects;



namespace Rfe.DiffSvc.WebApi.Interfaces.Repos
{



    /// <summary>
    /// Defines methods of the diff service repository.
    /// These shall typically be some CRUD operations.
    /// </summary>
    public interface IDiffRepo
    {



        /// <summary>
        /// Gets a list of all <see cref="Diff"/> objects in the repo.
        /// </summary>
        /// <returns>Returns a list of all Diff objects currently stored in the repository.</returns>
        IList<Diff> GetList();



        /// <summary>
        /// Finds all <see cref="Diff"/> objects that match criteria given in the parameter.
        /// </summary>
        /// <param name="criteria">Criteria that has to be matched for those Diffs to be returned by this method.</param>
        /// <returns>Returns a list of Diffs matching the given criteria.</returns>
        //IList<Diff> FindList(Diff diff);
        IList<Diff> FindList(DiffFilter criteria);



        /// <summary>
        /// Tells whether there is at least one <see cref="Diff"/> object matching the given criteria.
        /// </summary>
        /// <param name="criteria">Criteria to check the Diffs with.</param>
        /// <returns>Returns true :-: the exists (at least) one Diff matching the given criteria, false :-: no matching Diff objects found.</returns>
        //bool Exists(Diff diff);
        bool Exists(DiffFilter criteria);



        /// <summary>
        /// Gets a particular <see cref="Diff"/> according to its ID passed over in the parameter.
        /// </summary>
        /// <param name="diff">Should contain an ID of the object to retrieve from the repo.</param>
        /// <returns>Returns Diff with the given ID.</returns>
        Diff Load(Diff diff);



        /// <summary>
        /// Stores a given <see cref="Diff"/> object back to the repo.
        /// The diff object should have its ID set.
        /// That means the object should already exist in the repo and this is an UPDATE operation.
        /// </summary>
        /// <param name="diff">Data to store to the repo.</param>
        void Store(Diff diff);



        /// <summary>
        /// Stores (adds) a given <see cref="Diff"/> object to the repo.
        /// ID of the object should be empty (default value) because this is an INSERT operation,
        /// meaning a new object will be added to the diff repo.
        /// </summary>
        /// <param name="diff">Object to be added to the repo.</param>
        /// <returns>Returns a new Diff object with a new unique ID set. The rest of the object returned is the same as that in the given parameter.</returns>
        Diff Add(Diff diff);



        /// <summary>
        /// Removes a <see cref="Diff"/> object from the repo.
        /// Which object to remove, this shall be inferred from the ID of the Diff passed in the parameter.
        /// This is a DELETE operation from the CRUD quadruplet.
        /// </summary>
        /// <param name="diff">Object to be removed (it is enough to pass an ID of the object to remove).</param>
        void Remove(Diff diff);



    }



}
