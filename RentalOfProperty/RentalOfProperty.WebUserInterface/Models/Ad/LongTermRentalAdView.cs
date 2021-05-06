// <copyright file="LongTermRentalAdView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.Ad
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Long term ad view model.
    /// </summary>
    public class LongTermRentalAdView : RentalAdView
    {
        /// <summary>
        /// Gets or sets BYN price.
        /// </summary>
        [Display(Name = "BYNPrice")]
        public double BYNPrice { get; set; }

        /// <summary>
        /// Gets or sets USD price.
        /// </summary>
        [Display(Name = "USDPrice")]
        public double USDPrice { get; set; }
    }
}
