// <copyright file="DailySearch.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Models
{
    /// <summary>
    /// Class for daily ads searching.
    /// </summary>
    public class DailySearch : Search
    {
        /// <summary>
        /// Gets or sets start BYN per person price.
        /// </summary>
        public double? StartBYNPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets finish BYN per person price.
        /// </summary>
        public double? FinishBYNPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets start USD per person price.
        /// </summary>
        public double? StartUSDPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets finish USD per person price.
        /// </summary>
        public double? FinishUSDPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets start USD per day price.
        /// </summary>
        public double? StartUSDPricePerDay { get; set; }

        /// <summary>
        /// Gets or sets finish USD per day price.
        /// </summary>
        public double? FinishUSDPricePerDay { get; set; }

        /// <summary>
        /// Gets or sets start BYN per day price.
        /// </summary>
        public double? StartBYNPricePerDay { get; set; }

        /// <summary>
        /// Gets or sets finish BYN per day price.
        /// </summary>
        public double? FinishBYNPricePerDay { get; set; }
    }
}
