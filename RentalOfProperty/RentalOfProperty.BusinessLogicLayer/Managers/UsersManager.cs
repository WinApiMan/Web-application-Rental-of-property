// <copyright file="UsersManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using RentalOfProperty.BusinessLogicLayer.Interfaces;
    using RentalOfProperty.BusinessLogicLayer.Models;
    using RentalOfProperty.DataAccessLayer.Interfaces;
    using RentalOfProperty.DataAccessLayer.Models;

    /// <summary>
    /// User manager.
    /// </summary>
    public class UsersManager : IUsersManager
    {
        private readonly IUserRepository<UserDTO> _usersRepository;

        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersManager"/> class.
        /// </summary>
        /// <param name="usersRepository">User repository object.</param>
        /// <param name="mapper">Automapper object.</param>
        public UsersManager(IUserRepository<UserDTO> usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Add user.
        /// </summary>
        /// <param name="item">New user.</param>
        /// <param name="role">His role.</param>
        /// <returns>Errors list.</returns>
        public async Task<IdentityResult> Create(User item, string role)
        {
            var createResult = await _usersRepository.Create(_mapper.Map<UserDTO>(item), item.Password, role);
            return new IdentityResult()
            {
                IsSuccessed = createResult.Succeeded,
                Errors = createResult.Errors.Select(item => item.Description),
            };
        }

        public async Task<User> FindById(string id)
        {
            var user = await _usersRepository.FindById(id);

            if (user is null)
            {
                throw new NullReferenceException("User not found");
            }
            else
            {
                return null;
            }
        }

        public Task<IEnumerable<User>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> Get(Func<User, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task Remove(User item)
        {
            throw new NotImplementedException();
        }

        public Task Update(User item)
        {
            throw new NotImplementedException();
        }
    }
}
