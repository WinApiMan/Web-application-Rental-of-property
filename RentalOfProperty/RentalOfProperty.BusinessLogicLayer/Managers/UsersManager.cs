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
    using DALIdentityResult = Microsoft.AspNetCore.Identity.IdentityResult;

    /// <summary>
    /// User manager.
    /// </summary>
    public class UsersManager : IUsersManager
    {
        private const int IncorrectUsersCount = 0;

        private readonly IUserRepository _usersRepository;

        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersManager"/> class.
        /// </summary>
        /// <param name="usersRepository">User repository object.</param>
        /// <param name="mapper">Automapper object.</param>
        public UsersManager(IUserRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Add user.
        /// </summary>
        /// <param name="user">New user.</param>
        /// <param name="role">His role.</param>
        /// <returns>Errors list.</returns>
        public async Task<IdentityResult> Create(User user, string role)
        {
            var identityResult = await _usersRepository.Create(_mapper.Map<UserDTO>(user), user.Password, role);
            return CreateIdentityResult(identityResult);
        }

        /// <summary>
        /// Find user by id.
        /// </summary>
        /// <param name="id">Primary key.</param>
        /// <returns>User object.</returns>
        public async Task<User> FindById(string id)
        {
            var user = await _usersRepository.FindById(id);

            if (user is null)
            {
                throw new NullReferenceException("User not found");
            }
            else
            {
                return _mapper.Map<User>(user);
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

        /// <summary>
        /// Method for updating item.
        /// </summary>
        /// <param name="user">User object.</param>
        /// <returns>Void return.</returns>
        public async Task<IdentityResult> Update(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException("User is null");
            }
            else
            {
                var users = _usersRepository.Get(item => item.Email.Equals(user.Email));

                if (users.Count() == IncorrectUsersCount)
                {
                    throw new NullReferenceException("Users is null");
                }
                else
                {
                    var userDTO = users.First();
                    userDTO.AvatarImagePath = user.AvatarImagePath;
                    userDTO.Email = user.Email;
                    userDTO.FullName = user.FullName;
                    userDTO.PhoneNumber = user.PhoneNumber;

                    var identityResult = await _usersRepository.Update(userDTO);

                    return CreateIdentityResult(identityResult);
                }
            }
        }

        /// <summary>
        /// Find user by email.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <returns>User object.</returns>
        public User FindByEmail(string email)
        {
            if (!(email is null))
            {
                var users = _mapper.Map<IEnumerable<User>>(_usersRepository.Get(item => item.Email.Equals(email)));

                if (users.Count() == IncorrectUsersCount)
                {
                    throw new NullReferenceException("Users is null");
                }
                else
                {
                    return users.First();
                }
            }
            else
            {
                throw new ArgumentNullException("Email is null");
            }
        }

        /// <summary>
        /// Generate token.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>Token.</returns>
        public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            if (!(userId is null))
            {
                var user = await _usersRepository.FindById(userId);

                if (user is null)
                {
                    throw new NullReferenceException("User is null");
                }
                else
                {
                    return await _usersRepository.GenerateEmailConfirmationTokenAsync(user);
                }
            }
            else
            {
                throw new ArgumentNullException("User is null");
            }
        }

        /// <summary>
        /// Confirm user email.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="code">Confirmation string.</param>
        /// <returns>Identity result object.</returns>
        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            if (userId is null)
            {
                throw new ArgumentNullException("User id is null");
            }
            else if (code is null)
            {
                throw new ArgumentNullException("Code is null");
            }
            else
            {
                var user = await _usersRepository.FindById(userId);
                if (user is null)
                {
                    throw new NullReferenceException("User is null");
                }
                else
                {
                    var identityResult = await _usersRepository.ConfirmEmailAsync(user, code);
                    return CreateIdentityResult(identityResult);
                }
            }
        }

        /// <summary>
        /// Check email confirm.
        /// </summary>
        /// <param name="login">User login.</param>
        /// <returns>Confirm result(true or false).</returns>
        public async Task<bool> IsEmailConfirmedAsync(string login)
        {
            if (login is null)
            {
                throw new ArgumentNullException("User id is null");
            }
            else
            {
                var users = _usersRepository.Get(item => item.Email.Equals(login));

                if (users.Count() == IncorrectUsersCount)
                {
                    throw new NullReferenceException("User is null");
                }
                else
                {
                    return users.Count() == IncorrectUsersCount ? false : await _usersRepository.IsEmailConfirmedAsync(users.First());
                }
            }
        }

        /// <summary>
        /// Sign in account.
        /// </summary>
        /// <param name="user">Sign in user object.</param>
        /// <returns>Password signInResult(true or false).</returns>
        public async Task<SignInResult> PasswordSignInAsync(SignInUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException("Sign in user is null");
            }
            else
            {
                return _mapper.Map<SignInResult>(await _usersRepository.PasswordSignInAsync(_mapper.Map<SignInUserDTO>(user)));
            }
        }

        /// <summary>
        /// Account log off.
        /// </summary>
        /// <returns>Task result.</returns>
        public async Task SignOutAsync()
        {
            await _usersRepository.SignOutAsync();
        }

        /// <summary>
        /// Create business logic identity result.
        /// </summary>
        /// <param name="identityResult">Data access layer identity result.</param>
        /// <returns>Bll identity result.</returns>
        public IdentityResult CreateIdentityResult(DALIdentityResult identityResult)
        {
            var errors = new List<string>();

            foreach (var error in identityResult.Errors)
            {
                errors.Add(error.Description);
            }

            return new IdentityResult
            {
                Succeeded = identityResult.Succeeded,
                Errors = errors,
            };
        }
    }
}
