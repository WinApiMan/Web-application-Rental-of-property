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
        /// Get daily rental ads.
        /// </summary>
        /// <returns>Rental ads list.</returns>
        Task<IEnumerable<DailyRentalAd>> GetDailyRentalAds();

        /// <summary>
        /// Get longTerm rental ads.
        /// </summary>
        /// <returns>Rental ads list.</returns>
        Task<IEnumerable<LongTermRentalAd>> GetLongTermRentalAds();
    }
}
