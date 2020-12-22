// <copyright file="UsersRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using RentalOfProperty.DataAccessLayer.Interfaces;
    using RentalOfProperty.DataAccessLayer.Models;

    /// <summary>
    /// Users main operations.
    /// </summary>
    public class UsersRepository : IRepository<UserDTO>
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
        /// Method for adding item in table.
        /// </summary>
        /// <param name="item">Adding item.</param>
        /// <returns>Void return.</returns>
        public async Task Create(UserDTO item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method for find item in table.
        /// </summary>
        /// <param name="id">Primary key.</param>
        /// <returns>Result item.</returns>
        public async Task<UserDTO> FindById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method for getting all items from table.
        /// </summary>
        /// <returns>All items.</returns>
        public async Task<IEnumerable<UserDTO>> Get()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method for getting items from table with some filter.
        /// </summary>
        /// <param name="predicate">Search filter.</param>
        /// <returns>Result items.</returns>
        public async Task<IEnumerable<UserDTO>> Get(Func<UserDTO, bool> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Metohd for removing an item from table.
        /// </summary>
        /// <param name="item">Removing item.</param>
        /// <returns>Void return.</returns>
        public Task Remove(UserDTO item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method for updating item.
        /// </summary>
        /// <param name="item">Updating item.</param>
        /// <returns>Void return.</returns>
        public Task Update(UserDTO item)
        {
            throw new NotImplementedException();
        }
    }
}
