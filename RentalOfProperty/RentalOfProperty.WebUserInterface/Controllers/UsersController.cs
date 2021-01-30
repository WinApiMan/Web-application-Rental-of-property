﻿// <copyright file="UsersController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Controllers
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Configuration;
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

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="logger">Error loger.</param>
        /// <param name="mapper">Models mapper.</param>
        /// <param name="usersManager">User manager object.</param>
        /// <param name="localizer">User controller localizer.</param>
        /// <param name="sharedLocalizer">Shared localizer.</param>
        /// <param name="emailService">Email service.</param>
        /// <param name="configuration">Application configuration.</param>
        public UsersController(ILogger<UsersController> logger, IMapper mapper, IUsersManager usersManager, IStringLocalizer<UsersController> localizer, IStringLocalizer<SharedResource> sharedLocalizer, IMailService emailService, IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _usersManager = usersManager;
            _localizer = localizer;
            _sharedLocalizer = sharedLocalizer;
            _emailService = emailService;
            _configuration = configuration;
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
        /// Authorization get request.
        /// </summary>
        /// <returns>Action result object.</returns>
        public IActionResult Authorization()
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
                const string UserRole = "User";
                if (ModelState.IsValid)
                {
                    var user = _mapper.Map<User>(registerModel);
                    user.Id = Guid.NewGuid().ToString();

                    var createResult = await _usersManager.Create(user, UserRole);

                    if (createResult.Succeeded)
                    {
                        user = _usersManager.FindByEmail(user.Email);
                        if (user is null)
                        {
                            return RedirectToAction("Error", "Home");
                        }
                        else
                        {
                            string code = await _usersManager.GenerateEmailConfirmationTokenAsync(user.Id);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                            var callbackUrl = Url.Action(
                            "ConfirmEmail",
                            "Users",
                            new
                            {
                                userId = user.Id,
                                code,
                            },
                            protocol: HttpContext.Request.Scheme);

                            // Create email message
                            var message = new EmailMessage()
                            {
                                EmailAdress = user.Email,
                                Message = $"{_localizer["ConfirmRegistration"]} <a href='{callbackUrl}'>link</a>",
                                Subject = _localizer["EmailSubject"],
                                SenderName = _localizer["EmailSenderName"],
                            };

                            // Create email sender settings
                            var sendEmailSetting = new SendEmailSetting()
                            {
                                SenderAdress = _configuration.GetValue<string>("MailSender:Email"),
                                SenderPassword = _configuration.GetValue<string>("MailSender:Password"),
                                Host = _configuration.GetValue<string>("MailSender:Host"),
                                Port = _configuration.GetValue<int>("MailSender:Port"),
                                SocketOptions = _configuration.GetValue<int>("MailSender:SocketOptions"),
                            };

                            await _emailService.SendEmailAsync(message, sendEmailSetting);

                            return Content(_localizer["RegistrationFinish"]);
                        }
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

        /// <summary>
        /// Confirm email address.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="code">Confirmation string.</param>
        /// <returns>Action result object.</returns>
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            try
            {
                if (userId is null || code is null)
                {
                    return RedirectToAction("Error", "Home");
                }
                else
                {
                    var user = await _usersManager.FindById(userId);
                    code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                    var result = await _usersManager.ConfirmEmailAsync(userId, code);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Authorization));
                    }
                    else
                    {
                        return RedirectToAction("Error", "Home");
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// User authorization.
        /// </summary>
        /// <param name="authorizeModel">Authorization data.</param>
        /// <returns>Action result object.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authorization(AuthorizeView authorizeModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check email confirm
                    if (await _usersManager.IsEmailConfirmedAsync(authorizeModel.Login))
                    {
                        var signInUser = _mapper.Map<SignInUser>(authorizeModel);
                        signInUser.IsLockoutOnFailure = false;

                        var signInResult = await _usersManager.PasswordSignInAsync(signInUser);

                        if (signInResult.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, _localizer["IncorrectLoginOrPassword"]);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, _localizer["EmailIsNotConfirmed"]);
                    }
                }
            }
            catch (NullReferenceException)
            {
                ModelState.AddModelError(string.Empty, _localizer["UserIsNotExist"]);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
                return View();
            }

            return View(authorizeModel);
        }

        /// <summary>
        /// Account log off.
        /// </summary>
        /// <returns>Action resul object.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            // удаляем аутентификационные куки
            await _usersManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
