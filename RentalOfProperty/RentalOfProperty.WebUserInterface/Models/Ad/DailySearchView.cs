// <copyright file="DailySearchView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.Ad
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class for daily ads searching.
    /// </summary>
    public class DailySearchView : SearchView
    {
        /// <summary>
        /// Gets or sets start BYN per person price.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "MoneyError")]
        [Display(Name = "StartBYNPricePerPerson")]
        public double? StartBYNPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets finish BYN per person price.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "MoneyError")]
        [Display(Name = "FinishBYNPricePerPerson")]
        public double? FinishBYNPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets start USD per person price.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "MoneyError")]
        [Display(Name = "StartUSDPricePerPerson")]
        public double? StartUSDPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets finish USD per person price.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "MoneyError")]
        [Display(Name = "FinishUSDPricePerPerson")]
        public double? FinishUSDPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets start USD per day price.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "MoneyError")]
        [Display(Name = "StartUSDPricePerDay")]
        public double? StartUSDPricePerDay { get; set; }

        /// <summary>
        /// Gets or sets finish USD per day price.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "MoneyError")]
        [Display(Name = "FinishUSDPricePerDay")]
        public double? FinishUSDPricePerDay { get; set; }

        /// <summary>
        /// Gets or sets start BYN per day price.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "MoneyError")]
        [Display(Name = "StartBYNPricePerDay")]
        public double? StartBYNPricePerDay { get; set; }

        /// <summary>
        /// Gets or sets finish BYN per day price.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "MoneyError")]
        [Display(Name = "FinishBYNPricePerDay")]
        public double? FinishBYNPricePerDay { get; set; }
    }
}
