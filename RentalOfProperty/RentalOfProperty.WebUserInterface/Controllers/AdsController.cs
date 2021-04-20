namespace RentalOfProperty.WebUserInterface.Controllers
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
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Rendering;

    /// <summary>
    /// Ads controller.
    /// </summary>
    public class AdsController : Controller
    {
        private const int DefaultPage = 1;

        private const int PageSize = 30;

        private readonly ILogger<AdsController> _logger;

        private readonly IAdsManager _adsManager;

        private readonly IUsersManager _usersManager;

        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdsController"/> class.
        /// </summary>
        /// <param name="adsManager">Ads manager model.</param>
        /// <param name="usersManager">Users manager model.</param>
        /// <param name="mapper">Mapper model..</param>
        /// <param name="logger">Error logger.</param>
        public AdsController(IAdsManager adsManager, IUsersManager usersManager, IMapper mapper, ILogger<AdsController> logger)
        {
            _adsManager = adsManager;
            _usersManager = usersManager;
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
            if (pageNumber < DefaultPage)
            {
                pageNumber = DefaultPage;
            }

            IEnumerable<RentalAd> ads = new List<RentalAd>();
            int favoriteCount = default;

            if (adsTypeMenuItem == AdsTypeMenu.FavoriteAds)
            {
                if (User.Identity.Name is null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var user = _usersManager.FindByEmail(User.Identity.Name);
                    ads = await _adsManager.GetFavoriteAds(user.Id);
                    favoriteCount = ads.Count();

                    ads = ads
                        .Skip((pageNumber - DefaultPage) * PageSize)
                        .Take(PageSize);
                }
            }
            else
            {
                ads = await _adsManager.GetAdsForPage(_mapper.Map<BLLAdsTypeMenu>(adsTypeMenuItem), pageNumber, PageSize);
            }

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

            ViewBag.AdsTypeMenuItem = adsTypeMenuItem;

            ViewBag.Regions = new SelectList(
                new List<string>
                {
                    "Любая область",
                    "Брестская область",
                    "Витебская область",
                    "Гомельская область",
                    "Гродненская область",
                    "Минская область",
                    "Могилёвская область",
                });

            ViewBag.RoomsCount = new SelectList(
                new List<string>
                {
                    string.Empty,
                    "1",
                    "2",
                    "3",
                    "4",
                    "5",
                    "6",
                    "7",
                    "8",
                    "9",
                });

            return View(new AdsPageView
            {
                AdViews = adViews,
                PageInfo = new PageInfo
                {
                    PageNumber = pageNumber,
                    PageSize = PageSize,
                    TotalItems = adsTypeMenuItem == AdsTypeMenu.FavoriteAds ? favoriteCount : _adsManager.GetRentalAdsCount(_mapper.Map<BLLAdsTypeMenu>(adsTypeMenuItem)),
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
            bool isLoadSuccess = true;
            var adViews = new List<AdView>();

            try
            {
                await _adsManager.LoadLongTermAdsFromGoHomeBy(_mapper.Map<BLLLoadDataFromSourceMenu>(dataSourceMenuItem));

                var ads = await _adsManager.GetAdsForPage(DefaultPage, PageSize);

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
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
                isLoadSuccess = false;
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
                IsSuccess = isLoadSuccess,
            });
        }

        /// <summary>
        /// Add ad to favourites.
        /// </summary>
        /// <param name="rentalAdId">Rental ad unique number.</param>
        /// <returns>Task result.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrRemoveFavorite(string rentalAdId)
        {
            try
            {
                var user = _usersManager.FindByEmail(User.Identity.Name);
                await _adsManager.AddOrRemoveFavorite(user.Id, rentalAdId);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Search long term ads.
        /// </summary>
        /// <param name="longTermSearchView">Object to search.</param>
        /// <param name="pageNumber">Ads page number.</param>
        /// <returns>Action result object.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LongTermSearch(LongTermSearchView longTermSearchView, int pageNumber = DefaultPage)
        {
            var ads = await _adsManager.LongTermSearch(_mapper.Map<LongTermSearch>(longTermSearchView));

            int adsCount = ads.Count();

            ads = ads
                .Skip((pageNumber - DefaultPage) * PageSize)
                .Take(PageSize);

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

            ViewBag.AdsTypeMenuItem = AdsTypeMenu.LongTermSearch;

            ViewBag.Regions = new SelectList(
                new List<string>
                {
                    "Любая область",
                    "Брестская область",
                    "Витебская область",
                    "Гомельская область",
                    "Гродненская область",
                    "Минская область",
                    "Могилёвская область",
                });

            ViewBag.RoomsCount = new SelectList(
                new List<string>
                {
                    string.Empty,
                    "1",
                    "2",
                    "3",
                    "4",
                    "5",
                    "6",
                    "7",
                    "8",
                    "9",
                });

            return View("AdsByType", new AdsPageView
            {
                AdViews = adViews,
                PageInfo = new PageInfo
                {
                    PageNumber = pageNumber,
                    PageSize = PageSize,
                    TotalItems = adsCount,
                },
                LongTermSearch = longTermSearchView,
                IsSuccess = false,
            });
        }

        /// <summary>
        /// Search daily ads.
        /// </summary>
        /// <param name="dailySearchView">Object to search.</param>
        /// <param name="pageNumber">Ads page number.</param>
        /// <returns>Action result object.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DailySearch(DailySearchView dailySearchView, int pageNumber = DefaultPage)
        {
            var ads = await _adsManager.DailySearch(_mapper.Map<DailySearch>(dailySearchView));

            int adsCount = ads.Count();

            ads = ads
                .Skip((pageNumber - DefaultPage) * PageSize)
                .Take(PageSize);

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

            ViewBag.AdsTypeMenuItem = AdsTypeMenu.DailySearch;

            ViewBag.Regions = new SelectList(
                new List<string>
                {
                    "Любая область",
                    "Брестская область",
                    "Витебская область",
                    "Гомельская область",
                    "Гродненская область",
                    "Минская область",
                    "Могилёвская область",
                });

            ViewBag.RoomsCount = new SelectList(
                new List<string>
                {
                    string.Empty,
                    "1",
                    "2",
                    "3",
                    "4",
                    "5",
                    "6",
                    "7",
                    "8",
                    "9",
                });

            return View("AdsByType", new AdsPageView
            {
                AdViews = adViews,
                PageInfo = new PageInfo
                {
                    PageNumber = pageNumber,
                    PageSize = PageSize,
                    TotalItems = adsCount,
                },
                DailySearch = dailySearchView,
                IsSuccess = false,
            });
        }
    }
}
