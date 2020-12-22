// <copyright file="UserDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Models
{
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// User entiti.
    /// </summary>
    public class UserDTO : IdentityUser
    {
        /// <summary>
        /// Gets or sets full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets user phone(mobile or home).
        /// </summary>
        public string Phone { get; set; }
    }
}
