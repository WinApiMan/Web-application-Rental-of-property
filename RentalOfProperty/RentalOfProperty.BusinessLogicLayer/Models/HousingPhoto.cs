// <copyright file="HousingPhoto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Models
{
    /// <summary>
    /// Housing photo model.
    /// </summary>
    public class HousingPhoto
    {
        /// <summary>
        /// Gets or sets unique key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets rental ad unique key.
        /// </summary>
        public int RentalAdId { get; set; }

        /// <summary>
        /// Gets or sets path to photo.
        /// </summary>
        public string PathToPhoto { get; set; }
    }
}
