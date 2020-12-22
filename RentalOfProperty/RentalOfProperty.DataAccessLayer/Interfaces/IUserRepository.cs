namespace RentalOfProperty.DataAccessLayer.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using RentalOfProperty.DataAccessLayer.Models;

    /// <summary>
    /// Interface of user repository for data base.
    /// </summary>
    /// <typeparam name="T">Entity class.</typeparam>
    public interface IUserRepository<T>
        where T : class
    {
        /// <summary>
        /// Method for adding item in table.
        /// </summary>
        /// <param name="item">Adding item.</param>
        /// <param name="password">User password.</param>
        /// <param name="role">Role.</param>
        /// <returns>Is successed result.</returns>
        Task<bool> Create(T item, string password, string role);

        /// <summary>
        /// Method for find item in table.
        /// </summary>
        /// <param name="id">Primary key.</param>
        /// <returns>Result item.</returns>
        Task<T> FindById(string id);

        /// <summary>
        /// Method for getting all items from table.
        /// </summary>
        /// <returns>All items.</returns>
        IEnumerable<T> Get();

        /// <summary>
        /// Method for getting items from table with some filter.
        /// </summary>
        /// <param name="predicate">Search filter.</param>
        /// <returns>Result items.</returns>
        IEnumerable<T> Get(Func<T, bool> predicate);

        /// <summary>
        /// Metohd for removing an item from table.
        /// </summary>
        /// <param name="item">Removing item.</param>
        /// <returns>Void return.</returns>
        Task<IdentityResult> Remove(T item);

        /// <summary>
        /// Method for updating item.
        /// </summary>
        /// <param name="item">Updating item.</param>
        /// <returns>Void return.</returns>
        Task<IdentityResult> Update(T item);
    }
}
