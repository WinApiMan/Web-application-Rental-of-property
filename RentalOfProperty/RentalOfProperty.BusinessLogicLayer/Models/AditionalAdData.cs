﻿// <copyright file="AditionalAdData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Models
{
    using System;

    /// <summary>
    /// Aditional ad data model.
    /// </summary>
    public class AditionalAdData
    {
        /// <summary>
        /// Gets or sets unique key.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets rental ad number.
        /// </summary>
        public int RentalAdNumber { get; set; }

        /// <summary>
        /// Gets or sets update date.
        /// </summary>
        public DateTime UpdateDate { get; set; }

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
    }
}