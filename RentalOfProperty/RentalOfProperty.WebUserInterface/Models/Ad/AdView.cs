// <copyright file="AdView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.Ad
{
    using System.Collections.Generic;

    /// <summary>
    /// Ad view model.
    /// </summary>
    public class AdView
    {
        /// <summary>
        /// Gets or sets rental ad view model.
        /// </summary>
        public RentalAdView RentalAdView { get; set; }

        /// <summary>
        /// Gets or sets housing photo list.
        /// </summary>
        public IEnumerable<HousingPhotoView> HousingPhotos { get; set; }
    }
}
