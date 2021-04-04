﻿namespace RentalOfProperty.WebUserInterface.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using RentalOfProperty.BusinessLogicLayer.Interfaces;
    using RentalOfProperty.BusinessLogicLayer.Models;
    using RentalOfProperty.WebUserInterface.Enums;
    using RentalOfProperty.WebUserInterface.Models.Ad;
    using BLLLoadDataFromSourceMenu = BusinessLogicLayer.Enums.LoadDataFromSourceMenu;
    using BLLAdsTypeMenu = BusinessLogicLayer.Enums.AdsTypeMenu;

    /// <summary>
    /// Ads controller.
    /// </summary>
    public class AdsController : Controller
    {
        private const int DefaultPage = 1;

        private const int PageSize = 30;

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
        /// Long term rent get query.
        /// </summary>
        /// <param name="adsTypeMenuItem">Ads type menu item.</param>
        /// <param name="pageNumber">Current page number.</param>
        /// <returns>Task result.</returns>
        public async Task<IActionResult> AdsByType(AdsTypeMenu adsTypeMenuItem, int pageNumber = DefaultPage)
        {
            var ads = await _adsManager.GetAdsForPage(_mapper.Map<BLLAdsTypeMenu>(adsTypeMenuItem), pageNumber, PageSize);
            var adViews = new List<AdView>();

            foreach (var ad in ads)
            {
                if (ad is DailyRentalAd)
                {
                    adViews.Add(new AdView
                    {
                        RentalAdView = _mapper.Map<DailyRentalAdView>(ad as DailyRentalAd),
                        HousingPhotos = _mapper.Map<IEnumerable<HousingPhotoView>>(await _adsManager.GetHousingPhotosByRentalAdId(ad.Id)),
                    });
                }
                else
                {
                    adViews.Add(new AdView
                    {
                        RentalAdView = _mapper.Map<LongTermRentalAdView>(ad as LongTermRentalAd),
                        HousingPhotos = _mapper.Map<IEnumerable<HousingPhotoView>>(await _adsManager.GetHousingPhotosByRentalAdId(ad.Id)),
                    });
                }
            }

            ViewBag.AdsTypeMenuItem = adsTypeMenuItem;

            return View(new AdsPageView
            {
                AdViews = adViews,
                PageInfo = new PageInfo
                {
                    PageNumber = pageNumber,
                    PageSize = PageSize,
                    TotalItems = _adsManager.GetRentalAdsCount(_mapper.Map<BLLAdsTypeMenu>(adsTypeMenuItem)),
                },
                IsSuccess = false,
            });
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
                const bool IsLoadSuccess = true;
                await _adsManager.LoadLongTermAdsFromGoHomeBy(_mapper.Map<BLLLoadDataFromSourceMenu>(dataSourceMenuItem));

                var ads = await _adsManager.GetAdsForPage(DefaultPage, PageSize);
                var adViews = new List<AdView>();

                foreach (var ad in ads)
                {
                    if (ad is DailyRentalAd)
                    {
                        adViews.Add(new AdView
                        {
                            RentalAdView = _mapper.Map<DailyRentalAdView>(ad as DailyRentalAd),
                            HousingPhotos = _mapper.Map<IEnumerable<HousingPhotoView>>(await _adsManager.GetHousingPhotosByRentalAdId(ad.Id)),
                        });
                    }
                    else
                    {
                        adViews.Add(new AdView
                        {
                            RentalAdView = _mapper.Map<LongTermRentalAdView>(ad as LongTermRentalAd),
                            HousingPhotos = _mapper.Map<IEnumerable<HousingPhotoView>>(await _adsManager.GetHousingPhotosByRentalAdId(ad.Id)),
                        });
                    }
                }

                return PartialView("_AdsPage", new AdsPageView
                {
                    AdViews = adViews,
                    PageInfo = new PageInfo
                    {
                        PageNumber = DefaultPage,
                        PageSize = PageSize,
                        TotalItems = _adsManager.GetRentalAdsCount(),
                    },
                    IsSuccess = IsLoadSuccess,
                });
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
