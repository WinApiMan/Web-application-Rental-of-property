// <copyright file="ForgotPasswordView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.User
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Forgot password view model.
    /// </summary>
    public class ForgotPasswordView
    {
        /// <summary>
        /// Gets or sets email adress.
        /// </summary>
        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailValidError")]
        [Display(Name = "Email")]
        [StringLength(50, ErrorMessage = "EmailLengthError", MinimumLength = 1)]
        public string Email { get; set; }
    }
}
