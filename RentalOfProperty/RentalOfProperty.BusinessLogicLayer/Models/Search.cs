// <copyright file="Search.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Models
{
    /// <summary>
    /// Class for ads searching.
    /// </summary>
    public class Search
    {
        /// <summary>
        /// Gets or sets region.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets district.
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// Gets or sets locality.
        /// </summary>
        public string Locality { get; set; }

        /// <summary>
        /// Gets or sets address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets start rooms count.
        /// </summary>
        public int? StartRoomsCount { get; set; }

        /// <summary>
        /// Gets or sets finish rooms count.
        /// </summary>
        public int? FinishRoomsCount { get; set; }

        /// <summary>
        /// Gets or sets start current floor.
        /// </summary>
        public int? StartCurrentFloor { get; set; }

        /// <summary>
        /// Gets or sets finish current floor.
        /// </summary>
        public int? FinishCurrentFloor { get; set; }

        /// <summary>
        /// Gets or sets start total floor.
        /// </summary>
        public int? StartTotalFloor { get; set; }

        /// <summary>
        /// Gets or sets finish total floor.
        /// </summary>
        public int? FinishTotalFloor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is first floor.
        /// </summary>
        public bool IsFirstFloor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is first floor.
        /// </summary>
        public bool IsLastFloor { get; set; }
    }
}
