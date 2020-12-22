// <copyright file="Roles.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models
{
    /// <summary>
    /// Class with all user roles.
    /// </summary>
    public static class Roles
    {
        /// <summary>
        /// Gets or sets administrator role.
        /// </summary>
        public static string Administrator { get; set; } = "Administrator";

        /// <summary>
        /// Gets or sets user role.
        /// </summary>
        public static string User { get; set; } = "User";
    }
}
