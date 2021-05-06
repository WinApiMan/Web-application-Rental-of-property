// <copyright file="RentalAdView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.Ad
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Rental ad view model.
    /// </summary>
    public class RentalAdView
    {
        /// <summary>
        /// Gets or sets unique key.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets contact person unique key.
        /// </summary>
        [Display(Name = "ContactPersonId")]
        public string ContactPersonId { get; set; }

        /// <summary>
        /// Gets or sets unique link.
        /// </summary>
        [Display(Name = "SourceLink")]
        public string SourceLink { get; set; }

        /// <summary>
        /// Gets or sets rental ad number.
        /// </summary>
        [Display(Name = "RentalAdNumber")]
        public int RentalAdNumber { get; set; }

        /// <summary>
        /// Gets or sets update date.
        /// </summary>
        [Display(Name = "UpdateDate")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets region.
        /// </summary>
        [Display(Name = "Region")]
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets district.
        /// </summary>
        [Display(Name = "District")]
        public string District { get; set; }

        /// <summary>
        /// Gets or sets locality.
        /// </summary>
        [Display(Name = "Locality")]
        public string Locality { get; set; }

        /// <summary>
        /// Gets or sets address.
        /// </summary>
        [Display(Name = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets total count of rooms.
        /// </summary>
        [Display(Name = "TotalCountOfRooms")]
        public int TotalCountOfRooms { get; set; }

        /// <summary>
        /// Gets or sets rent count of rooms.
        /// </summary>
        [Display(Name = "RentCountOfRooms")]
        public int RentCountOfRooms { get; set; }

        /// <summary>
        /// Gets or sets total area.
        /// </summary>
        [Display(Name = "TotalArea")]
        public double TotalArea { get; set; }

        /// <summary>
        /// Gets or sets living area.
        /// </summary>
        [Display(Name = "LivingArea")]
        public double LivingArea { get; set; }

        /// <summary>
        /// Gets or sets kitchen area.
        /// </summary>
        [Display(Name = "KitchenArea")]
        public double KitchenArea { get; set; }

        /// <summary>
        /// Gets or sets total floors.
        /// </summary>
        [Display(Name = "TotalFloors")]
        public int TotalFloors { get; set; }

        /// <summary>
        /// Gets or sets floor.
        /// </summary>
        [Display(Name = "Floor")]
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
        public string Bathroom { get; set; }

        /// <summary>
        /// Gets or sets notes.
        /// </summary>
        [Display(Name = "Notes")]

        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets facilities.
        /// </summary>
        [Display(Name = "Facilities")]
        public string Facilities { get; set; }

        /// <summary>
        /// Gets or sets RentalType.
        /// </summary>
        public int RentalType { get; set; }

        /// <summary>
        /// Gets or sets total views
        /// </summary>
        public int TotalViews { get; set; }

        /// <summary>
        /// Gets or sets month views.
        /// </summary>
        public int MonthViews { get; set; }

        /// <summary>
        /// Gets or sets week views.
        /// </summary>
        public int WeekViews { get; set; }

        /// <summary>
        /// Gets or sets land area.
        /// </summary>
        [Display(Name = "LandArea")]
        public double LandArea { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is favorite.
        /// </summary>
        public bool IsFavorite { get; set; }
    }
}
