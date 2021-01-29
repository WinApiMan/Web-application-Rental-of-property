// <copyright file="AuthorizeView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.User
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Authorize view model.
    /// </summary>
    public class AuthorizeView
    {
        /// <summary>
        /// Gets or sets email adress.
        /// </summary>
        [Required(ErrorMessage = "LoginRequired")]
        [EmailAddress(ErrorMessage = "LoginValidError")]
        [Display(Name = "Login")]
        [StringLength(50, ErrorMessage = "LoginLengthError", MinimumLength = 1)]
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets user password.
        /// </summary>
        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(50, ErrorMessage = "PasswordLengthError", MinimumLength = 10)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether remember in memory or no.
        /// </summary>
        [Display(Name = "RememberMe")]
        public bool IsRememberMe { get; set; }
    }
}
