// <copyright file="SignInUserDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Models
{
    /// <summary>
    /// Sign in dto model.
    /// </summary>
    public class SignInUserDTO
    {
        /// <summary>
        /// Gets or sets email adress.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets user password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether remember in memory or no.
        /// </summary>
        public bool IsRememberMe { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether lockout on failure.
        /// </summary>
        public bool IsLockoutOnFailure { get; set; }
    }
}
