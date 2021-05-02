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
        /// Get ads with predicate for page.
        /// </summary>
        /// <param name="adsTypeMenuItem">Ads type menu item.</param>
        /// <param name="pageNumber">Current page number.</param>
        /// <param name="pageSize">Ads count in page.</param>
        /// <returns>Ads list.</returns>
        Task<IEnumerable<RentalAd>> GetAdsForPage(AdsTypeMenu adsTypeMenuItem, int pageNumber, int pageSize);

        /// <summary>
        /// Get rental ads count.
        /// </summary>
        /// <returns>Ads count.</returns>
        int GetRentalAdsCount();

        /// <summary>
        /// Get rental ads count with predicate.
        /// </summary>
        /// <param name="adsTypeMenuItem">Ads type menu item.</param>
        /// <returns>Ads count.</returns>
        int GetRentalAdsCount(AdsTypeMenu adsTypeMenuItem);

        /// <summary>
        /// Get housing photos by rental ad id.
        /// </summary>
        /// <param name="id">Rental ad id.</param>
        /// <returns>Housing photo list.</returns>
        Task<IEnumerable<HousingPhoto>> GetHousingPhotosByRentalAdId(string id);

        /// <summary>
        /// Method add to favourites if isFavorite==false or remove if isFavorite==true.
        /// </summary>
        /// <param name="userId">User unique key.</param>
        /// <param name="rentalAdId">Ads unique key.</param>
        /// <returns>Action result.</returns>
        Task AddOrRemoveFavorite(string userId, string rentalAdId);

        /// <summary>
        /// Get user favorite ads.
        /// </summary>
        /// <param name="userId">User unique key.</param>
        /// <returns>Favorite ads list.</returns>
        Task<IEnumerable<UserRentalAd>> GetUserFavoriteAds(string userId);

        /// <summary>
        /// Get user favorite ads.
        /// </summary>
        /// <param name="userId">User unique key.</param>
        /// <returns>Favorite ads list.</returns>
        Task<IEnumerable<RentalAd>> GetFavoriteAds(string userId);

        /// <summary>
        /// Search long term ads use parametrs.
        /// </summary>
        /// <param name="longTermSearch">Parametrs for searching.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Result ads.</returns>
        Task<IEnumerable<RentalAd>> LongTermSearch(LongTermSearch longTermSearch, int pageNumber, int pageSize);

        /// <summary>
        /// Search daily ads use parametrs.
        /// </summary>
        /// <param name="dailySearch">Parametrs for searching.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Result ads.</returns>
        Task<IEnumerable<RentalAd>> DailySearch(DailySearch dailySearch, int pageNumber, int pageSize);

        /// <summary>
        /// Get daily search count.
        /// </summary>
        /// <param name="dailySearch">Daily search parametrs.</param>
        /// <returns>Count.</returns>
        int GetDailySearchCount(DailySearch dailySearch);

        /// <summary>
        /// Get long term search count.
        /// </summary>
        /// <param name="longTermSearch">Long term search parametrs.</param>
        /// <returns>Count.</returns>
        int GetLongTermSearchCount(LongTermSearch longTermSearch);

        /// <summary>
        /// Get rental ad by id.
        /// </summary>
        /// <param name="id">Unique key.</param>
        /// <returns>Rental ad.</returns>
        Task<AllRentalAdInfo> GetAllRentalAdInfo(string id);
    }
}
