// <copyright file="IAdsManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RentalOfProperty.BusinessLogicLayer.Enums;
    using RentalOfProperty.BusinessLogicLayer.Models;

    /// <summary>
    /// Ads manager interface.
    /// </summary>
    public interface IAdsManager
    {
        /// <summary>
        /// Load long term ads from GoHome.by site.
        /// </summary>
        /// <param name="loadDataFromSourceMenuItem">Menu item.</param>
        /// <returns>Task result.</returns>
        Task LoadLongTermAdsFromGoHomeBy(LoadDataFromSourceMenu loadDataFromSourceMenuItem);

        /// <summary>
        /// Gets ads for page.
        /// </summary>
        /// <param name="pageNumber">Current page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Ads list.</returns>
        Task<IEnumerable<RentalAd>> GetAdsForPage(int pageNumber, int pageSize);

        /// <summary>
        /// Get rental ads count.
        /// </summary>
        /// <returns>Ads count.</returns>
        int GetRentalAdsCount();

        /// <summary>
        /// Get housing photos by rental ad id.
        /// </summary>
        /// <param name="id">Rental ad id.</param>
        /// <returns>Housing photo list.</returns>
        Task<IEnumerable<HousingPhoto>> GetHousingPhotosByRentalAdId(string id);
    }
}
