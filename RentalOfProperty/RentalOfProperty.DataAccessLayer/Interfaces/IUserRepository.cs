// <copyright file="IUserRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using RentalOfProperty.DataAccessLayer.Models;

    /// <summary>
    /// Interface of user repository for data base.
    /// </summary>
    /// <typeparam name="T">Entity class.</typeparam>
    public interface IUserRepository
    {
        /// <summary>
        /// Method for adding item in table.
        /// </summary>
        /// <param name="user">Adding user.</param>
        /// <param name="password">User password.</param>
        /// <param name="role">Role.</param>
        /// <returns>Is successed result.</returns>
        Task<IdentityResult> Create(UserDTO user, string password, string role);

        /// <summary>
        /// Method for find item in table.
        /// </summary>
        /// <param name="id">Primary key.</param>
        /// <returns>Result user.</returns>
        Task<UserDTO> FindById(string id);

        /// <summary>
        /// Method for getting all items from table.
        /// </summary>
        /// <returns>All items.</returns>
        IEnumerable<UserDTO> Get();

        /// <summary>
        /// Method for getting items from table with some filter.
        /// </summary>
        /// <param name="predicate">Search filter.</param>
        /// <returns>Result items.</returns>
        IEnumerable<UserDTO> Get(Func<UserDTO, bool> predicate);

        /// <summary>
        /// Metohd for removing an item from table.
        /// </summary>
        /// <param name="user">Removing user.</param>
        /// <returns>Void return.</returns>
        Task<IdentityResult> Remove(UserDTO user);

        /// <summary>
        /// Method for updating item.
        /// </summary>
        /// <param name="user">Updating user.</param>
        /// <returns>Void return.</returns>
        Task<IdentityResult> Update(UserDTO user);

        /// <summary>
        /// Generate token.
        /// </summary>
        /// <param name="user">User object.</param>
        /// <returns>Token.</returns>
        Task<string> GenerateEmailConfirmationTokenAsync(UserDTO user);

        /// <summary>
        /// Confirm user email.
        /// </summary>
        /// <param name="user">User object.</param>
        /// <param name="code">Confirmation string.</param>
        /// <returns>Identity result object.</returns>
        Task<IdentityResult> ConfirmEmailAsync(UserDTO user, string code);

        /// <summary>
        /// Check email confirm.
        /// </summary>
        /// <param name="user">User object.</param>
        /// <returns>Confirm result(true or false).</returns>
        Task<bool> IsEmailConfirmedAsync(UserDTO user);

        /// <summary>
        /// Sign in account.
        /// </summary>
        /// <param name="user">User sign in object.</param>
        /// <returns>Sign in result object.</returns>
        Task<SignInResult> PasswordSignInAsync(SignInUserDTO user);

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
        Task<IdentityResult> ChangePasswordAsync(UserDTO user, string oldPassword, string newPassword);
    }
}
