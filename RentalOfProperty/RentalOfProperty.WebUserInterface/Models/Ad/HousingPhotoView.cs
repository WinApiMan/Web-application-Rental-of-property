// <copyright file="HousingPhotoView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.Ad
{
    /// <summary>
    /// Housing photo view model.
    /// </summary>
    public class HousingPhotoView
    {
        /// <summary>
        /// Gets or sets unique key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets rental ad unique key.
        /// </summary>
        public string RentalAdId { get; set; }

        /// <summary>
        /// Gets or sets path to photo.
        /// </summary>
        public string PathToPhoto { get; set; }
    }
}
