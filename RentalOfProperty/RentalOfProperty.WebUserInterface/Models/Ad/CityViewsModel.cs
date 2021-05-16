// <copyright file="CityViewsModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.Ad
{
    /// <summary>
    /// City views model.
    /// </summary>
    public class CityViewsModel
    {
        /// <summary>
        /// Gets or sets city name.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets daily ads views.
        /// </summary>
        public int DailyAdsViews { get; set; }

        /// <summary>
        /// Gets or sets long term ads views.
        /// </summary>
        public int LongTermAdsViews { get; set; }
    }
}
