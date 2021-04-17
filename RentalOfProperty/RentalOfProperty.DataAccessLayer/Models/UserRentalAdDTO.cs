// <copyright file="UserRentalAdDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Models
{
    /// <summary>
    /// User rental ad dto object for intermediate table.
    /// </summary>
    public class UserRentalAdDTO
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
