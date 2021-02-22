// <copyright file="ChangePasswordView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Models.User
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Change password view model.
    /// </summary>
    public class ChangePasswordView
    {
        /// <summary>
        /// Gets or sets user old password.
        /// </summary>
        [Required(ErrorMessage = "OldPasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "OldPassword")]
        [StringLength(50, ErrorMessage = "PasswordLengthError", MinimumLength = 10)]
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets user new password.
        /// </summary>
        [Required(ErrorMessage = "NewPasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword")]
        [StringLength(50, ErrorMessage = "PasswordLengthError", MinimumLength = 10)]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets confirm new user password.
        /// </summary>
        [Required(ErrorMessage = "NewPasswordConfirmRequired")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "PasswordsComparingError")]
        [Display(Name = "NewPasswordConfirm")]
        [StringLength(50, ErrorMessage = "PasswordConfirmLengthError", MinimumLength = 10)]
        public string NewPasswordConfirm { get; set; }
    }
}
