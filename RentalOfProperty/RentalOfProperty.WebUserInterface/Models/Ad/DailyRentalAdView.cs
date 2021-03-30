// <copyright file="DailyRentalAdViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.Ad
{
    /// <summary>
    /// Daily rental ad view model.
    /// </summary>
    public class DailyRentalAdView : RentalAdView
    {
        /// <summary>
        /// Gets or sets BYN per person price.
        /// </summary>
        public double BYNPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets USD per person price.
        /// </summary>
        public double USDPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets USD per day price.
        /// </summary>
        public double USDPricePerDay { get; set; }

        /// <summary>
        /// Gets or sets BYN per day price.
        /// </summary>
        public double BYNPricePerDay { get; set; }
    }
}
