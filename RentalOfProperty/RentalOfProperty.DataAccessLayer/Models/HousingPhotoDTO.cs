// <copyright file="HousingPhotoDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Models
{
    /// <summary>
    /// Housing photo model.
    /// </summary>
    public class HousingPhotoDTO
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
