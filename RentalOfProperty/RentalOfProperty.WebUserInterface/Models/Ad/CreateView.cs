// <copyright file="CreateView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.Ad
{
    using System.ComponentModel.DataAnnotations;
    using RentalOfProperty.BusinessLogicLayer.Enums;

    /// <summary>
    /// Rental ad view model.
    /// </summary>
    public class CreateView
    {
        /// <summary>
        /// Gets or sets unique key.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets region.
        /// </summary>
        [Display(Name = "Region")]
        [Required(ErrorMessage = "RegionRequired")]
        [StringLength(70, ErrorMessage = "RegionError", MinimumLength = 1)]
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets district.
        /// </summary>
        [Display(Name = "District")]
        [Required(ErrorMessage = "DistrictRequired")]
        [StringLength(70, ErrorMessage = "DistrictError", MinimumLength = 1)]
        public string District { get; set; }

        /// <summary>
        /// Gets or sets locality.
        /// </summary>
        [Display(Name = "Locality")]
        [Required(ErrorMessage = "LocalityRequired")]
        [StringLength(70, ErrorMessage = "LocalityError", MinimumLength = 1)]
        public string Locality { get; set; }

        /// <summary>
        /// Gets or sets address.
        /// </summary>
        [Display(Name = "Address")]
        [Required(ErrorMessage = "AddressRequired")]
        [StringLength(100, ErrorMessage = "AddressError", MinimumLength = 1)]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets total count of rooms.
        /// </summary>
        [Display(Name = "TotalCountOfRooms")]
        [Required(ErrorMessage = "TotalCountOfRoomsRequired")]
        [Range(1, 10000, ErrorMessage = "TotalCountOfRoomsError")]
        public int TotalCountOfRooms { get; set; }

        /// <summary>
        /// Gets or sets rent count of rooms.
        /// </summary>
        [Display(Name = "RentCountOfRooms")]
        [Required(ErrorMessage = "RentCountOfRoomsRequired")]
        [Range(1, 10000, ErrorMessage = "RentCountOfRoomsError")]
        public int RentCountOfRooms { get; set; }

        /// <summary>
        /// Gets or sets total area.
        /// </summary>
        [Display(Name = "TotalArea")]
        [Required(ErrorMessage = "TotalAreaRequired")]
        [Range(1.0, 1000000.0, ErrorMessage = "TotalAreaError")]
        public double TotalArea { get; set; }

        /// <summary>
        /// Gets or sets living area.
        /// </summary>
        [Display(Name = "LivingArea")]
        [Required(ErrorMessage = "LivingAreaRequired")]
        [Range(1.0, 1000000.0, ErrorMessage = "LivingAreaError")]
        public double LivingArea { get; set; }

        /// <summary>
        /// Gets or sets kitchen area.
        /// </summary>
        [Display(Name = "KitchenArea")]
        [Required(ErrorMessage = "KitchenAreaRequired")]
        [Range(1.0, 1000000.0, ErrorMessage = "KitchenAreaError")]
        public double KitchenArea { get; set; }

        /// <summary>
        /// Gets or sets total floors.
        /// </summary>
        [Display(Name = "TotalFloors")]
        [Required(ErrorMessage = "TotalFloorsRequired")]
        [Range(1, 1000, ErrorMessage = "TotalFloorsError")]
        public int TotalFloors { get; set; }

        /// <summary>
        /// Gets or sets floor.
        /// </summary>
        [Display(Name = "Floor")]
        [Required(ErrorMessage = "FloorRequired")]
        [Range(1, 1000, ErrorMessage = "FloorError")]
        public int Floor { get; set; }

        /// <summary>
        /// Gets or sets x map coordinate.
        /// </summary>
        public double XMapCoordinate { get; set; }

        /// <summary>
        /// Gets or sets y map coordinate.
        /// </summary>
        public double YMapCoordinate { get; set; }

        /// <summary>
        /// Gets or sets bathroom.
        /// </summary>
        [Display(Name = "Bathroom")]
        [Required(ErrorMessage = "BathroomRequired")]
        [StringLength(70, ErrorMessage = "BathroomError", MinimumLength = 1)]
        public string Bathroom { get; set; }

        /// <summary>
        /// Gets or sets notes.
        /// </summary>
        [Display(Name = "Notes")]
        [StringLength(100, ErrorMessage = "NotesError", MinimumLength = 1)]
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        [Display(Name = "Description")]
        [Required(ErrorMessage = "DescriptionRequired")]
        [StringLength(300, ErrorMessage = "DescriptionError", MinimumLength = 1)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets facilities.
        /// </summary>
        [Display(Name = "Facilities")]
        [Required(ErrorMessage = "FacilitiesRequired")]
        [StringLength(200, ErrorMessage = "FacilitiesError", MinimumLength = 1)]
        public string Facilities { get; set; }

        /// <summary>
        /// Gets or sets RentalType.
        /// </summary>
        [Display(Name = "RentalType")]
        [Required(ErrorMessage = "RentalTypeRequired")]
        public AdsTypeMenu RentalType { get; set; }

        /// <summary>
        /// Gets or sets land area.
        /// </summary>
        [Display(Name = "LandArea")]
        [Required(ErrorMessage = "LandAreaRequired")]
        [Range(1.0, 1000000.0, ErrorMessage = "LandAreaError")]
        public double LandArea { get; set; }

        /// <summary>
        /// Gets or sets BYN per person price.
        /// </summary>
        [Display(Name = "BYNPricePerPerson")]
        [Required(ErrorMessage = "BYNPricePerPersonRequired")]
        [Range(0.0, 1000000000.0, ErrorMessage = "BYNPricePerPersonError")]
        public double BYNPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets USD per person price.
        /// </summary>
        [Display(Name = "USDPricePerPerson")]
        [Required(ErrorMessage = "USDPricePerPersonRequired")]
        [Range(0.0, 1000000000.0, ErrorMessage = "USDPricePerPersonError")]
        public double USDPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets USD per day price.
        /// </summary>
        [Display(Name = "USDPricePerDay")]
        [Required(ErrorMessage = "USDPricePerDayRequired")]
        [Range(0.0, 1000000000.0, ErrorMessage = "USDPricePerDayError")]
        public double USDPricePerDay { get; set; }

        /// <summary>
        /// Gets or sets BYN per day price.
        /// </summary>
        [Display(Name = "BYNPricePerDay")]
        [Required(ErrorMessage = "BYNPricePerDayRequired")]
        [Range(0.0, 1000000000.0, ErrorMessage = "BYNPricePerDayError")]
        public double BYNPricePerDay { get; set; }

        /// <summary>
        /// Gets or sets BYN price.
        /// </summary>
        [Display(Name = "BYNPrice")]
        [Required(ErrorMessage = "BYNPriceRequired")]
        [Range(0.0, 1000000000.0, ErrorMessage = "BYNPriceError")]
        public double BYNPrice { get; set; }

        /// <summary>
        /// Gets or sets USD price.
        /// </summary>
        [Display(Name = "USDPrice")]
        [Required(ErrorMessage = "USDPriceRequired")]
        [Range(0.0, 1000000000.0, ErrorMessage = "USDPriceError")]
        public double USDPrice { get; set; }
    }
}
