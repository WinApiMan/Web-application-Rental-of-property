// <copyright file="IAdsFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for filter ads table.
    /// </summary>
    /// <typeparam name="TEntity">Entity model.</typeparam>
    public interface IAdsFilter<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Get ads for page.
        /// </summary>
        /// <param name="pageNumber">Current page number.</param>
        /// <param name="pageSize">Ads count in page.</param>
        /// <returns>Ads list.</returns>
        Task<IEnumerable<TEntity>> GetAdsForPage(int pageNumber, int pageSize);

        /// <summary>
        /// Get ads with predicate for page.
        /// </summary>
        /// <param name="predicate">Predicate object.</param>
        /// <param name="pageNumber">Current page number.</param>
        /// <param name="pageSize">Ads count in page.</param>
        /// <returns>Ads list.</returns>
        Task<IEnumerable<TEntity>> GetAdsForPage(Expression<Func<TEntity, bool>> predicate, int pageNumber, int pageSize);

        /// <summary>
        /// Get ads with predicate for page.
        /// </summary>
        /// <param name="query">Sql query.</param>
        /// <param name="pageNumber">Current page number.</param>
        /// <param name="pageSize">Ads count in page.</param>
        /// <returns>Ads list.</returns>
        Task<IEnumerable<TEntity>> GetAdsForPage(string query, int pageNumber, int pageSize);

        /// <summary>
        /// Get rental ads count.
        /// </summary>
        /// <returns>Ads count.</returns>
        int GetRentalAdsCount();

        /// <summary>
        /// Get rental ads count with predicate.
        /// </summary>
        /// <param name="predicate">Predicate object.</param>
        /// <returns>Ads count.</returns>
        int GetRentalAdsCount(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get rental ads count with sql query.
        /// </summary>
        /// <param name="query">Sql query string.</param>
        /// <returns>Ads count.</returns>
        int GetRentalAdsCount(string query);
    }
}
