// <copyright file="AdsPageView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.Ad
{
    using System.Collections.Generic;

    /// <summary>
    /// Ads page view model.
    /// </summary>
    public class AdsPageView
    {
        /// <summary>
        /// Gets or sets rental ad view objects list.
        /// </summary>
        public IEnumerable<RentalAdView> RentalAdView { get; set; }

        /// <summary>
        /// Gets or sets page info model.
        /// </summary>
        public PageInfo PageInfo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is load success.
        /// </summary>
        public bool IsSuccess { get; set; }
    }
}
