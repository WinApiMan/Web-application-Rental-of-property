// <copyright file="IDataLoader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Interfaces
{
    using System.Collections.Generic;
    using RentalOfProperty.DataAccessLayer.Enums;
    using RentalOfProperty.DataAccessLayer.Models;

    /// <summary>
    /// Data loader interface.
    /// </summary>
    public interface IDataLoader
    {
        /// <summary>
        /// Gets or sets ad list.
        /// </summary>
        public List<AdDTO> Ads { get; set; }

        /// <summary>
        /// Load data in ads.
        /// </summary>
        /// <param name="rentalAdMenu">Rental ad menu enum.</param>
        void LoadDataInAds(RentalAdMenu rentalAdMenu);
    }
}
