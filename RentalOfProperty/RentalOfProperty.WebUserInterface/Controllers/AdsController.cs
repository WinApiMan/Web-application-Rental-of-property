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
    using RentalOfProperty.WebUserInterface.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Hosting;
    using System.IO;
    using Microsoft.Extensions.Localization;

    /// <summary>
    /// Ads controller.
    /// </summary>
    public class AdsController : Controller
    {
        private const int DefaultPage = 1;

        private const int PageSize = 30;

        private readonly ILogger<AdsController> _logger;

        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        private readonly IAdsManager _adsManager;

        private readonly IUsersManager _usersManager;

        private readonly IWebHostEnvironment _applicationEnvironment;

        private readonly IMapper _mapper;

        public const string ImagePath = @"\Files\RentalOfPropertyPhotos\";

        /// <summary>
        /// Initializes a new instance of the <see cref="AdsController"/> class.
        /// </summary>
        /// <param name="adsManager">Ads manager model.</param>
        /// <param name="usersManager">Users manager model.</param>
        /// <param name="mapper">Mapper model..</param>
        /// <param name="logger">Error logger.</param>
        public AdsController(IAdsManager adsManager, IUsersManager usersManager, IMapper mapper, ILogger<AdsController> logger, IWebHostEnvironment applicationEnvironment)
        {
            _adsManager = adsManager;
            _usersManager = usersManager;
            _mapper = mapper;
            _logger = logger;
            _applicationEnvironment = applicationEnvironment;
        }

        /// <summary>
        /// All info about rental ad.
        /// </summary>
        /// <param name="id">Unique key.</param>
        /// <returns>Action result object.</returns>
        public async Task<IActionResult> RentalAd(string id)
        {
            try
            {
                var allRentalAdInfo = await _adsManager.GetAllRentalAdInfo(id);
                var allRentalAdInfoView = new AllRentalAdInfoView
                {
                    AditionalAdData = _mapper.Map<AditionalAdDataView>(allRentalAdInfo.AditionalAdData),
                    ContactPerson = _mapper.Map<ContactPersonView>(allRentalAdInfo.ContactPerson),
                    Photos = _mapper.Map<IEnumerable<HousingPhotoView>>(allRentalAdInfo.Photos),
                    IsOriginal = allRentalAdInfo.IsOriginal,
                };
                var userId = User.Identity.Name is null ? default : _usersManager.FindByEmail(User.Identity.Name).Id;
                var userFavoriteAds = userId is null ? default : await _adsManager.GetUserFavoriteAds(userId);

                if (allRentalAdInfo.RentalAd is DailyRentalAd)
                {
                    allRentalAdInfoView.RentalAd = _mapper.Map<DailyRentalAdView>(allRentalAdInfo.RentalAd);
                }
                else
                {
                    allRentalAdInfoView.RentalAd = _mapper.Map<LongTermRentalAdView>(allRentalAdInfo.RentalAd);
                }

                if (!(userFavoriteAds is null))
                {
                    allRentalAdInfoView.RentalAd.IsFavorite = userFavoriteAds.FirstOrDefault(userAd => userAd.RentalAdId.Equals(allRentalAdInfoView.RentalAd.Id) && userAd.UserId.Equals(userId)) is null ? false : true;
                }

                allRentalAdInfoView.MapPlace = new MapPlace
                {
                    GeoLong = allRentalAdInfo.RentalAd.XMapCoordinate,
                    GeoLat = allRentalAdInfo.RentalAd.YMapCoordinate,
                };

                return View(allRentalAdInfoView);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Create view.
        /// </summary>
        /// <returns>Redirect to create page.</returns>
        [Authorize]
        public IActionResult Create()
        {
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
            return View();
        }

        /// <summary>
        /// Edit view.
        /// </summary>
        /// <param name="id">Unique key.</param>
        /// <returns>Redirect to edit page.</returns>
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var user = _usersManager.FindByEmail(User.Identity.Name);
                var ad = await _adsManager.GetAllRentalAdInfo(id);

                if (ad.RentalAd.UserId.Equals(user.Id) || User.IsInRole(UserRoles.Administrator))
                {
                    var editView = new CreateView
                    {
                        Id = ad.RentalAd.Id,
                        Address = ad.RentalAd.Address,
                        Bathroom = ad.RentalAd.Bathroom,
                        Description = ad.RentalAd.Description,
                        District = ad.RentalAd.District,
                        Facilities = ad.RentalAd.Facilities,
                        Floor = ad.RentalAd.Floor,
                        KitchenArea = ad.RentalAd.KitchenArea,
                        LandArea = ad.RentalAd.KitchenArea,
                        LivingArea = ad.RentalAd.LivingArea,
                        Locality = ad.RentalAd.Locality,
                        Notes = ad.RentalAd.Notes,
                        Region = ad.RentalAd.Region,
                        RentalType = (BLLAdsTypeMenu)ad.RentalAd.RentalType,
                        RentCountOfRooms = ad.RentalAd.RentCountOfRooms,
                        TotalArea = ad.RentalAd.TotalArea,
                        TotalCountOfRooms = ad.RentalAd.TotalCountOfRooms,
                        TotalFloors = ad.RentalAd.TotalFloors,
                        XMapCoordinate = ad.RentalAd.XMapCoordinate,
                        YMapCoordinate = ad.RentalAd.YMapCoordinate,
                    };

                    if (ad.RentalAd is DailyRentalAd)
                    {
                        var dailyAd = ad.RentalAd as DailyRentalAd;
                        editView.USDPricePerDay = dailyAd.USDPricePerDay;
                        editView.USDPricePerPerson = dailyAd.USDPricePerPerson;
                        editView.BYNPricePerDay = dailyAd.BYNPricePerDay;
                        editView.BYNPricePerPerson = dailyAd.BYNPricePerPerson;
                    }
                    else
                    {
                        var longTermAd = ad.RentalAd as LongTermRentalAd;
                        editView.BYNPrice = longTermAd.BYNPrice;
                        editView.USDPrice = longTermAd.USDPrice;
                    }

                    ViewBag.PhotosCount = ad.Photos.Count();

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

                    return View(editView);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
                return RedirectToAction("Index", "Home");
            }
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
            else if (adsTypeMenuItem == AdsTypeMenu.MyAds)
            {
                if (User.Identity.Name is null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var user = _usersManager.FindByEmail(User.Identity.Name);
                    ads = await _adsManager.GetUserAds(user.Id);
                    favoriteCount = ads.Count();

                    ads = ads
                        .Skip((pageNumber - DefaultPage) * PageSize)
                        .Take(PageSize);
                }
            }
            else if (adsTypeMenuItem == AdsTypeMenu.AdsForPublish)
            {
                if (User.IsInRole(UserRoles.Administrator))
                {
                    ads = await _adsManager.GetAdsForPublish();
                    favoriteCount = ads.Count();
                    ads = ads
                        .Skip((pageNumber - DefaultPage) * PageSize)
                        .Take(PageSize);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ads = await _adsManager.GetAdsForPage(_mapper.Map<BLLAdsTypeMenu>(adsTypeMenuItem), pageNumber, PageSize);
                favoriteCount = _adsManager.GetRentalAdsCount(_mapper.Map<BLLAdsTypeMenu>(adsTypeMenuItem));
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
                    TotalItems = favoriteCount,
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
        /// <param name="returnUrl">Return url.</param>
        /// <returns>Task result.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrRemoveFavorite(string rentalAdId, string returnUrl)
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

            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Redirect(returnUrl);
            }
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
            var ads = await _adsManager.LongTermSearch(_mapper.Map<LongTermSearch>(longTermSearchView), pageNumber, PageSize);

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
                    TotalItems = _adsManager.GetLongTermSearchCount(_mapper.Map<LongTermSearch>(longTermSearchView)),
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
            var ads = await _adsManager.DailySearch(_mapper.Map<DailySearch>(dailySearchView), pageNumber, PageSize);

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
                    TotalItems = _adsManager.GetDailySearchCount(_mapper.Map<DailySearch>(dailySearchView)),
                },
                DailySearch = dailySearchView,
                IsSuccess = false,
            });
        }

        /// <summary>
        /// Create ad.
        /// </summary>
        /// <param name="photos">Ad photos.</param>
        /// <param name="createView">Ad parametrs.</param>
        /// <returns>Redirect to action.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFileCollection photos, CreateView createView)
        {
            try
            {
                var urls = new List<string>();
                var user = _usersManager.FindByEmail(User.Identity.Name);

                foreach (var photo in photos)
                {
                    string filePath = string.Concat(ImagePath, $"{Guid.NewGuid()}{photo.FileName}");
                    string currentFilePath = string.Concat(_applicationEnvironment.WebRootPath, filePath);

                    urls.Add(filePath);

                    // Save file to /Files/Images in wwwroot
                    using var fileStream = new FileStream(currentFilePath, FileMode.Create);
                    await photo.CopyToAsync(fileStream);
                }

                string rentalAdId = await _adsManager.CreateAd(_mapper.Map<CreateModel>(createView), user.Id, $"https://{Request.Host.Value}", urls);

                return RedirectToAction("RentalAd", "Ads", new { id = rentalAdId });
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Remove ad.
        /// </summary>
        /// <param name="id">Ad unique key.</param>
        /// <returns>Action result object.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(string id)
        {
            try
            {
                var user = _usersManager.FindByEmail(User.Identity.Name);
                var photos = await _adsManager.GetHousingPhotosByRentalAdId(id);
                var photosPath = photos.Select(item => item.PathToPhoto);
                await _adsManager.Remove(id, user.Id, User.IsInRole(UserRoles.Administrator));

                foreach (var path in photosPath)
                {
                    System.IO.File.Delete($"{_applicationEnvironment.WebRootPath}{path}");
                }

                return RedirectToAction("AdsByType", "Ads", new { adsTypeMenuItem = AdsTypeMenu.MyAds });
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Edit ad.
        /// </summary>
        /// <param name="photos">Ad photos.</param>
        /// <param name="createView">Ad parametrs.</param>
        /// <returns>Redirect to action.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFileCollection photos, CreateView createView)
        {
            try
            {
                var user = _usersManager.FindByEmail(User.Identity.Name);
                var oldPhotos = await _adsManager.GetHousingPhotosByRentalAdId(createView.Id);
                var photosPath = oldPhotos.Select(item => item.PathToPhoto);

                foreach (var path in photosPath)
                {
                    System.IO.File.Delete($"{_applicationEnvironment.WebRootPath}{path}");
                }

                var photosUrl = photos.Select(photo => string.Concat(ImagePath, $"{Guid.NewGuid()}{photo.FileName}")).ToArray();

                await _adsManager.Edit(_mapper.Map<CreateModel>(createView), user.Id, User.IsInRole(UserRoles.Administrator), photosUrl);

                int index = 0;

                foreach (var photo in photos)
                {
                    string currentFilePath = string.Concat(_applicationEnvironment.WebRootPath, photosUrl[index]);

                    // Save file to /Files/Images in wwwroot
                    using var fileStream = new FileStream(currentFilePath, FileMode.Create);
                    await photo.CopyToAsync(fileStream);

                    index++;
                }

                return RedirectToAction("RentalAd", "Ads", new { id = createView.Id });
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Publish or unpublish ad.
        /// </summary>
        /// <param name="id">Ad unique key.</param>
        /// <returns>Publish result.</returns>
        [Authorize(Roles = UserRoles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Publish(string id)
        {
            try
            {
                await _adsManager.Publish(id);
                return RedirectToAction("RentalAd", "Ads", new { id });
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error : {exception.Message}");
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// General search.
        /// </summary>
        /// <param name="generalSearchView">General search model.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>Task result.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GeneralSearch(GeneralSearchView generalSearchView, int pageNumber = DefaultPage)
        {
            var ads = await _adsManager.GeneralSearch(_mapper.Map<GeneralSearch>(generalSearchView), pageNumber, PageSize);

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

            ViewBag.AdsTypeMenuItem = AdsTypeMenu.GeneralSearch;

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
                    TotalItems = _adsManager.GetGeneralSearchCount(_mapper.Map<GeneralSearch>(generalSearchView)),
                },
                GeneralSearch = generalSearchView,
                IsSuccess = false,
            });
        }
    }
}
