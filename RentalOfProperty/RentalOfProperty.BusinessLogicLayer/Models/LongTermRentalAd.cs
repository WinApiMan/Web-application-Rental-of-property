// <copyright file="LongTermRentalAd.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Models
{
    /// <summary>
    /// Long term ad model.
    /// </summary>
    public class LongTermRentalAd : RentalAd
    {
        /// <summary>
        /// Gets or sets BYN price.
        /// </summary>
        public double BYNPrice { get; set; }

        /// <summary>
        /// Gets or sets USD price.
        /// </summary>
        public double USDPrice { get; set; }
    }
}
