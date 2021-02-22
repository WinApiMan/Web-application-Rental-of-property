// <copyright file="EditView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.User
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Model for user update.
    /// </summary>
    public class EditView
    {
        /// <summary>
        /// Gets or sets full user name.
        /// </summary>
        [Required(ErrorMessage = "FullNameRequired")]
        [Display(Name = "FullName")]
        [StringLength(50, ErrorMessage = "FullNameLengthError", MinimumLength = 5)]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets user phone(mobile or home).
        /// </summary>
        [Required(ErrorMessage = "PhoneRequired")]
        [Phone(ErrorMessage = "PhoneValidError")]
        [Display(Name = "Phone")]
        [StringLength(50, ErrorMessage = "PhoneLengthError", MinimumLength = 5)]
        public string PhoneNumber { get; set; }
    }
}
