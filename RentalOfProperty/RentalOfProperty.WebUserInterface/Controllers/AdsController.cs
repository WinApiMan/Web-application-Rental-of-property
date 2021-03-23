namespace RentalOfProperty.WebUserInterface.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using RentalOfProperty.WebUserInterface.Enums;

    /// <summary>
    /// Ads controller.
    /// </summary>
    public class AdsController : Controller
    {
        private readonly ILogger<AdsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdsController"/> class.
        /// </summary>
        /// <param name="logger">Error logger.</param>
        public AdsController(ILogger<AdsController> logger)
        {
            _logger = logger;
        }

        [Authorize(Roles = UserRoles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadDataFromSource(LoadDataFromSourceMenu dataSourceMenuItem)
        {
            try
            {
                switch (dataSourceMenuItem)
                {
                    case LoadDataFromSourceMenu.GoHomeByLongTermAds:
                        break;

                    case LoadDataFromSourceMenu.RealtByLongTermAds:
                        break;

                    case LoadDataFromSourceMenu.GoHomeByDailyAds:
                        break;

                    case LoadDataFromSourceMenu.RealtByDailyAds:
                        break;

                    default:
                        break;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
