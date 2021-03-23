// <copyright file="AditionalAdDataDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Models
{
    using System;

    /// <summary>
    /// Aditional ad data model.
    /// </summary>
    public class AditionalAdDataDTO
    {
        /// <summary>
        /// Gets or sets unique key.
        /// </summary>
        public int Id { get; set; }

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
