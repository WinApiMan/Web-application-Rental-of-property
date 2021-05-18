// <copyright file="GeneralSearchView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.Ad
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// General search.
    /// </summary>
    public class GeneralSearchView
    {
        /// <summary>
        /// Gets or sets locality.
        /// </summary>
        [StringLength(200, ErrorMessage = "LocalityLengthError")]
        [Display(Name = "Locality")]
        public string Locality { get; set; }

        /// <summary>
        /// Gets or sets start rooms count.
        /// </summary>
        [Range(1, 1000, ErrorMessage = "RoomError")]
        [Display(Name = "RoomsCount")]
        public int? RoomsCount { get; set; }

        /// <summary>
        /// Gets or sets start total area.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "AreaError")]
        [Display(Name = "StartArea")]
        public double? StartArea { get; set; }

        /// <summary>
        /// Gets or sets finish total area.
        /// </summary>
        [Range(1, 1000000.0, ErrorMessage = "AreaError")]
        [Display(Name = "FinishArea")]
        public double? FinishArea { get; set; }

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
        /// Gets or sets a value indicating whether gets or sets long term.
        /// </summary>
        [Display(Name = "LongTerm")]
        public bool LongTerm { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets daily.
        /// </summary>
        [Display(Name = "Daily")]
        public bool Daily { get; set; }
    }
}
