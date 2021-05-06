// <copyright file="DailyRentalAdView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.Ad
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Daily rental ad view model.
    /// </summary>
    public class DailyRentalAdView : RentalAdView
    {
        /// <summary>
        /// Gets or sets BYN per person price.
        /// </summary>
        [Display(Name = "BYNPricePerPerson")]
        public double BYNPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets USD per person price.
        /// </summary>
        [Display(Name = "USDPricePerPerson")]
        public double USDPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets USD per day price.
        /// </summary>
        [Display(Name = "USDPricePerDay")]
        public double USDPricePerDay { get; set; }

        /// <summary>
        /// Gets or sets BYN per day price.
        /// </summary>
        [Display(Name = "BYNPricePerDay")]
        public double BYNPricePerDay { get; set; }
    }
}
