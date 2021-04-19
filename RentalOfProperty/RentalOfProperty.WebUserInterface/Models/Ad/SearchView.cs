// <copyright file="SearchView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.Ad
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class for ads searching.
    /// </summary>
    public class SearchView
    {
        /// <summary>
        /// Gets or sets region.
        /// </summary>
        [StringLength(200, ErrorMessage = "RegionLengthError")]
        [Display(Name = "Region")]
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets district.
        /// </summary>
        [StringLength(200, ErrorMessage = "DistrictLengthError")]
        [Display(Name = "District")]
        public string District { get; set; }

        /// <summary>
        /// Gets or sets locality.
        /// </summary>
        [StringLength(200, ErrorMessage = "LocalityLengthError")]
        [Display(Name = "Locality")]
        public string Locality { get; set; }

        /// <summary>
        /// Gets or sets address.
        /// </summary>
        [StringLength(200, ErrorMessage = "AddressLengthError")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets start rooms count.
        /// </summary>
        [Range(1, 1000, ErrorMessage = "RoomError")]
        [Display(Name = "StartRoomsCount")]
        public int? StartRoomsCount { get; set; }

        /// <summary>
        /// Gets or sets finish rooms count.
        /// </summary>
        [Range(1, 1000, ErrorMessage = "RoomError")]
        [Display(Name = "FinishRoomsCount")]
        public int? FinishRoomsCount { get; set; }

        /// <summary>
        /// Gets or sets start current floor.
        /// </summary>
        [Range(1, 1000, ErrorMessage = "FloorError")]
        [Display(Name = "StartCurrentFloor")]
        public int? StartCurrentFloor { get; set; }

        /// <summary>
        /// Gets or sets finish current floor.
        /// </summary>
        [Range(1, 1000, ErrorMessage = "FloorError")]
        [Display(Name = "FinishCurrentFloor")]
        public int? FinishCurrentFloor { get; set; }

        /// <summary>
        /// Gets or sets start total floor.
        /// </summary>
        [Range(1, 1000, ErrorMessage = "FloorError")]
        [Display(Name = "StartTotalFloor")]
        public int? StartTotalFloor { get; set; }

        /// <summary>
        /// Gets or sets finish total floor.
        /// </summary>
        [Range(1, 1000, ErrorMessage = "FloorError")]
        [Display(Name = "FinishTotalFloor")]
        public int? FinishTotalFloor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is first floor.
        /// </summary>
        [Display(Name = "IsFirstFloor")]
        public bool IsFirstFloor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is first floor.
        /// </summary>
        [Display(Name = "IsLastFloor")]
        public bool IsLastFloor { get; set; }
    }
}
