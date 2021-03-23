// <copyright file="Ad.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Ad model.
    /// </summary>
    public class Ad
    {
        /// <summary>
        /// Gets or sets rental ad data model.
        /// </summary>
        public RentalAd RentalAd { get; set; }

        /// <summary>
        /// Gets or sets contact person model.
        /// </summary>
        public ContactPerson ContactPerson { get; set; }

        /// <summary>
        /// Gets or sets housing photo models.
        /// </summary>
        public IEnumerable<HousingPhoto> HousingPhotos { get; set; }

        /// <summary>
        /// Gets or sets aditional ad data model.
        /// </summary>
        public AditionalAdData AditionalAdData { get; set; }
    }
}
