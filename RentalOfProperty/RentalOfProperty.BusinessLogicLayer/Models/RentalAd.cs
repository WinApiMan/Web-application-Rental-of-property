// <copyright file="RentalAd.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace RentalOfProperty.BusinessLogicLayer.Models
{
    using System;

    /// <summary>
    /// Rental ad model.
    /// </summary>
    public class RentalAd
    {
        /// <summary>
        /// Gets or sets unique key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets unique link.
        /// </summary>
        public string SourceLink { get; set; }

        /// <summary>
        /// Gets or sets rental ad number.
        /// </summary>
        public int RentalAdNumber { get; set; }

        /// <summary>
        /// Gets or sets update date.
        /// </summary>
        public DateTime UpdateDate { get; set; }

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
        /// Gets or sets total count of rooms.
        /// </summary>
        public int TotalCountOfRooms { get; set; }

        /// <summary>
        /// Gets or sets rent count of rooms.
        /// </summary>
        public int RentCountOfRooms { get; set; }

        /// <summary>
        /// Gets or sets total area.
        /// </summary>
        public double TotalArea { get; set; }

        /// <summary>
        /// Gets or sets living area.
        /// </summary>
        public double LivingArea { get; set; }

        /// <summary>
        /// Gets or sets kitchen area.
        /// </summary>
        public double KitchenArea { get; set; }

        /// <summary>
        /// Gets or sets total floors.
        /// </summary>
        public int TotalFloors { get; set; }

        /// <summary>
        /// Gets or sets floor.
        /// </summary>
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
        public string Bathroom { get; set; }

        /// <summary>
        /// Gets or sets notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets facilities.
        /// </summary>
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
        public double LandArea { get; set; }
    }
}
