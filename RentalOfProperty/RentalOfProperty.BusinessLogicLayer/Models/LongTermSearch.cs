// <copyright file="LongTermSearch.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Models
{
    /// <summary>
    /// Class for long term ads searching.
    /// </summary>
    public class LongTermSearch : Search
    {
        /// <summary>
        /// Gets or sets start BYN price.
        /// </summary>
        public double? StartBYNPrice { get; set; }

        /// <summary>
        /// Gets or sets finish BYN price.
        /// </summary>
        public double? FinishBYNPrice { get; set; }

        /// <summary>
        /// Gets or sets start USD price.
        /// </summary>
        public double? StartUSDPrice { get; set; }

        /// <summary>
        /// Gets or sets finish USD price.
        /// </summary>
        public double? FinishUSDPrice { get; set; }

        /// <summary>
        /// Gets or sets start total area.
        /// </summary>
        public double? StartTotalArea { get; set; }

        /// <summary>
        /// Gets or sets finish total area.
        /// </summary>
        public double? FinishTotalArea { get; set; }

        /// <summary>
        /// Gets or sets start living area.
        /// </summary>
        public double? StartLivingArea { get; set; }

        /// <summary>
        /// Gets or sets finish living area.
        /// </summary>
        public double? FinishLivingArea { get; set; }

        /// <summary>
        /// Gets or sets start kitchen area.
        /// </summary>
        public double? StartKitchenArea { get; set; }

        /// <summary>
        /// Gets or sets finish kitchen area.
        /// </summary>
        public double? FinishKitchenArea { get; set; }

        /// <summary>
        /// Gets or sets start land area.
        /// </summary>
        public double? StartLandArea { get; set; }

        /// <summary>
        /// Gets or sets start land area.
        /// </summary>
        public double? FinishLandArea { get; set; }
    }
}
