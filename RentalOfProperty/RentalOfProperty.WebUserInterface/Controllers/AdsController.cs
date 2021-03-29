namespace RentalOfProperty.WebUserInterface.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using RentalOfProperty.BusinessLogicLayer.Interfaces;
    using RentalOfProperty.WebUserInterface.Enums;
    using BLLLoadDataFromSourceMenu = BusinessLogicLayer.Enums.LoadDataFromSourceMenu;

    /// <summary>
    /// Ads controller.
    /// </summary>
    public class AdsController : Controller
    {
        private readonly ILogger<AdsController> _logger;

        private readonly IAdsManager _adsManager;

        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdsController"/> class.
        /// </summary>
        /// <param name="adsManager">Ads manager model.</param>
        /// <param name="mapper">Mapper model..</param>
        /// <param name="logger">Error logger.</param>
        public AdsController(IAdsManager adsManager, IMapper mapper, ILogger<AdsController> logger)
        {
            _adsManager = adsManager;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Load data from source post request.
        /// </summary>
        /// <param name="dataSourceMenuItem">Data source menu item.</param>
        /// <returns>Action result object.</returns>
        [Authorize(Roles = UserRoles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadDataFromSource(LoadDataFromSourceMenu dataSourceMenuItem)
        {
            try
            {
                await _adsManager.LoadLongTermAdsFromGoHomeBy(_mapper.Map<BLLLoadDataFromSourceMenu>(dataSourceMenuItem));
                return RedirectToAction("Index", "Home");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
