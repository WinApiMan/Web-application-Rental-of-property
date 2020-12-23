// <copyright file="IUsersManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RentalOfProperty.BusinessLogicLayer.Models;

    /// <summary>
    /// User manager interface.
    /// </summary>
    public interface IUsersManager
    {
        /// <summary>
        /// Method for adding item in table.
        /// </summary>
        /// <param name="item">Adding item.</param>
        /// <param name="role">User role.</param>
        /// <returns>Void return.</returns>
        Task<IdentityResult> Create(User item, string role);

        /// <summary>
        /// Method for find item in table.
        /// </summary>
        /// <param name="id">Primary key.</param>
        /// <returns>Result item.</returns>
        Task<User> FindById(string id);

        /// <summary>
        /// Method for getting all items from table.
        /// </summary>
        /// <returns>All items.</returns>
        Task<IEnumerable<User>> Get();

        /// <summary>
        /// Method for getting items from table with some filter.
        /// </summary>
        /// <param name="predicate">Search filter.</param>
        /// <returns>Result items.</returns>
        Task<IEnumerable<User>> Get(Func<User, bool> predicate);

        /// <summary>
        /// Metohd for removing an item from table.
        /// </summary>
        /// <param name="item">Removing item.</param>
        /// <returns>Void return.</returns>
        Task Remove(User item);

        /// <summary>
        /// Method for updating item.
        /// </summary>
        /// <param name="item">Updating item.</param>
        /// <returns>Void return.</returns>
        Task Update(User item);
    }
}
