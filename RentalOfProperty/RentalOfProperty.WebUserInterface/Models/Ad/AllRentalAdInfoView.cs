// <copyright file="AllRentalAdInfoView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models
{
    using System.Collections.Generic;
    using RentalOfProperty.WebUserInterface.Models.Ad;

    /// <summary>
    /// All rental ad.
    /// </summary>
    public class AllRentalAdInfoView
    {
        /// <summary>
        /// Gets or sets rental ad info.
        /// </summary>
        public RentalAdView RentalAd { get; set; }

        /// <summary>
        /// Gets or sets aditional rental ad info.
        /// </summary>
        public AditionalAdDataView AditionalAdData { get; set; }

        /// <summary>
        /// Gets or sets contact repson info.
        /// </summary>
        public ContactPersonView ContactPerson { get; set; }

        /// <summary>
        /// Gets or sets housing photos.
        /// </summary>
        public IEnumerable<HousingPhotoView> Photos { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is ad create on this site.
        /// </summary>
        public bool IsOriginal { get; set; }
    }
}
