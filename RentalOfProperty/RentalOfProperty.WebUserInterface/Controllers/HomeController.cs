// <copyright file="HomeController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using RentalOfProperty.BusinessLogicLayer.Interfaces;
    using RentalOfProperty.BusinessLogicLayer.Models;
    using RentalOfProperty.Models;
    using RentalOfProperty.WebUserInterface.Models.Ad;

    /// <summary>
    /// Main.
    /// </summary>
    public class HomeController : Controller
    {
        private const int DefaultPage = 1;

        private const int PageSize = 30;

        private readonly ILogger<HomeController> _logger;

        private readonly IAdsManager _adsManager;

        private readonly IUsersManager _usersManager;

        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">Logger object.</param>
        /// <param name="adsManager">Ads manager.</param>
        /// <param name="usersManager">User manager.</param>
        /// <param name="mapper">Automapper object.</param>
        public HomeController(ILogger<HomeController> logger, IAdsManager adsManager, IUsersManager usersManager, IMapper mapper)
        {
            _logger = logger;
            _adsManager = adsManager;
            _usersManager = usersManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Index get request.
        /// </summary>
        /// <param name="pageNumber">Current page number.</param>
        /// <returns>Task result.</returns>
        public async Task<IActionResult> Index(int pageNumber = DefaultPage)
        {
            if (pageNumber < DefaultPage)
            {
                pageNumber = DefaultPage;
            }

            var ads = await _adsManager.GetAdsForPage(pageNumber, PageSize);
            var adViews = new List<AdView>();
            var userId = User.Identity.Name is null ? default : _usersManager.FindByEmail(User.Identity.Name).Id;
            var userFavoriteAds = userId is null ? default : await _adsManager.GetUserFavoriteAds(userId);

            foreach (var ad in ads)
            {
                if (ad is DailyRentalAd)
                {
                    adViews.Add(new AdView
                    {
                        RentalAdView = _mapper.Map<DailyRentalAdView>(ad as DailyRentalAd),
                        HousingPhotos = _mapper.Map<IEnumerable<HousingPhotoView>>(await _adsManager.GetHousingPhotosByRentalAdId(ad.Id)),
                    });

                    if (!(userFavoriteAds is null))
                    {
                        adViews.Last().RentalAdView.IsFavorite = userFavoriteAds.FirstOrDefault(userAd => userAd.RentalAdId.Equals(ad.Id) && userAd.UserId.Equals(userId)) is null ? false : true;
                    }
                }
                else
                {
                    adViews.Add(new AdView
                    {
                        RentalAdView = _mapper.Map<LongTermRentalAdView>(ad as LongTermRentalAd),
                        HousingPhotos = _mapper.Map<IEnumerable<HousingPhotoView>>(await _adsManager.GetHousingPhotosByRentalAdId(ad.Id)),
                    });

                    if (!(userFavoriteAds is null))
                    {
                        adViews.Last().RentalAdView.IsFavorite = userFavoriteAds.FirstOrDefault(userAd => userAd.RentalAdId.Equals(ad.Id) && userAd.UserId.Equals(userId)) is null ? false : true;
                    }
                }
            }

            return View(new AdsPageView
            {
                AdViews = adViews,
                PageInfo = new PageInfo
                {
                    PageNumber = pageNumber,
                    PageSize = PageSize,
                    TotalItems = _adsManager.GetRentalAdsCount(),
                },
                IsSuccess = false,
            });
        }

        /// <summary>
        /// Privacy.
        /// </summary>
        /// <returns>Result object.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Error.
        /// </summary>
        /// <returns>Result object.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
