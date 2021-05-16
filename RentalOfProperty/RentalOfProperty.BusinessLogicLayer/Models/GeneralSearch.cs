// <copyright file="GeneralSearch.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Models
{
    /// <summary>
    /// General search model.
    /// </summary>
    public class GeneralSearch
    {
        /// <summary>
        /// Gets or sets locality.
        /// </summary>
        public string Locality { get; set; }

        /// <summary>
        /// Gets or sets start rooms count.
        /// </summary>
        public int? RoomsCount { get; set; }

        /// <summary>
        /// Gets or sets start total area.
        /// </summary>
        public double? StartArea { get; set; }

        /// <summary>
        /// Gets or sets finish total area.
        /// </summary>
        public double? FinishArea { get; set; }

        /// <summary>
        /// Gets or sets start USD price.
        /// </summary>
        public double? StartUSDPrice { get; set; }

        /// <summary>
        /// Gets or sets finish USD price.
        /// </summary>
        public double? FinishUSDPrice { get; set; }
    }
}
