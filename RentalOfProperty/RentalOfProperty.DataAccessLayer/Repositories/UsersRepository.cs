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
    public class UsersRepository : IUserRepository<UserDTO>
    {
        private readonly UserManager<UserDTO> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersRepository"/> class.
        /// </summary>
        /// <param name="userManager">Users manager.</param>
        public UsersRepository(UserManager<UserDTO> userManager)
        {
            _userManager = userManager;
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
        /// <param name="item">Removing user.</param>
        /// <returns>Void return.</returns>
        public async Task<IdentityResult> Remove(UserDTO item)
        {
            return await _userManager.DeleteAsync(item);
        }

        /// <summary>
        /// Method for updating user.
        /// </summary>
        /// <param name="item">User updating.</param>
        /// <returns>Void return.</returns>
        public async Task<IdentityResult> Update(UserDTO item)
        {
            return await _userManager.UpdateAsync(item);
        }
    }
}
