// <copyright file="UserRentalAd.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Models
{
    /// <summary>
    /// User rental ad object for intermediate table.
    /// </summary>
    public class UserRentalAd
    {
        /// <summary>
        /// Gets or sets unique number.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets unique rental ad number.
        /// </summary>
        public string RentalAdId { get; set; }

        /// <summary>
        /// Gets or sets unique user number.
        /// </summary>
        public string UserId { get; set; }
    }
}
