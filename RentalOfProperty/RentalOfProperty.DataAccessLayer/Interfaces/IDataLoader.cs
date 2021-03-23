// <copyright file="IDataLoader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Interfaces
{
    using RentalOfProperty.DataAccessLayer.Enums;

    /// <summary>
    /// Data loader interface.
    /// </summary>
    public interface IDataLoader
    {
        /// <summary>
        /// Load data in ads.
        /// </summary>
        /// <param name="rentalAdMenu">Rental ad menu enum.</param>
        void LoadDataInAds(RentalAdMenu rentalAdMenu);
    }
}
