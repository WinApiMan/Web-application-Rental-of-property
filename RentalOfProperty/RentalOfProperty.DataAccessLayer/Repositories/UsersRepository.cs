// <copyright file="UsersRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using RentalOfProperty.DataAccessLayer.Interfaces;
    using RentalOfProperty.DataAccessLayer.Models;

    /// <summary>
    /// Users main operations.
    /// </summary>
    public class UsersRepository : IUserRepository
    {
        private readonly UserManager<UserDTO> _userManager;

        private readonly SignInManager<UserDTO> _signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersRepository"/> class.
        /// </summary>
        /// <param name="userManager">Users manager.</param>
        /// <param name="signInManager">Sign in manager.</param>
        public UsersRepository(UserManager<UserDTO> userManager, SignInManager<UserDTO> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Method for adding user in table.
        /// </summary>
        /// <param name="user">User adding.</param>
        /// <param name="password">User password.</param>
        /// <param name="role">Role.</param>
        /// <returns>Is successed result.</returns>
        public async Task<IdentityResult> Create(UserDTO user, string password, string role)
        {
            user.UserName = user.Email;

            var createResult = await _userManager.CreateAsync(user, password);

            if (createResult.Succeeded)
            {
                return await _userManager.AddToRoleAsync(user, role);
            }
            else
            {
                return createResult;
            }
        }

        /// <summary>
        /// Method for find user in table.
        /// </summary>
        /// <param name="id">Primary key.</param>
        /// <returns>User.</returns>
        public async Task<UserDTO> FindById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        /// <summary>
        /// Method for getting all users from table.
        /// </summary>
        /// <returns>Users.</returns>
        public IEnumerable<UserDTO> Get()
        {
            return _userManager.Users;
        }

        /// <summary>
        /// Method for getting users from table with some filter.
        /// </summary>
        /// <param name="predicate">Search filter.</param>
        /// <returns>Users.</returns>
        public IEnumerable<UserDTO> Get(Func<UserDTO, bool> predicate)
        {
            return _userManager.Users.Where(predicate).ToList();
        }

        /// <summary>
        /// Metohd for removing an user from table.
        /// </summary>
        /// <param name="user">Removing user.</param>
        /// <returns>Void return.</returns>
        public async Task<IdentityResult> Remove(UserDTO user)
        {
            return await _userManager.DeleteAsync(user);
        }

        /// <summary>
        /// Method for updating user.
        /// </summary>
        /// <param name="user">User updating.</param>
        /// <returns>Void return.</returns>
        public async Task<IdentityResult> Update(UserDTO user)
        {
            return await _userManager.UpdateAsync(user);
        }

        /// <summary>
        /// Generate token.
        /// </summary>
        /// <param name="user">User object.</param>
        /// <returns>Token.</returns>
        public async Task<string> GenerateEmailConfirmationTokenAsync(UserDTO user)
        {
            // Token generation for user
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        /// <summary>
        /// Confirm user email.
        /// </summary>
        /// <param name="user">User object.</param>
        /// <param name="code">Confirmation string.</param>
        /// <returns>Identity result object.</returns>
        public async Task<IdentityResult> ConfirmEmailAsync(UserDTO user, string code)
        {
            return await _userManager.ConfirmEmailAsync(user, code);
        }

        /// <summary>
        /// Check email confirm.
        /// </summary>
        /// <param name="user">User object.</param>
        /// <returns>Confirm result(true or false).</returns>
        public async Task<bool> IsEmailConfirmedAsync(UserDTO user)
        {
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        /// <summary>
        /// Sign in account.
        /// </summary>
        /// <param name="user">User sign in object.</param>
        /// <returns>Sign in result object.</returns>
        public async Task<SignInResult> PasswordSignInAsync(SignInUserDTO user)
        {
            return await _signInManager.PasswordSignInAsync(user.Login, user.Password, user.IsRememberMe, user.IsLockoutOnFailure);
        }

        /// <summary>
        /// Account log off.
        /// </summary>
        /// <returns>Task result.</returns>
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        /// <summary>
        /// Change user password.
        /// </summary>
        /// <param name="user">User object.</param>
        /// <param name="oldPassword">Old user password.</param>
        /// <param name="newPassword">New user password.</param>
        /// <returns>Identity result object.</returns>
        public async Task<IdentityResult> ChangePasswordAsync(UserDTO user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        /// <summary>
        /// Generate code to reset password.
        /// </summary>
        /// <param name="user">User object.</param>
        /// <returns>Generated code.</returns>
        public async Task<string> GeneratePasswordResetTokenAsync(UserDTO user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        /// <summary>
        /// Reset user password.
        /// </summary>
        /// <param name="user">User object.</param>
        /// <param name="code">Generated code.</param>
        /// <param name="password">New user password.</param>
        /// <returns>Reset result object.</returns>
        public async Task<IdentityResult> ResetPasswordAsync(UserDTO user, string code, string password)
        {
            return await _userManager.ResetPasswordAsync(user, code, password);
        }
    }
}
