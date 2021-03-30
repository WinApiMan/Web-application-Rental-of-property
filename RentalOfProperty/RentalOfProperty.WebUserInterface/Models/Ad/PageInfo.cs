// <copyright file="PageInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.Ad
{
    using System;

    /// <summary>
    /// Page info model.
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// Gets or sets current page number.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets objects count on page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets total items.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Gets total pages.
        /// </summary>
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
}
