﻿// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Models
{
    /// <summary>
    /// User entiti.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets full user name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets email adress.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets user phone(mobile or home).
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets user password.
        /// </summary>
        public string Password { get; set; }
    }
}