// <copyright file="DailyRentalAd.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Models
{
    /// <summary>
    /// Daily rental ad model.
    /// </summary>
    public class DailyRentalAd : RentalAd
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
