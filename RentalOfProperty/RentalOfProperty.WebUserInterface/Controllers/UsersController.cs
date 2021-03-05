// <copyright file="UsersController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Controllers
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using RentalOfProperty.BusinessLogicLayer.Interfaces;
    using RentalOfProperty.BusinessLogicLayer.Models;
    using RentalOfProperty.WebUserInterface.Models.User;

    using IOFIle = System.IO.File;

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

        private readonly IWebHostEnvironment _applicationEnvironment;

        private const string DefaultAvatarImagePath = @"\Files\Images\DefaultAccount.png";

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
        /// <param name="applicationEnvironment">Application enviroment.</param>
        public UsersController(ILogger<UsersController> logger, IMapper mapper, IUsersManager usersManager, IStringLocalizer<UsersController> localizer, IStringLocalizer<SharedResource> sharedLocalizer, IMailService emailService, IConfiguration configuration, IWebHostEnvironment applicationEnvironment)
        {
            _logger = logger;
            _mapper = mapper;
            _usersManager = usersManager;
            _localizer = localizer;
            _sharedLocalizer = sharedLocalizer;
            _emailService = emailService;
            _configuration = configuration;
            _applicationEnvironment = applicationEnvironment;
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
        /// Edit user get request.
        /// </summary>
        /// <returns>Action result object.</returns>
        [Authorize]
        public IActionResult EditUserData()
        {
            try
            {
                var user = _mapper.Map<EditView>(_usersManager.FindByEmail(User.Identity.Name));

                if (!(user is null))
                {
                    return View(user);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
            }

            return RedirectToAction("Error", "Home");
        }

        /// <summary>
        /// Change user password get request.
        /// </summary>
        /// <returns>Action result object.</returns>
        [Authorize]
        public IActionResult ChangePassword()
        {
            try
            {
                if (!(_usersManager.FindByEmail(User.Identity.Name) is null))
                {
                    return View();
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
            }

            return RedirectToAction("Error", "Home");
        }

        /// <summary>
        /// Forgot password get request.
        /// </summary>
        /// <returns>Action result object.</returns>
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Reset password get request.
        /// </summary>
        /// <param name="code">Generated code.</param>
        /// <returns>Action result object.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code is null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                return View();
            }
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
                    user.AvatarImagePath = DefaultAvatarImagePath;

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

                            await _emailService.SendEmailAsync(message, GetBasicSendEmailSetting());

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

        /// <summary>
        /// Load avatar in site.
        /// </summary>
        /// <param name="file">Image.</param>
        /// <returns>Action result object.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadAvatarImage(IFormFile file)
        {
            try
            {
                const string ImagePath = @"\Files\Images\";
                const int IncorrectIndex = -1;

                if (!(file is null))
                {
                    // Path to file
                    string filePath = string.Concat(ImagePath, file.FileName);
                    string currentFilePath = string.Concat(_applicationEnvironment.WebRootPath, filePath);

                    if (Array.IndexOf(Directory.GetFiles(string.Concat(_applicationEnvironment.WebRootPath, ImagePath)), currentFilePath) == IncorrectIndex)
                    {
                        var user = _usersManager.FindByEmail(User.Identity.Name);

                        // Save file to /Files/Images in wwwroot
                        using (var fileStream = new FileStream(currentFilePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);

                            // Check default image
                            if (!user.AvatarImagePath.Equals(DefaultAvatarImagePath))
                            {
                                IOFIle.Delete(string.Concat(_applicationEnvironment.WebRootPath, user.AvatarImagePath));
                            }
                        }

                        user.AvatarImagePath = filePath;
                        await _usersManager.Update(user);
                    }
                    else
                    {
                        _logger.LogError("Error : file is null");
                    }
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// Get path to user avatar.
        /// </summary>
        /// <returns>Avatar path.</returns>
        [Authorize]
        [HttpPost]
        public IActionResult GetAvatarPath()
        {
            try
            {
                var user = _usersManager.FindByEmail(User.Identity.Name);
                return Ok(user.AvatarImagePath);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
                return BadRequest("Load image error.");
            }
        }

        /// <summary>
        /// Edit user data.
        /// </summary>
        /// <param name="editView">Edit view model.</param>
        /// <returns>Action result object.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserData(EditView editView)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _usersManager.FindByEmail(User.Identity.Name);

                    user.FullName = editView.FullName;
                    user.PhoneNumber = editView.PhoneNumber;

                    var identityResult = await _usersManager.Update(user);

                    if (identityResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, _sharedLocalizer[error]);
                        }
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
            }

            return View(editView);
        }

        /// <summary>
        /// Change user password.
        /// </summary>
        /// <param name="changePasswordView">Change password view model.</param>
        /// <returns>Action result object.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordView changePasswordView)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _usersManager.FindByEmail(User.Identity.Name);
                    var changeResult = await _usersManager.ChangePasswordAsync(user, changePasswordView.OldPassword, changePasswordView.NewPassword);

                    if (changeResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in changeResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, _sharedLocalizer[error]);
                        }
                    }
                }
            }
            catch (ArgumentNullException)
            {
                ModelState.AddModelError(string.Empty, _localizer["UserIsNotExist"]);
            }
            catch (NullReferenceException)
            {
                ModelState.AddModelError(string.Empty, _localizer["UserIsNotExist"]);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
                return RedirectToAction("Error", "Home");
            }

            return View(changePasswordView);
        }

        /// <summary>
        /// Restore user password with usung email.
        /// </summary>
        /// <param name="forgotPasswordView">Forgot password view model.</param>
        /// <returns>Action result object.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordView forgotPasswordView)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _usersManager.FindByEmail(forgotPasswordView.Email);

                    if (await _usersManager.IsEmailConfirmedAsync(forgotPasswordView.Email))
                    {
                        string code = await _usersManager.GeneratePasswordResetTokenAsync(user.Id);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                        var callbackUrl = Url.Action(
                           "ResetPassword",
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
                            Message = $"{_localizer["ConfirmResetPassword"]} <a href='{callbackUrl}'>link</a>",
                            Subject = _localizer["EmailResetPasswordSubject"],
                            SenderName = _localizer["EmailSenderName"],
                        };

                        await _emailService.SendEmailAsync(message, GetBasicSendEmailSetting());
                    }

                    return Content(_localizer["RestoreFinish"]);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
                return Content(_localizer["RestoreFinish"]);
            }

            return View(forgotPasswordView);
        }

        /// <summary>
        /// Resert password. Change old data to new data.
        /// </summary>
        /// <param name="resetPasswordView">Reset password view model.</param>
        /// <returns>Action result object.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordView resetPasswordView)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _usersManager.FindByEmail(resetPasswordView.Email);
                    resetPasswordView.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordView.Code));
                    var resetResult = await _usersManager.ResetPasswordAsync(user.Id, resetPasswordView.Code, resetPasswordView.Password);

                    if (resetResult.Succeeded)
                    {
                        return RedirectToAction(nameof(Authorization));
                    }
                    else
                    {
                        foreach (var error in resetResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, _sharedLocalizer[error]);
                        }
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

            return View(resetPasswordView);
        }

        /// <summary>
        /// Get basic send email setting.
        /// </summary>
        /// <returns>Email setting.</returns>
        public SendEmailSetting GetBasicSendEmailSetting()
        {
            return new SendEmailSetting()
            {
                SenderAdress = _configuration.GetValue<string>("MailSender:Email"),
                SenderPassword = _configuration.GetValue<string>("MailSender:Password"),
                Host = _configuration.GetValue<string>("MailSender:Host"),
                Port = _configuration.GetValue<int>("MailSender:Port"),
                SocketOptions = _configuration.GetValue<int>("MailSender:SocketOptions"),
            };
        }
    }
}
