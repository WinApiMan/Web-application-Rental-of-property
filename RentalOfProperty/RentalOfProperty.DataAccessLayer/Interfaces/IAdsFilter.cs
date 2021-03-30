// <copyright file="IAdsFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Interfaces
{
    using System.Collections.Generic;
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
        /// Get rental ads count.
        /// </summary>
        /// <returns>Ads count.</returns>
        int GetRentalAdsCount();
    }
}
