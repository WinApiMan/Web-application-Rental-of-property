// <copyright file="SettingsController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.Controllers.Settings
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Controller for application settings.
    /// </summary>
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsController"/> class.
        /// </summary>
        /// <param name="logger">Logger for exceptions writing.</param>
        public SettingsController(ILogger<SettingsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Set language in ui.
        /// </summary>
        /// <param name="culture">Language culture.</param>
        /// <param name="returnUrl">Result url.</param>
        /// <returns>Action result object.</returns>
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            try
            {
                const int YearsForSettings = 1;

                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(YearsForSettings),
                    });

                return Ok(returnUrl);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Set language error: {exception.Message}");
                return BadRequest("Language not found");
            }
        }
    }
}
