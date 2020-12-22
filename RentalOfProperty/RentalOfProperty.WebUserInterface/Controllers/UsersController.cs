// <copyright file="UsersController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using RentalOfProperty.BusinessLogicLayer.Interfaces;
    using RentalOfProperty.BusinessLogicLayer.Models;
    using RentalOfProperty.WebUserInterface.Models;
    using RentalOfProperty.WebUserInterface.Models.User;

    /// <summary>
    /// User controller.
    /// </summary>
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;

        private readonly IMapper _mapper;

        private readonly IUsersManager _usersManager;

        private readonly IStringLocalizer<UsersController> _localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="logger">Error loger.</param>
        /// <param name="mapper">Models mapper.</param>
        /// <param name="usersManager">User manager object.</param>
        public UsersController(ILogger<UsersController> logger, IMapper mapper, IUsersManager usersManager, IStringLocalizer<UsersController> localizer)
        {
            _logger = logger;
            _mapper = mapper;
            _usersManager = usersManager;
            _localizer = localizer;
        }

        /// <summary>
        /// Registration get request.
        /// </summary>
        /// <returns>Action result object.</returns>
        public IActionResult Registration()
        {
            return View();
        }

        /// <summary>
        /// Registration post request.
        /// </summary>
        /// <param name="registerModel">Register view model.</param>
        /// <returns>Action result object.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegisterView registerModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _mapper.Map<User>(registerModel);

                    await _usersManager.Create(user, Roles.User);

                    return RedirectToAction("Index", "Home");
                }

                return View(registerModel);
            }
            catch (ArgumentException)
            {
                ModelState.AddModelError(string.Empty, _localizer["CreateUserError"]);
                return View(registerModel);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
                return View();
            }
        }
    }
}
