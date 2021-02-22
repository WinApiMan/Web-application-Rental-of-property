// <copyright file="MultilanguageIdentityErrorDescriber.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Context
{
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Class for identity errors localization.
    /// </summary>
    public class MultilanguageIdentityErrorDescriber : IdentityErrorDescriber
    {
        /// <summary>
        /// Method for generate default error.
        /// </summary>
        /// <returns>Identity error object.</returns>
        public override IdentityError DefaultError()
        {
            return new IdentityError
            {
                Code = nameof(DefaultError),
                Description = "DefaultIdentityError",
            };
        }

        /// <summary>
        /// Method for generate concurrency failure error.
        /// </summary>
        /// <returns>Identity error object.</returns>
        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError
            {
                Code = nameof(ConcurrencyFailure),
                Description = "ConcurencyFailureIdentityError",
            };
        }

        /// <summary>
        /// Method for generate password mismatch error.
        /// </summary>
        /// <returns>Identity error object.</returns>
        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = "PasswordMismatchIdentityError",
            };
        }

        /// <summary>
        /// Method for generate invalid token error.
        /// </summary>
        /// <returns>Identity error object.</returns>
        public override IdentityError InvalidToken()
        {
            return new IdentityError
            {
                Code = nameof(InvalidToken),
                Description = "InvalidTokenIdentityError",
            };
        }

        /// <summary>
        /// Method for generate login already associated error.
        /// </summary>
        /// <returns>Identity error object.</returns>
        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError
            {
                Code = nameof(LoginAlreadyAssociated),
                Description = "LoginAlreadyAssociatedIdentityError",
            };
        }

        /// <summary>
        /// Method for generate invalid user name error.
        /// </summary>
        /// <param name="userName">Incorrect name.</param>
        /// <returns>Identity error object.</returns>
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = "InvalidUserNameIdentityError",
            };
        }

        /// <summary>
        /// Method for generate invalid email error.
        /// </summary>
        /// <param name="email">Incorrect email.</param>
        /// <returns>Identity error object.</returns>
        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = "InvalidEmailIdentityError",
            };
        }

        /// <summary>
        /// Method for generate duplicate user name error.
        /// </summary>
        /// <param name="userName">Duplicate name.</param>
        /// <returns>Identity error object.</returns>
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = "DuplicateUserNameIdentityError",
            };
        }

        /// <summary>
        /// Method for generate duplicate email error.
        /// </summary>
        /// <param name="email">Duplicate email.</param>
        /// <returns>Identity error object.</returns>
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = "DuplicateEmailIdentityError",
            };
        }

        /// <summary>
        /// Method for generate invalid role name error.
        /// </summary>
        /// <param name="role">Incorrect role name.</param>
        /// <returns>Identity error object.</returns>
        public override IdentityError InvalidRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(InvalidRoleName),
                Description = "InvalidRoleNameIdentityError",
            };
        }

        /// <summary>
        /// Method for generate duplicate role name error.
        /// </summary>
        /// <param name="role">Duplicate role name.</param>
        /// <returns>Identity error object.</returns>
        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateRoleName),
                Description = "DuplicateRoleNameIdentityError",
            };
        }

        /// <summary>
        /// Method for generate user already has password error.
        /// </summary>
        /// <returns>Identity error object.</returns>
        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyHasPassword),
                Description = "UserAlreadyHasPasswordIdentityError",
            };
        }

        /// <summary>
        /// Method for generate user lockout not enabled error.
        /// </summary>
        /// <returns>Identity error object.</returns>
        public override IdentityError UserLockoutNotEnabled()
        {
            return new IdentityError
            {
                Code = nameof(UserLockoutNotEnabled),
                Description = "UserLockoutNotEnabledIdentityError",
            };
        }

        /// <summary>
        /// Method for generate user already in role error.
        /// </summary>
        /// <param name="role">New role.</param>
        /// <returns>Identity error object.</returns>
        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyInRole),
                Description = "UserAlreadyInRoleIdentityError",
            };
        }

        /// <summary>
        /// Method for generate user not in role error.
        /// </summary>
        /// <param name="role">User role.</param>
        /// <returns>Identity error object.</returns>
        public override IdentityError UserNotInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(UserNotInRole),
                Description = "UserNotInRoleIdentityError",
            };
        }

        /// <summary>
        /// Method for generate password to short error.
        /// </summary>
        /// <param name="length">Minimum length.</param>
        /// <returns>Identity error object.</returns>
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = "PasswordTooShortIdentityError",
            };
        }

        /// <summary>
        /// Method for generate password requires non alplhanumeric error.
        /// </summary>
        /// <returns>Identity error object.</returns>
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "PasswordRequiresNonAlphanumericIdentityError",
            };
        }

        /// <summary>
        /// Method for generate password requires digit error.
        /// </summary>
        /// <returns>Identity error object.</returns>
        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "PasswordRequiresDigitIdentityError",
            };
        }

        /// <summary>
        /// Method for generate password requires lower error.
        /// </summary>
        /// <returns>Identity error object.</returns>
        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = "PasswordRequiresLowerIdentityError",
            };
        }

        /// <summary>
        /// Method for generate password requires upper error.
        /// </summary>
        /// <returns>Identity error object.</returns>
        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "PasswordRequiresUpperIdentityError",
            };
        }
    }
}
