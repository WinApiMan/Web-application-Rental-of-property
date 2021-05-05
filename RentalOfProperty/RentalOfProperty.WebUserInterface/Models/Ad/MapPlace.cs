// <copyright file="MapPlace.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.Ad
{
    /// <summary>
    /// Class for google map.
    /// </summary>
    public class MapPlace
    {
        /// <summary>
        /// Gets or sets unique key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets x map coordinate.
        /// </summary>
        public double GeoLong { get; set; }

        /// <summary>
        /// Gets or sets y map coordinate.
        /// </summary>
        public double GeoLat { get; set; }
    }
}
