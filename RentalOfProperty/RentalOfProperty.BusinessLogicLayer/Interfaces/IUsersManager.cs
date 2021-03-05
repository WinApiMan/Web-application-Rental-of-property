// <copyright file="IUsersManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RentalOfProperty.BusinessLogicLayer.Models;

    /// <summary>
    /// User manager interface.
    /// </summary>
    public interface IUsersManager
    {
        /// <summary>
        /// Method for adding item in table.
        /// </summary>
        /// <param name="item">Adding item.</param>
        /// <param name="role">User role.</param>
        /// <returns>Void return.</returns>
        Task<IdentityResult> Create(User item, string role);

        /// <summary>
        /// Method for find item in table.
        /// </summary>
        /// <param name="id">Primary key.</param>
        /// <returns>Result item.</returns>
        Task<User> FindById(string id);

        /// <summary>
        /// Method for getting all items from table.
        /// </summary>
        /// <returns>All items.</returns>
        Task<IEnumerable<User>> Get();

        /// <summary>
        /// Method for getting items from table with some filter.
        /// </summary>
        /// <param name="predicate">Search filter.</param>
        /// <returns>Result items.</returns>
        Task<IEnumerable<User>> Get(Func<User, bool> predicate);

        /// <summary>
        /// Metohd for removing an item from table.
        /// </summary>
        /// <param name="item">Removing item.</param>
        /// <returns>Void return.</returns>
        Task Remove(User item);

        /// <summary>
        /// Method for updating item.
        /// </summary>
        /// <param name="item">Updating item.</param>
        /// <returns>Void return.</returns>
        Task<IdentityResult> Update(User item);

        /// <summary>
        /// Find user by email.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <returns>User object.</returns>
        User FindByEmail(string email);

        /// <summary>
        /// Generate token.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>Token.</returns>
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);

        /// <summary>
        /// Confirm user email.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="code">Confirmation string.</param>
        /// <returns>Identity result object.</returns>
        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);

        /// <summary>
        /// Check email confirm.
        /// </summary>
        /// <param name="login">User login.</param>
        /// <returns>Confirm result(true or false).</returns>
        Task<bool> IsEmailConfirmedAsync(string login);

        /// <summary>
        /// Sign in account.
        /// </summary>
        /// <param name="user">Sign in user object.</param>
        /// <returns>Password signInResult(true or false).</returns>
        Task<SignInResult> PasswordSignInAsync(SignInUser user);

        /// <summary>
        /// Account log off.
        /// </summary>
        /// <returns>Task result.</returns>
        Task SignOutAsync();

        /// <summary>
        /// Change user password.
        /// </summary>
        /// <param name="user">User object.</param>
        /// <param name="oldPassword">Old user password.</param>
        /// <param name="newPassword">New user password.</param>
        /// <returns>Identity result object.</returns>
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        /// <summary>
        /// Generate code to reset password.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>Generated code.</returns>
        Task<string> GeneratePasswordResetTokenAsync(string userId);

        /// <summary>
        /// Reset user password.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="code">Generated code.</param>
        /// <param name="password">New user password.</param>
        /// <returns>Reset result object.</returns>
        Task<IdentityResult> ResetPasswordAsync(string userId, string code, string password);
    }
}
