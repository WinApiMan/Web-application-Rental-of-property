// <copyright file="LongTermSearchView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.Ad
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class for long term ads searching.
    /// </summary>
    public class LongTermSearchView : SearchView
    {
        /// <summary>
        /// Gets or sets start BYN price.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "MoneyError")]
        [Display(Name = "StartBYNPrice")]
        public double? StartBYNPrice { get; set; }

        /// <summary>
        /// Gets or sets finish BYN price.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "MoneyError")]
        [Display(Name = "FinishBYNPrice")]
        public double? FinishBYNPrice { get; set; }

        /// <summary>
        /// Gets or sets start USD price.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "MoneyError")]
        [Display(Name = "StartUSDPrice")]
        public double? StartUSDPrice { get; set; }

        /// <summary>
        /// Gets or sets finish USD price.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "MoneyError")]
        [Display(Name = "FinishUSDPrice")]
        public double? FinishUSDPrice { get; set; }

        /// <summary>
        /// Gets or sets start total area.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "AreaError")]
        [Display(Name = "StartTotalArea")]
        public double? StartTotalArea { get; set; }

        /// <summary>
        /// Gets or sets finish total area.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "AreaError")]
        [Display(Name = "FinishTotalArea")]
        public double? FinishTotalArea { get; set; }

        /// <summary>
        /// Gets or sets start living area.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "AreaError")]
        [Display(Name = "StartLivingArea")]
        public double? StartLivingArea { get; set; }

        /// <summary>
        /// Gets or sets finish living area.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "AreaError")]
        [Display(Name = "FinishLivingArea")]
        public double? FinishLivingArea { get; set; }

        /// <summary>
        /// Gets or sets start kitchen area.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "AreaError")]
        [Display(Name = "StartKitchenArea")]
        public double? StartKitchenArea { get; set; }

        /// <summary>
        /// Gets or sets finish kitchen area.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "AreaError")]
        [Display(Name = "FinishKitchenArea")]
        public double? FinishKitchenArea { get; set; }

        /// <summary>
        /// Gets or sets start land area.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "AreaError")]
        [Display(Name = "StartLandArea")]
        public double? StartLandArea { get; set; }

        /// <summary>
        /// Gets or sets start land area.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "AreaError")]
        [Display(Name = "FinishLandArea")]
        public double? FinishLandArea { get; set; }
    }
}
