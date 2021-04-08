﻿// <copyright file="IRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface of generic repository for data base.
    /// </summary>
    /// <typeparam name="TEntity">Entity class.</typeparam>
    public interface IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Method for adding item in table.
        /// </summary>
        /// <param name="item">Adding item.</param>
        /// <returns>Void return.</returns>
        Task Create(TEntity item);

        /// <summary>
        /// Add models.
        /// </summary>
        /// <param name="entities">Adding models.</param>
        /// <returns>Task result.</returns>
        Task CreateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Method for getting all items from table.
        /// </summary>
        /// <returns>All items.</returns>
        Task<IEnumerable<TEntity>> Get();

        /// <summary>
        /// Method for getting items from table with some filter.
        /// </summary>
        /// <param name="predicate">Search filter.</param>
        /// <returns>Result items.</returns>
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Metohd for removing an item from table.
        /// </summary>
        /// <param name="item">Removing item.</param>
        /// <returns>Void return.</returns>
        Task Remove(TEntity item);

        /// <summary>
        /// Remove models.
        /// </summary>
        /// <param name="entities">Removing entities.</param>
        /// <returns>Task result.</returns>
        Task RemoveRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Method for updating item.
        /// </summary>
        /// <param name="item">Updating item.</param>
        /// <returns>Void return.</returns>
        Task Update(TEntity item);

        /// <summary>
        /// Update models.
        /// </summary>
        /// <param name="items">Updating models.</param>
        /// <returns>Task result.</returns>
        Task UpdateRange(IEnumerable<TEntity> items);
    }
}
