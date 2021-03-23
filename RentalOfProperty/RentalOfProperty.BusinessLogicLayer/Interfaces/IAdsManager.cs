// <copyright file="IAdsManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Interfaces
{
    using System.Threading.Tasks;
    using RentalOfProperty.BusinessLogicLayer.Enums;

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
    }
}
