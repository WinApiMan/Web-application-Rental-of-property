// <copyright file="AllRentalAdInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// All rental ad.
    /// </summary>
    public class AllRentalAdInfo
    {
        /// <summary>
        /// Gets or sets rental ad info.
        /// </summary>
        public RentalAd RentalAd { get; set; }

        /// <summary>
        /// Gets or sets aditional rental ad info.
        /// </summary>
        public AditionalAdData AditionalAdData { get; set; }

        /// <summary>
        /// Gets or sets contact repson info.
        /// </summary>
        public ContactPerson ContactPerson { get; set; }

        /// <summary>
        /// Gets or sets housing photos.
        /// </summary>
        public IEnumerable<HousingPhoto> Photos { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is ad create on this site.
        /// </summary>
        public bool IsOriginal { get; set; }
    }
}
