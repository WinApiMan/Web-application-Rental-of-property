// <copyright file="ResetPasswordView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.User
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Reset password view model.
    /// </summary>
    public class ResetPasswordView
    {
        /// <summary>
        /// Gets or sets email adress.
        /// </summary>
        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailValidError")]
        [Display(Name = "Email")]
        [StringLength(50, ErrorMessage = "EmailLengthError", MinimumLength = 1)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets user password.
        /// </summary>
        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(50, ErrorMessage = "PasswordLengthError", MinimumLength = 10)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets confirm user password.
        /// </summary>
        [Required(ErrorMessage = "PasswordConfirmRequired")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "PasswordsComparingError")]
        [Display(Name = "PasswordConfirm")]
        [StringLength(50, ErrorMessage = "PasswordConfirmLengthError", MinimumLength = 10)]
        public string PasswordConfirm { get; set; }

        /// <summary>
        /// Gets or sets password recovery code.
        /// </summary>
        public string Code { get; set; }
    }
}
