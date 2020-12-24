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

        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        private readonly IMailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="logger">Error loger.</param>
        /// <param name="mapper">Models mapper.</param>
        /// <param name="usersManager">User manager object.</param>
        /// <param name="localizer">User controller localizer.</param>
        /// <param name="sharedLocalizer">Shared localizer.</param>
        /// <param name="emailService">Email service.</param>
        public UsersController(ILogger<UsersController> logger, IMapper mapper, IUsersManager usersManager, IStringLocalizer<UsersController> localizer, IStringLocalizer<SharedResource> sharedLocalizer, IMailService emailService)
        {
            _logger = logger;
            _mapper = mapper;
            _usersManager = usersManager;
            _localizer = localizer;
            _sharedLocalizer = sharedLocalizer;
            _emailService = emailService;
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
                const string UserRole = "User", DefaultAccountImagePath = "~/Files/Images/DefaultAccount.png";
                if (ModelState.IsValid)
                {
                    var user = _mapper.Map<User>(registerModel);
                    user.AvatarImagePath = DefaultAccountImagePath;
                    user.Id = Guid.NewGuid().ToString();

                    var createResult = await _usersManager.Create(user, UserRole);

                    if (createResult.IsSuccessed)
                    {
                        user = _usersManager.FindByEmail(user.Email);
                        string code = await _usersManager.GenerateEmailConfirmationTokenAsync(user);

                        var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new
                        {
                            userId = user.Id,
                            code,
                        },
                        protocol: HttpContext.Request.Scheme);

                        await _emailService.SendEmailAsync(user.Email, "Confirm your account", $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>", "Администрация сайта");
                        return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
                    }
                    else
                    {
                        foreach (var error in createResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, _sharedLocalizer[error]);
                        }
                    }
                }

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
