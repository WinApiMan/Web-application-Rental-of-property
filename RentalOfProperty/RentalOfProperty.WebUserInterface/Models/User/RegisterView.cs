// <copyright file="RegisterView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.User
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Register view model.
    /// </summary>
    public class RegisterView
    {
        /// <summary>
        /// Gets or sets full user name.
        /// </summary>
        [Required(ErrorMessage = "FullNameRequired")]
        [Display(Name = "FullName")]
        [StringLength(50, ErrorMessage = "FullNameLengthError", MinimumLength = 5)]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets email adress.
        /// </summary>
        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailValidError")]
        [Display(Name = "Email")]
        [StringLength(50, ErrorMessage = "EmailLengthError", MinimumLength = 1)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets phone (mobile or home).
        /// </summary>
        [Required(ErrorMessage = "PhoneRequired")]
        [Phone(ErrorMessage = "PhoneValidError")]
        [Display(Name = "Phone")]
        [StringLength(50, ErrorMessage = "PhoneLengthError", MinimumLength = 5)]
        public string Phone { get; set; }

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
    }
}
