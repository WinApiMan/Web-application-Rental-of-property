// <copyright file="LongTermRentalAdDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Models
{
    /// <summary>
    /// Long term ad model.
    /// </summary>
    public class LongTermRentalAdDTO : RentalAdDTO
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
