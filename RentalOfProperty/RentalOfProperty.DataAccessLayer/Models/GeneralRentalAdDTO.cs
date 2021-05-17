﻿// <copyright file="GeneralRentalAdDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Models
{
    using System;

    /// <summary>
    /// General rental ad model.
    /// </summary>
    public class GeneralRentalAdDTO
    {
        /// <summary>
        /// Gets or sets unique key.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets contact person unique key.
        /// </summary>
        public string ContactPersonId { get; set; }

        /// <summary>
        /// Gets or sets user unique key.
        /// </summary>
        public string UserId { get; set; }

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
        /// Gets or sets total views.
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

        /// <summary>
        /// Gets or sets a value indicating whether is ad published.
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        /// Gets or sets BYN price.
        /// </summary>
        public double? BYNPrice { get; set; }

        /// <summary>
        /// Gets or sets USD price.
        /// </summary>
        public double? USDPrice { get; set; }

        /// <summary>
        /// Gets or sets BYN per person price.
        /// </summary>
        public double? BYNPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets USD per person price.
        /// </summary>
        public double? USDPricePerPerson { get; set; }

        /// <summary>
        /// Gets or sets USD per day price.
        /// </summary>
        public double? USDPricePerDay { get; set; }

        /// <summary>
        /// Gets or sets BYN per day price.
        /// </summary>
        public double? BYNPricePerDay { get; set; }

        /// <summary>
        /// Gets or sets Discriminator.
        /// </summary>
        public string Discriminator { get; set; }
    }
}