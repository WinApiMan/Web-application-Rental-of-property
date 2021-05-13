// <copyright file="CreateModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Models
{
    using RentalOfProperty.BusinessLogicLayer.Enums;

    /// <summary>
    /// Create ad model.
    /// </summary>
    public class CreateModel
    {
        /// <summary>
        /// Gets or sets unique key.
        /// </summary>
        public string Id { get; set; }

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
        public AdsTypeMenu RentalType { get; set; }

        /// <summary>
        /// Gets or sets land area.
        /// </summary>
        public double LandArea { get; set; }

        /// <summary>
        /// Gets or sets BYN per person price.
        /// </summary>
        public double BYNPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets USD per person price.
        /// </summary>
        public double USDPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets USD per day price.
        /// </summary>
        public double USDPricePerDay { get; set; }

        /// <summary>
        /// Gets or sets BYN per day price.
        /// </summary>
        public double BYNPricePerDay { get; set; }

        /// <summary>
        /// Gets or sets BYN price.
        /// </summary>
        public double BYNPrice { get; set; }

        /// <summary>
        /// Gets or sets USD price.
        /// </summary>
        public double USDPrice { get; set; }
    }
}
