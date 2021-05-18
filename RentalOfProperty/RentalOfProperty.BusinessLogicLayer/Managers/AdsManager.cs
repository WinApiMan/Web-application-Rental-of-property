// <copyright file="AdsManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using RentalOfProperty.BusinessLogicLayer.Enums;
    using RentalOfProperty.BusinessLogicLayer.Interfaces;
    using RentalOfProperty.BusinessLogicLayer.Models;
    using RentalOfProperty.DataAccessLayer.Enums;
    using RentalOfProperty.DataAccessLayer.Interfaces;
    using RentalOfProperty.DataAccessLayer.Models;

    /// <summary>
    /// Ads manager.
    /// </summary>
    public class AdsManager : IAdsManager
    {
        private readonly IDataLoader _goHomeByDataLoader;

        private readonly IDataLoader _realtByDataLoader;

        private readonly IMapper _mapper;

        private readonly IRepository<AditionalAdDataDTO> _aditionalAdDatasRepository;

        private readonly IRepository<ContactPersonDTO> _contactPersonsRepository;

        private readonly IRepository<HousingPhotoDTO> _housingPhotosRepository;

        private readonly IRepository<RentalAdDTO> _rentalAdsRepository;

        private readonly IRepository<DailyRentalAdDTO> _dailyRentalAdsRepository;

        private readonly IRepository<LongTermRentalAdDTO> _longTermRentalAdsRepository;

        private readonly IRepository<UserRentalAdDTO> _userRentalAdsRepository;

        private readonly IAdsFilter<RentalAdDTO> _adsFilterRepository;

        private readonly IAdsFilter<DailyRentalAdDTO> _dailyAdsFilterRepository;

        private readonly IAdsFilter<LongTermRentalAdDTO> _longTermAdsFilterRepository;

        private readonly IAdsFilter<GeneralRentalAdDTO> _generalAdsFilterRepository;

        private readonly IUserRepository _usersRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdsManager"/> class.
        /// </summary>
        /// <param name="serviceResolver">Service resolver for loaders.</param>
        /// <param name="mapper">Mapper object.</param>
        /// <param name="aditionalAdDatasRepository">Aditional ad datas repository.</param>
        /// <param name="contactPersonsRepository">Contact persons repository.</param>
        /// <param name="housingPhotosRepository">Housing photos repository.</param>
        /// <param name="rentalAdsRepository">Rental ads repository.</param>
        /// <param name="dailyRentalAdsRepository">Daily rental ads repository.</param>
        /// <param name="longTermRentalAdsRepository">Long term rental ads repository.</param>
        /// <param name="userRentalAdsRepository">User rental ads repository.</param>
        /// <param name="adsFilterRepository">Ads filter repository.</param>
        /// <param name="dailyAdsFilterRepository">Daily ads filter repository.</param>
        /// <param name="longTermAdsFilterRepository">Long term ads filter repository.</param>
        /// <param name="generalAdsFilterRepository">General ads filter repository.</param>
        /// <param name="userRepository">User repository.</param>
        public AdsManager(Func<LoaderMenu, IDataLoader> serviceResolver, IMapper mapper, IRepository<AditionalAdDataDTO> aditionalAdDatasRepository, IRepository<ContactPersonDTO> contactPersonsRepository, IRepository<HousingPhotoDTO> housingPhotosRepository, IRepository<RentalAdDTO> rentalAdsRepository, IRepository<DailyRentalAdDTO> dailyRentalAdsRepository, IRepository<LongTermRentalAdDTO> longTermRentalAdsRepository, IRepository<UserRentalAdDTO> userRentalAdsRepository, IAdsFilter<RentalAdDTO> adsFilterRepository, IUserRepository userRepository, IAdsFilter<DailyRentalAdDTO> dailyAdsFilterRepository, IAdsFilter<LongTermRentalAdDTO> longTermAdsFilterRepository, IAdsFilter<GeneralRentalAdDTO> generalAdsFilterRepository)
        {
            _goHomeByDataLoader = serviceResolver(LoaderMenu.GoHomeBy);
            _realtByDataLoader = serviceResolver(LoaderMenu.RealtBY);
            _mapper = mapper;
            _aditionalAdDatasRepository = aditionalAdDatasRepository;
            _contactPersonsRepository = contactPersonsRepository;
            _housingPhotosRepository = housingPhotosRepository;
            _rentalAdsRepository = rentalAdsRepository;
            _dailyRentalAdsRepository = dailyRentalAdsRepository;
            _longTermRentalAdsRepository = longTermRentalAdsRepository;
            _userRentalAdsRepository = userRentalAdsRepository;
            _adsFilterRepository = adsFilterRepository;
            _usersRepository = userRepository;
            _dailyAdsFilterRepository = dailyAdsFilterRepository;
            _longTermAdsFilterRepository = longTermAdsFilterRepository;
            _generalAdsFilterRepository = generalAdsFilterRepository;
        }

        /// <summary>
        /// Load long term ads from GoHome.by site.
        /// </summary>
        /// <param name="loadDataFromSourceMenuItem">Menu item.</param>
        /// <returns>Task result.</returns>
        public async Task LoadLongTermAdsFromGoHomeBy(LoadDataFromSourceMenu loadDataFromSourceMenuItem)
        {
            const string GoHomeByHead = "https://gohome.by";

            switch (loadDataFromSourceMenuItem)
            {
                case LoadDataFromSourceMenu.GoHomeByDailyAds:
                    _goHomeByDataLoader.LoadDataInAds(RentalAdMenu.RealtApartmentsByDailyRentalAd);
                    _goHomeByDataLoader.LoadDataInAds(RentalAdMenu.RealtHousesByDailyRentalAd);

                    var newAds = _goHomeByDataLoader.Ads;

                    // Apartment ads
                    var oldApartmentRentalAds = await _dailyRentalAdsRepository
                        .Get(dailyRentalAd => dailyRentalAd.RentalType == (int)RentalAdMenu.RealtApartmentsByDailyRentalAd
                        && dailyRentalAd.SourceLink.Contains(GoHomeByHead));

                    // House ads
                    var oldHouseRentalAds = await _dailyRentalAdsRepository
                        .Get(dailyRentalAd => dailyRentalAd.RentalType == (int)RentalAdMenu.RealtHousesByDailyRentalAd
                        && dailyRentalAd.SourceLink.Contains(GoHomeByHead));

                    // Apartment + house ads
                    var oldRentalAds = oldApartmentRentalAds.Concat(oldHouseRentalAds);

                    var contactPersonsId = oldRentalAds.Select(oldRentalAd => oldRentalAd.ContactPersonId).Distinct();

                    // Apartment + house contact persons
                    var oldContactPersons = new List<ContactPersonDTO>();
                    var allOldContactPersons = await _contactPersonsRepository.Get();

                    foreach (var id in contactPersonsId)
                    {
                        var contactPerson = allOldContactPersons.FirstOrDefault(contactPerson => contactPerson.Id.Equals(id));

                        if (!(contactPerson is null))
                        {
                            oldContactPersons.Add(contactPerson);
                        }
                    }

                    // Remove contact persons with any tables items
                    await _contactPersonsRepository.RemoveRange(oldContactPersons);

                    var oldAditionalDatas = new List<AditionalAdDataDTO>();

                    foreach (var oldRentalAd in oldRentalAds)
                    {
                        var aditionalAdData = newAds.FirstOrDefault(newAd => newAd.AditionalAdData.RentalAdNumber == oldRentalAd.RentalAdNumber);

                        if (aditionalAdData is null)
                        {
                            var oldAditionalAdData = await _aditionalAdDatasRepository.Get(aditionalAdData => aditionalAdData.RentalAdNumber == oldRentalAd.RentalAdNumber);
                            oldAditionalDatas.Add(oldAditionalAdData.First());
                        }
                    }

                    // Remove filter aditional ad datas
                    await _aditionalAdDatasRepository.RemoveRange(oldAditionalDatas);

                    // All aditional ad datas
                    var oldAditionalAdDatas = await _aditionalAdDatasRepository.Get();

                    var addingAditionalAdDatas = new List<AditionalAdDataDTO>();
                    var updatingAditionalAdDatas = new List<AditionalAdDataDTO>();
                    var addingHousingPhotos = new List<HousingPhotoDTO>();

                    foreach (var ad in newAds)
                    {
                        var aditionalAdData = oldAditionalAdDatas.FirstOrDefault(adData => adData.RentalAdNumber == ad.AditionalAdData.RentalAdNumber);

                        if (aditionalAdData is null)
                        {
                            addingAditionalAdDatas.Add(ad.AditionalAdData);
                        }
                        else
                        {
                            aditionalAdData.UpdateDate = ad.AditionalAdData.UpdateDate;
                            updatingAditionalAdDatas.Add(aditionalAdData);
                        }

                        addingHousingPhotos.AddRange(ad.HousingPhotos);
                    }

                    // Add contact persons
                    await _contactPersonsRepository.CreateRange(newAds.Select(ad => ad.ContactPerson));

                    // Add daily rental ads
                    await _dailyRentalAdsRepository.CreateRange(newAds.Select(ad => ad.RentalAd as DailyRentalAdDTO));

                    // Add housing photos
                    await _housingPhotosRepository.CreateRange(addingHousingPhotos);

                    // Add aditional ad datas
                    await _aditionalAdDatasRepository.CreateRange(addingAditionalAdDatas);

                    // Update exsisting ad datas
                    await _aditionalAdDatasRepository.UpdateRange(updatingAditionalAdDatas);

                    break;

                case LoadDataFromSourceMenu.GoHomeByLongTermAds:
                    _goHomeByDataLoader.LoadDataInAds(RentalAdMenu.RealtApartmentsByLongTermRentalAd);
                    _goHomeByDataLoader.LoadDataInAds(RentalAdMenu.RealtHousesByLongTermRentalAd);

                    newAds = _goHomeByDataLoader.Ads;

                    // Apartment ads
                    var oldLongTermApartmentRentalAds = await _longTermRentalAdsRepository
                        .Get(longTermRentalAd => longTermRentalAd.RentalType == (int)RentalAdMenu.RealtApartmentsByLongTermRentalAd
                        && longTermRentalAd.SourceLink.Contains(GoHomeByHead));

                    // House ads
                    var oldLongTermHouseRentalAds = await _longTermRentalAdsRepository
                        .Get(longTermRentalAd => longTermRentalAd.RentalType == (int)RentalAdMenu.RealtHousesByLongTermRentalAd
                        && longTermRentalAd.SourceLink.Contains(GoHomeByHead));

                    // Apartment + house ads
                    var oldLongTermRentalAds = oldLongTermApartmentRentalAds.Concat(oldLongTermHouseRentalAds);

                    contactPersonsId = oldLongTermRentalAds.Select(oldRentalAd => oldRentalAd.ContactPersonId).Distinct();

                    // Apartment + house contact persons
                    oldContactPersons = new List<ContactPersonDTO>();
                    allOldContactPersons = await _contactPersonsRepository.Get();

                    foreach (var id in contactPersonsId)
                    {
                        var contactPerson = allOldContactPersons.FirstOrDefault(contactPerson => contactPerson.Id.Equals(id));

                        if (!(contactPerson is null))
                        {
                            oldContactPersons.Add(contactPerson);
                        }
                    }

                    // Remove contact persons with any tables items
                    await _contactPersonsRepository.RemoveRange(oldContactPersons);

                    oldAditionalDatas = new List<AditionalAdDataDTO>();

                    foreach (var oldRentalAd in oldLongTermRentalAds)
                    {
                        var aditionalAdData = newAds.FirstOrDefault(newAd => newAd.AditionalAdData.RentalAdNumber == oldRentalAd.RentalAdNumber);

                        if (aditionalAdData is null)
                        {
                            var oldAditionalAdData = await _aditionalAdDatasRepository.Get(aditionalAdData => aditionalAdData.RentalAdNumber == oldRentalAd.RentalAdNumber);
                            oldAditionalDatas.Add(oldAditionalAdData.First());
                        }
                    }

                    // Remove filter aditional ad datas
                    await _aditionalAdDatasRepository.RemoveRange(oldAditionalDatas);

                    // All aditional ad datas
                    oldAditionalAdDatas = await _aditionalAdDatasRepository.Get();

                    addingAditionalAdDatas = new List<AditionalAdDataDTO>();
                    updatingAditionalAdDatas = new List<AditionalAdDataDTO>();
                    addingHousingPhotos = new List<HousingPhotoDTO>();

                    foreach (var ad in newAds)
                    {
                        var aditionalAdData = oldAditionalAdDatas.FirstOrDefault(adData => adData.RentalAdNumber == ad.AditionalAdData.RentalAdNumber);

                        if (aditionalAdData is null)
                        {
                            addingAditionalAdDatas.Add(ad.AditionalAdData);
                        }
                        else
                        {
                            aditionalAdData.UpdateDate = ad.AditionalAdData.UpdateDate;
                            updatingAditionalAdDatas.Add(aditionalAdData);
                        }

                        addingHousingPhotos.AddRange(ad.HousingPhotos);
                    }

                    // Add contact persons
                    await _contactPersonsRepository.CreateRange(newAds.Select(ad => ad.ContactPerson));

                    // Add daily rental ads
                    await _longTermRentalAdsRepository.CreateRange(newAds.Select(ad => ad.RentalAd as LongTermRentalAdDTO));

                    // Add housing photos
                    await _housingPhotosRepository.CreateRange(addingHousingPhotos);

                    // Add aditional ad datas
                    await _aditionalAdDatasRepository.CreateRange(addingAditionalAdDatas);

                    // Update exsisting ad datas
                    await _aditionalAdDatasRepository.UpdateRange(updatingAditionalAdDatas);

                    break;

                default:
                    return;
            }
        }

        /// <summary>
        /// Gets ads for page.
        /// </summary>
        /// <param name="pageNumber">Current page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Ads list.</returns>
        public async Task<IEnumerable<RentalAd>> GetAdsForPage(int pageNumber, int pageSize)
        {
            var adsDTO = await _adsFilterRepository.GetAdsForPage(item => item.IsPublished, pageNumber, pageSize);
            var ads = new List<RentalAd>();

            foreach (var ad in adsDTO)
            {
                if (ad is DailyRentalAdDTO)
                {
                    ads.Add(_mapper.Map<DailyRentalAd>(ad as DailyRentalAdDTO));
                }
                else
                {
                    ads.Add(_mapper.Map<LongTermRentalAd>(ad as LongTermRentalAdDTO));
                }
            }

            return ads;
        }

        /// <summary>
        /// Get ads with predicate for page.
        /// </summary>
        /// <param name="adsTypeMenuItem">Ads type menu item.</param>
        /// <param name="pageNumber">Current page number.</param>
        /// <param name="pageSize">Ads count in page.</param>
        /// <returns>Ads list.</returns>
        public async Task<IEnumerable<RentalAd>> GetAdsForPage(AdsTypeMenu adsTypeMenuItem, int pageNumber, int pageSize)
        {
            var adsDTO = adsTypeMenuItem switch
            {
                AdsTypeMenu.LongTermAds => await _adsFilterRepository.GetAdsForPage(ad => ad is LongTermRentalAdDTO && ad.IsPublished, pageNumber, pageSize),
                AdsTypeMenu.DayilyAds => await _adsFilterRepository.GetAdsForPage(ad => ad is DailyRentalAdDTO && ad.IsPublished, pageNumber, pageSize),
                _ => new List<RentalAdDTO>(),
            };

            var ads = new List<RentalAd>();

            foreach (var ad in adsDTO)
            {
                if (ad is DailyRentalAdDTO)
                {
                    ads.Add(_mapper.Map<DailyRentalAd>(ad as DailyRentalAdDTO));
                }
                else
                {
                    ads.Add(_mapper.Map<LongTermRentalAd>(ad as LongTermRentalAdDTO));
                }
            }

            return ads;
        }

        /// <summary>
        /// Get rental ads count.
        /// </summary>
        /// <returns>Ads count.</returns>
        public int GetRentalAdsCount()
        {
            return _adsFilterRepository.GetRentalAdsCount(item => item.IsPublished);
        }

        /// <summary>
        /// Get rental ads count with predicate.
        /// </summary>
        /// <param name="adsTypeMenuItem">Ads type menu item.</param>
        /// <returns>Ads count.</returns>
        public int GetRentalAdsCount(AdsTypeMenu adsTypeMenuItem)
        {
            return adsTypeMenuItem switch
            {
                AdsTypeMenu.LongTermAds => _adsFilterRepository.GetRentalAdsCount(ad => ad is LongTermRentalAdDTO && ad.IsPublished),
                AdsTypeMenu.DayilyAds => _adsFilterRepository.GetRentalAdsCount(ad => ad is DailyRentalAdDTO && ad.IsPublished),
                _ => default,
            };
        }

        /// <summary>
        /// Get housing photos by rental ad id.
        /// </summary>
        /// <param name="id">Rental ad id.</param>
        /// <returns>Housing photo list.</returns>
        public async Task<IEnumerable<HousingPhoto>> GetHousingPhotosByRentalAdId(string id)
        {
            return _mapper.Map<IEnumerable<HousingPhoto>>(await _housingPhotosRepository.Get(housingPhoto => housingPhoto.RentalAdId.Equals(id)));
        }

        /// <summary>
        /// Method add to favourites if isFavorite==false or remove if isFavorite==true.
        /// </summary>
        /// <param name="userId">User unique key.</param>
        /// <param name="rentalAdId">Ads unique key.</param>
        /// <returns>Action result.</returns>
        public async Task AddOrRemoveFavorite(string userId, string rentalAdId)
        {
            if (userId is null || rentalAdId is null)
            {
                throw new ArgumentNullException("User id or rental ad id is null");
            }
            else
            {
                var rentalAds = await _rentalAdsRepository.Get(ad => ad.Id.Equals(rentalAdId));

                if (rentalAds.Count() != default)
                {
                    var userRentalAds = await _userRentalAdsRepository.Get(userRentalAd => userRentalAd.RentalAdId.Equals(rentalAdId) && userRentalAd.UserId.Equals(userId));

                    if (userRentalAds.Count() == default)
                    {
                        await _userRentalAdsRepository.Create(new UserRentalAdDTO
                        {
                            RentalAdId = rentalAdId,
                            UserId = userId,
                        });
                    }
                    else
                    {
                        await _userRentalAdsRepository.Remove(userRentalAds.First());
                    }
                }
            }
        }

        /// <summary>
        /// Get user favorite ads.
        /// </summary>
        /// <param name="userId">User unique key.</param>
        /// <returns>Favorite ads list.</returns>
        public async Task<IEnumerable<UserRentalAd>> GetUserFavoriteAds(string userId)
        {
            if (userId is null)
            {
                throw new ArgumentNullException("User id is null");
            }
            else
            {
                return _mapper.Map<IEnumerable<UserRentalAd>>(await _userRentalAdsRepository.Get(userRentalAd => userRentalAd.UserId.Equals(userId)));
            }
        }

        /// <summary>
        /// Get user favorite ads.
        /// </summary>
        /// <param name="userId">User unique key.</param>
        /// <returns>Favorite ads list.</returns>
        public async Task<IEnumerable<RentalAd>> GetFavoriteAds(string userId)
        {
            if (userId is null)
            {
                throw new ArgumentNullException("User id is null");
            }
            else
            {
                var userAds = await _userRentalAdsRepository.Get(userRentalAd => userRentalAd.UserId.Equals(userId));
                var rentalAds = new List<RentalAd>();

                foreach (var userAd in userAds)
                {
                    var ads = await _rentalAdsRepository.Get(ad => ad.Id.Equals(userAd.RentalAdId));

                    if (ads.First() is DailyRentalAdDTO)
                    {
                        rentalAds.Add(_mapper.Map<DailyRentalAd>(ads.First() as DailyRentalAdDTO));
                    }
                    else
                    {
                        rentalAds.Add(_mapper.Map<LongTermRentalAd>(ads.First() as LongTermRentalAdDTO));
                    }
                }

                return rentalAds;
            }
        }

        /// <summary>
        /// Get user ads.
        /// </summary>
        /// <param name="userId">User unique key.</param>
        /// <returns>Favorite ads list.</returns>
        public async Task<IEnumerable<RentalAd>> GetUserAds(string userId)
        {
            if (userId is null)
            {
                throw new ArgumentNullException("User id is null");
            }
            else
            {
                var ads = await _rentalAdsRepository.Get(item => item.UserId.Equals(userId));
                var rentalAds = new List<RentalAd>();

                foreach (var ad in ads)
                {

                    if (ad is DailyRentalAdDTO)
                    {
                        rentalAds.Add(_mapper.Map<DailyRentalAd>(ad as DailyRentalAdDTO));
                    }
                    else
                    {
                        rentalAds.Add(_mapper.Map<LongTermRentalAd>(ad as LongTermRentalAdDTO));
                    }
                }

                return rentalAds;
            }
        }

        /// <summary>
        /// Get ads for publish.
        /// </summary>
        /// <returns>Ads for publish.</returns>
        public async Task<IEnumerable<RentalAd>> GetAdsForPublish()
        {
            var ads = await _rentalAdsRepository.Get(item => !item.IsPublished);
            var rentalAds = new List<RentalAd>();

            foreach (var ad in ads)
            {
                if (ad is DailyRentalAdDTO)
                {
                    rentalAds.Add(_mapper.Map<DailyRentalAd>(ad as DailyRentalAdDTO));
                }
                else
                {
                    rentalAds.Add(_mapper.Map<LongTermRentalAd>(ad as LongTermRentalAdDTO));
                }
            }

            return rentalAds;
        }

        /// <summary>
        /// Search long term ads use parametrs.
        /// </summary>
        /// <param name="longTermSearch">Parametrs for searching.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Result ads.</returns>
        public async Task<IEnumerable<RentalAd>> LongTermSearch(LongTermSearch longTermSearch, int pageNumber, int pageSize)
        {
            string query = CreateLongTermSearchSqlQueryString(longTermSearch);
            return _mapper.Map<IEnumerable<LongTermRentalAd>>(await _longTermAdsFilterRepository.GetAdsForPage(query, pageNumber, pageSize));
        }

        /// <summary>
        /// Search long term ads use parametrs.
        /// </summary>
        /// <param name="generalSearch">Parametrs for searching.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Result ads.</returns>
        public async Task<IEnumerable<RentalAd>> GeneralSearch(GeneralSearch generalSearch, int pageNumber, int pageSize)
        {
            string query = CreateGeneralSearchSqlQueryString(generalSearch);

            var ads = await _generalAdsFilterRepository.GetAdsForPage(query, pageNumber, pageSize);

            var result = new List<RentalAd>();

            foreach (var item in ads)
            {
                if (item.Discriminator.Equals("LongTermRentalAdDTO"))
                {
                    result.Add(_mapper.Map<LongTermRentalAd>(item));
                }
                else
                {
                    result.Add(_mapper.Map<DailyRentalAd>(item));
                }
            }

            return result;
        }

        /// <summary>
        /// Get general search count.
        /// </summary>
        /// <param name="generalSearch">Daily search parametrs.</param>
        /// <returns>Count.</returns>
        public int GetGeneralSearchCount(GeneralSearch generalSearch)
        {
            string query = CreateGeneralSearchSqlQueryString(generalSearch);
            return _generalAdsFilterRepository.GetRentalAdsCount(query);
        }

        /// <summary>
        /// Get cities with views.
        /// </summary>
        /// <returns>City views models.</returns>
        public async Task<IEnumerable<CityViews>> GetCitiesStatistic()
        {
            var longTermAds = await _longTermRentalAdsRepository.Get();
            var dailyAds = await _dailyRentalAdsRepository.Get();
            var aditionalAdsDatas = await _aditionalAdDatasRepository.Get();

            var cities = longTermAds.Select(item => item.Locality).Distinct().ToList();
            cities.AddRange(dailyAds.Select(item => item.Locality).Distinct());
            cities = cities.Distinct().ToList();

            var datas = new List<CityViews>();

            foreach (var city in cities)
            {
                var cityLongTermAds = longTermAds.Where(item => item.Locality.Equals(city));
                var cityDailyAds = dailyAds.Where(item => item.Locality.Equals(city));
                int aditionalViews = 0;

                foreach (var ad in cityLongTermAds)
                {
                    var aditionalData = aditionalAdsDatas.FirstOrDefault(item => item.Id.Equals(ad.Id));
                    aditionalViews += aditionalData is null ? 0 : aditionalData.TotalViews;
                }

                foreach (var ad in cityDailyAds)
                {
                    var aditionalData = aditionalAdsDatas.FirstOrDefault(item => item.Id.Equals(ad.Id));
                    aditionalViews += aditionalData is null ? 0 : aditionalData.TotalViews;
                }

                datas.Add(new CityViews
                {
                    City = city,
                    LongTermAdsViews = cityLongTermAds.Sum(item => item.TotalViews) + aditionalViews,
                    DailyAdsViews = cityDailyAds.Sum(item => item.TotalViews) + aditionalViews,
                });
            }

            return datas;
        }

        /// <summary>
        /// Get cities with views.
        /// </summary>
        /// <returns>City views models.</returns>
        public async Task<IEnumerable<RentCountOfRoomStatistic>> GetCountOfRoomsAdsStatistic()
        {
            var longTermAds = await _longTermRentalAdsRepository.Get();
            var dailyAds = await _dailyRentalAdsRepository.Get();

            var rentCountOfRooms = longTermAds.Select(item => item.RentCountOfRooms).Distinct().ToList();
            rentCountOfRooms.AddRange(dailyAds.Select(item => item.RentCountOfRooms).Distinct());
            rentCountOfRooms = rentCountOfRooms.Distinct().ToList();

            var countOfRoomAdsStatistic = new List<RentCountOfRoomStatistic>();

            foreach (int item in rentCountOfRooms)
            {
                countOfRoomAdsStatistic.Add(new RentCountOfRoomStatistic
                {
                    RentCountOfRoom = item,
                    LongTermAdsAveragePrice = longTermAds.Where(ad => ad.RentCountOfRooms == item).Count(),
                    DailyAdsAveragePrice = dailyAds.Where(ad => ad.RentCountOfRooms == item).Count(),
                });
            }

            return countOfRoomAdsStatistic.Where(item => item.RentCountOfRoom > 0).OrderBy(item => item.RentCountOfRoom);
        }

        /// <summary>
        /// Search daily ads use parametrs.
        /// </summary>
        /// <param name="dailySearch">Parametrs for searching.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Result ads.</returns>
        public async Task<IEnumerable<RentalAd>> DailySearch(DailySearch dailySearch, int pageNumber, int pageSize)
        {
            string query = CreateDailySearchSqlQueryString(dailySearch);
            return _mapper.Map<IEnumerable<DailyRentalAd>>(await _dailyAdsFilterRepository.GetAdsForPage(query, pageNumber, pageSize));
        }

        /// <summary>
        /// Get daily search count.
        /// </summary>
        /// <param name="dailySearch">Daily search parametrs.</param>
        /// <returns>Count.</returns>
        public int GetDailySearchCount(DailySearch dailySearch)
        {
            string query = CreateDailySearchSqlQueryString(dailySearch);
            return _dailyAdsFilterRepository.GetRentalAdsCount(query);
        }

        /// <summary>
        /// Get long term search count.
        /// </summary>
        /// <param name="longTermSearch">Long term search parametrs.</param>
        /// <returns>Count.</returns>
        public int GetLongTermSearchCount(LongTermSearch longTermSearch)
        {
            string query = CreateLongTermSearchSqlQueryString(longTermSearch);
            return _longTermAdsFilterRepository.GetRentalAdsCount(query);
        }

        /// <summary>
        /// Create search sql query string.
        /// </summary>
        /// <param name="adsType">Type of rental ads.</param>
        /// <param name="search">Search parametrs.</param>
        /// <param name="query">Start query.</param>
        /// <returns>Sql query string.</returns>
        public string CreateBasicSearchSqlQueryString(AdsTypeMenu adsType, Search search, string query)
        {
            if (adsType == AdsTypeMenu.DayilyAds)
            {
                query = string.Concat(query, $"where Discriminator = 'DailyRentalAdDTO' ");
            }
            else
            {
                query = string.Concat(query, $"where Discriminator = 'LongTermRentalAdDTO' ");
            }

            if (!string.IsNullOrEmpty(search.Address))
            {
                query = string.Concat(query, $"and charindex(N'{search.Address}', Address) > 0 ");
            }

            if (!string.IsNullOrEmpty(search.District))
            {
                query = string.Concat(query, $"and charindex(N'{search.District}', District) > 0 ");
            }

            if (!(search.FinishCurrentFloor is null))
            {
                query = string.Concat(query, $"and Floor <= {search.FinishCurrentFloor} ");
            }

            if (!(search.FinishRoomsCount is null))
            {
                query = string.Concat(query, $"and TotalCountOfRooms <= {search.FinishRoomsCount} ");
            }

            if (!(search.FinishTotalFloor is null))
            {
                query = string.Concat(query, $"and TotalFloors <= {search.FinishTotalFloor} ");
            }

            if (search.IsFirstFloor)
            {
                query = string.Concat(query, $"and Floor > 1 ");
            }

            if (search.IsLastFloor)
            {
                query = string.Concat(query, $"and Floor < TotalFloors ");
            }

            if (!string.IsNullOrEmpty(search.Locality))
            {
                query = string.Concat(query, $"and charindex(N'{search.Locality}', Locality) > 0 ");
            }

            if (!string.IsNullOrEmpty(search.Region) && !search.Region.Equals("Любая область"))
            {
                query = string.Concat(query, $"and charindex(N'{search.Region}', Region) > 0 ");
            }

            if (!(search.StartCurrentFloor is null))
            {
                query = string.Concat(query, $"and Floor >= {search.StartCurrentFloor} ");
            }

            if (!(search.StartRoomsCount is null))
            {
                query = string.Concat(query, $"and TotalCountOfRooms >= {search.StartRoomsCount} ");
            }

            if (!(search.StartTotalFloor is null))
            {
                query = string.Concat(query, $"and TotalFloors >= {search.StartTotalFloor} ");
            }

            return query;
        }

        /// <summary>
        /// Create daily search sql query string.
        /// </summary>
        /// <param name="dailySearch">Daily search parametrs.</param>
        /// <returns>Sql query string.</returns>
        public string CreateDailySearchSqlQueryString(DailySearch dailySearch)
        {
            const string Query = "Select Id, ContactPersonId, IsPublished, UserId, SourceLink, RentalAdNumber, UpdateDate, Region, District, Locality, Address, TotalCountOfRooms, RentCountOfRooms, TotalArea, LivingArea, KitchenArea, TotalFloors, Floor, XMapCoordinate, YMapCoordinate, Bathroom, Notes, Description, Facilities, RentalType, TotalViews, MonthViews, WeekViews, LandArea, Discriminator, BYNPricePerPerson, USDPricePerPerson, USDPricePerDay, BYNPricePerDay from dbo.RentalAds ";

            string query = CreateBasicSearchSqlQueryString(AdsTypeMenu.DayilyAds, dailySearch, Query);

            if (!(dailySearch.StartBYNPricePerPerson is null))
            {
                query = string.Concat(query, $"and BYNPricePerPerson >= {dailySearch.StartBYNPricePerPerson} ");
            }

            if (!(dailySearch.FinishBYNPricePerPerson is null))
            {
                query = string.Concat(query, $"and BYNPricePerPerson <= {dailySearch.FinishBYNPricePerPerson} ");
            }

            if (!(dailySearch.StartUSDPricePerPerson is null))
            {
                query = string.Concat(query, $"and USDPricePerPerson >= {dailySearch.StartUSDPricePerPerson} ");
            }

            if (!(dailySearch.FinishUSDPricePerPerson is null))
            {
                query = string.Concat(query, $"and USDPricePerPerson <= {dailySearch.FinishUSDPricePerPerson} ");
            }

            if (!(dailySearch.StartUSDPricePerDay is null))
            {
                query = string.Concat(query, $"and USDPricePerDay >= {dailySearch.StartUSDPricePerDay} ");
            }

            if (!(dailySearch.FinishUSDPricePerDay is null))
            {
                query = string.Concat(query, $"and USDPricePerDay <= {dailySearch.FinishUSDPricePerDay} ");
            }

            if (!(dailySearch.StartBYNPricePerDay is null))
            {
                query = string.Concat(query, $"and BYNPricePerDay >= {dailySearch.StartBYNPricePerDay} ");
            }

            if (!(dailySearch.FinishBYNPricePerDay is null))
            {
                query = string.Concat(query, $"and BYNPricePerDay <= {dailySearch.FinishBYNPricePerDay} ");
            }

            query = string.Concat(query, $"and IsPublished = (1) ");

            return query;
        }

        /// <summary>
        /// Create daily search sql query string.
        /// </summary>
        /// <param name="longTermSearch">Long term search parametrs.</param>
        /// <returns>Sql query string.</returns>
        public string CreateLongTermSearchSqlQueryString(LongTermSearch longTermSearch)
        {
            const string Query = "Select Id, ContactPersonId, IsPublished, UserId, SourceLink, RentalAdNumber, UpdateDate, Region, District, Locality, Address, TotalCountOfRooms, RentCountOfRooms, TotalArea, LivingArea, KitchenArea, TotalFloors, Floor, XMapCoordinate, YMapCoordinate, Bathroom, Notes, Description, Facilities, RentalType, TotalViews, MonthViews, WeekViews, LandArea, Discriminator, BYNPrice, USDPrice from dbo.RentalAds ";

            string query = CreateBasicSearchSqlQueryString(AdsTypeMenu.LongTermAds, longTermSearch, Query);

            if (!(longTermSearch.StartTotalArea is null))
            {
                query = string.Concat(query, $"and TotalArea >= {longTermSearch.StartTotalArea} ");
            }

            if (!(longTermSearch.FinishTotalArea is null))
            {
                query = string.Concat(query, $"and TotalArea <= {longTermSearch.FinishTotalArea} ");
            }

            if (!(longTermSearch.StartLivingArea is null))
            {
                query = string.Concat(query, $"and LivingArea >= {longTermSearch.StartLivingArea} ");
            }

            if (!(longTermSearch.FinishLivingArea is null))
            {
                query = string.Concat(query, $"and LivingArea <= {longTermSearch.FinishLivingArea} ");
            }

            if (!(longTermSearch.StartKitchenArea is null))
            {
                query = string.Concat(query, $"and KitchenArea >= {longTermSearch.StartKitchenArea} ");
            }

            if (!(longTermSearch.FinishKitchenArea is null))
            {
                query = string.Concat(query, $"and KitchenArea <= {longTermSearch.FinishKitchenArea} ");
            }

            if (!(longTermSearch.StartLandArea is null))
            {
                query = string.Concat(query, $"and LandArea >= {longTermSearch.StartLandArea} ");
            }

            if (!(longTermSearch.FinishLandArea is null))
            {
                query = string.Concat(query, $"and LandArea <= {longTermSearch.FinishLandArea} ");
            }

            if (!(longTermSearch.StartBYNPrice is null))
            {
                query = string.Concat(query, $"and BYNPrice >= {longTermSearch.StartBYNPrice} ");
            }

            if (!(longTermSearch.FinishBYNPrice is null))
            {
                query = string.Concat(query, $"and BYNPrice <= {longTermSearch.FinishBYNPrice} ");
            }

            if (!(longTermSearch.StartUSDPrice is null))
            {
                query = string.Concat(query, $"and USDPrice >= {longTermSearch.StartUSDPrice} ");
            }

            if (!(longTermSearch.FinishUSDPrice is null))
            {
                query = string.Concat(query, $"and USDPrice <= {longTermSearch.FinishUSDPrice} ");
            }

            query = string.Concat(query, $"and IsPublished = (1) ");

            return query;
        }

        /// <summary>
        /// Create general search sql query string.
        /// </summary>
        /// <param name="generalSearch">Long term search parametrs.</param>
        /// <returns>Sql query string.</returns>
        public string CreateGeneralSearchSqlQueryString(GeneralSearch generalSearch)
        {
            string query = "Select Id, ContactPersonId, IsPublished, UserId, SourceLink, RentalAdNumber, UpdateDate, Region, District, Locality, Address, TotalCountOfRooms, RentCountOfRooms, TotalArea, LivingArea, KitchenArea, TotalFloors, Floor, XMapCoordinate, YMapCoordinate, Bathroom, Notes, Description, Facilities, RentalType, TotalViews, MonthViews, WeekViews, LandArea, Discriminator, BYNPricePerPerson, USDPricePerPerson, USDPricePerDay, BYNPricePerDay, BYNPrice, USDPrice from dbo.RentalAds ";
            query = string.Concat(query, $"where IsPublished = (1) ");

            if (!string.IsNullOrEmpty(generalSearch.Locality))
            {
                query = string.Concat(query, $"and charindex(N'{generalSearch.Locality}', Locality) > 0 ");
            }

            if (!(generalSearch.RoomsCount is null))
            {
                query = string.Concat(query, $"and TotalCountOfRooms = {generalSearch.RoomsCount} ");
            }

            if (generalSearch.LongTerm && !generalSearch.Daily)
            {
                query = string.Concat(query, $"and Discriminator = 'LongTermRentalAdDTO' ");
            }

            if (!generalSearch.LongTerm && generalSearch.Daily)
            {
                query = string.Concat(query, $"and Discriminator = 'DailyRentalAdDTO' ");
            }

            if (!(generalSearch.StartArea is null))
            {
                query = string.Concat(query, $"and TotalArea >= {generalSearch.StartArea} ");
            }

            if (!(generalSearch.FinishArea is null))
            {
                query = string.Concat(query, $"and TotalArea <= {generalSearch.FinishArea} ");
            }

            if (!(generalSearch.StartUSDPrice is null) || !(generalSearch.FinishUSDPrice is null))
            {
                query = string.Concat(query, "and (");

                if (!(generalSearch.StartUSDPrice is null))
                {
                    query = string.Concat(query, $"USDPricePerDay >= {generalSearch.StartUSDPrice} ");
                    query = string.Concat(query, $"or USDPrice >= {generalSearch.StartUSDPrice} ");
                }

                if (!(generalSearch.FinishUSDPrice is null))
                {
                    if (!(generalSearch.StartUSDPrice is null))
                    {
                        query = string.Concat(query, "or ");
                    }

                    query = string.Concat(query, $"USDPricePerDay <= {generalSearch.FinishUSDPrice} ");
                    query = string.Concat(query, $"or USDPrice <= {generalSearch.FinishUSDPrice} ");
                }

                query = string.Concat(query, ")");
            }

            return query;
        }

        /// <summary>
        /// Get rental ad by id.
        /// </summary>
        /// <param name="id">Unique key.</param>
        /// <returns>Rental ad.</returns>
        public async Task<AllRentalAdInfo> GetAllRentalAdInfo(string id)
        {
            var allRentalAdInfo = new AllRentalAdInfo();

            IEnumerable<RentalAdDTO> rentalAds = await _dailyRentalAdsRepository.Get(ad => ad.Id.Equals(id));

            if (rentalAds.Count() == default)
            {
                rentalAds = await _longTermRentalAdsRepository.Get(ad => ad.Id.Equals(id));
            }

            if (rentalAds.Count() != default)
            {
                if (rentalAds.First() is DailyRentalAdDTO)
                {
                    allRentalAdInfo.RentalAd = _mapper.Map<DailyRentalAd>(rentalAds.First());
                }
                else
                {
                    allRentalAdInfo.RentalAd = _mapper.Map<LongTermRentalAd>(rentalAds.First());
                }

                var contactPersons = await _contactPersonsRepository.Get(person => person.Id.Equals(allRentalAdInfo.RentalAd.ContactPersonId));

                if (contactPersons.Count() != default)
                {
                    allRentalAdInfo.ContactPerson = _mapper.Map<ContactPerson>(contactPersons.First());
                }
                else
                {
                    allRentalAdInfo.IsOriginal = true;

                    var user = await _usersRepository.FindById(allRentalAdInfo.RentalAd.UserId);

                    if (user is null)
                    {
                        allRentalAdInfo.ContactPerson = new ContactPerson();
                    }
                    else
                    {
                        allRentalAdInfo.ContactPerson = new ContactPerson
                        {
                            Id = user.Id,
                            Name = user.FullName,
                            PhoneNumber = user.PhoneNumber,
                            Email = user.Email,
                            AdditionalPhoneNumber = "-",
                        };
                    }
                }

                allRentalAdInfo.Photos = await GetHousingPhotosByRentalAdId(id);
                var aditionalAdDatas = await _aditionalAdDatasRepository.Get(data => data.Id.Equals(allRentalAdInfo.RentalAd.Id));

                if (aditionalAdDatas.Count() == default)
                {
                    allRentalAdInfo.AditionalAdData = new AditionalAdData();
                }
                else
                {
                    var data = aditionalAdDatas.First();
                    data.TotalViews += 1;
                    data.WeekViews += 1;
                    data.MonthViews += 1;
                    await _aditionalAdDatasRepository.Update(data);

                    allRentalAdInfo.AditionalAdData = _mapper.Map<AditionalAdData>(aditionalAdDatas.First());
                }

                return allRentalAdInfo;
            }
            else
            {
                throw new NullReferenceException("Rental ad not found");
            }
        }

        /// <summary>
        /// Add new ad.
        /// </summary>
        /// <param name="createModel">Ad parametrs.</param>
        /// <param name="userId">User id.</param>
        /// <param name="hostUrl">Host url to creating source link.</param>
        /// <param name="photosUrls">Path to photos.</param>
        /// <returns>Task result.</returns>
        public async Task<string> CreateAd(CreateModel createModel, string userId, string hostUrl, IEnumerable<string> photosUrls)
        {
            var date = DateTime.Now;
            string rentalAdId = Guid.NewGuid().ToString();
            var rentalAd = CreateBasicAdModel(createModel, userId, date);

            if (createModel.RentalType == AdsTypeMenu.DayilyAds)
            {
                var dailyRentalAd = _mapper.Map<DailyRentalAdDTO>(rentalAd);
                dailyRentalAd.BYNPricePerDay = createModel.BYNPricePerDay;
                dailyRentalAd.BYNPricePerPerson = createModel.BYNPricePerPerson;
                dailyRentalAd.USDPricePerDay = createModel.USDPricePerDay;
                dailyRentalAd.USDPricePerPerson = createModel.USDPricePerPerson;
                dailyRentalAd.Id = rentalAdId;
                dailyRentalAd.SourceLink = $"/Ads/RentalAd/{rentalAdId}";

                await _dailyRentalAdsRepository.Create(dailyRentalAd);
            }
            else
            {
                var longTermRentalAd = _mapper.Map<LongTermRentalAdDTO>(rentalAd);
                longTermRentalAd.BYNPrice = createModel.BYNPrice;
                longTermRentalAd.USDPrice = createModel.USDPrice;
                longTermRentalAd.Id = rentalAdId;
                longTermRentalAd.SourceLink = $"{hostUrl}/Ads/RentalAd/{rentalAdId}";

                await _longTermRentalAdsRepository.Create(longTermRentalAd);
            }

            await _aditionalAdDatasRepository.Create(CreateAditionalAdData(rentalAdId, date));

            await _housingPhotosRepository.CreateRange(photosUrls.Select(item => new HousingPhotoDTO
            {
                PathToPhoto = item,
                RentalAdId = rentalAdId,
            }));

            return rentalAdId;
        }

        /// <summary>
        /// Create basic rental of property mode.
        /// </summary>
        /// <param name="createModel">Create parametrs.</param>
        /// <param name="userId">User id.</param>
        /// <param name="updateDate">Update date.</param>
        /// <returns>Basic rental of property model.</returns>
        public RentalAdDTO CreateBasicAdModel(CreateModel createModel, string userId, DateTime updateDate)
        {
            return new RentalAdDTO()
            {
                Address = createModel.Address,
                Bathroom = createModel.Bathroom,
                UserId = userId,
                Description = createModel.Description,
                District = createModel.District,
                Facilities = createModel.Facilities,
                Floor = createModel.Floor,
                IsPublished = false,
                KitchenArea = createModel.KitchenArea,
                LandArea = createModel.LandArea,
                LivingArea = createModel.LivingArea,
                Locality = createModel.Locality,
                Notes = createModel.Notes,
                Region = createModel.Region,
                RentCountOfRooms = createModel.RentCountOfRooms,
                TotalArea = createModel.TotalArea,
                TotalCountOfRooms = createModel.TotalCountOfRooms,
                TotalFloors = createModel.TotalFloors,
                XMapCoordinate = createModel.XMapCoordinate,
                YMapCoordinate = createModel.YMapCoordinate,
                TotalViews = default,
                MonthViews = default,
                WeekViews = default,
                UpdateDate = updateDate,
                RentalType = (int)createModel.RentalType,
                RentalAdNumber = default,
            };
        }

        /// <summary>
        /// Create default aditional ad data.
        /// </summary>
        /// <param name="id">Rental ad id.</param>
        /// <param name="updateDate">Date of create aditional ad data.</param>
        /// <returns>Aditional ad data object.</returns>
        public AditionalAdDataDTO CreateAditionalAdData(string id, DateTime updateDate)
        {
            return new AditionalAdDataDTO
            {
                Id = id,
                RentalAdNumber = default,
                TotalViews = default,
                MonthViews = default,
                WeekViews = default,
                UpdateDate = updateDate,
            };
        }

        /// <summary>
        /// Remove ad.
        /// </summary>
        /// <param name="id">Ad unique key.</param>
        /// <param name="userId">User unique key.</param>
        /// <param name="isAdministrator">Is user administrator.</param>
        /// <returns>Remove result.</returns>
        public async Task Remove(string id, string userId, bool isAdministrator)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("Ad id is null");
            }
            else
            {
                var ads = await _rentalAdsRepository.Get(item => item.Id.Equals(id));

                if (ads.Count() == default)
                {
                    throw new NullReferenceException("Ad not found");
                }
                else
                {
                    if (isAdministrator || ads.First().UserId.Equals(userId))
                    {
                        var aditionalAdDatas = await _aditionalAdDatasRepository.Get(item => item.Id.Equals(ads.First().Id));

                        await _aditionalAdDatasRepository.Remove(aditionalAdDatas.First());
                        await _rentalAdsRepository.Remove(ads.First());
                    }
                    else
                    {
                        throw new Exception("You can't remove ad");
                    }
                }
            }
        }

        /// <summary>
        /// Edit ad.
        /// </summary>
        /// <param name="createModel">Ad parametrs.</param>
        /// <param name="userId">User id.</param>
        /// <param name="isAdministrator">Is administrator.</param>
        /// <param name="photosUrls">Path to photos.</param>
        /// <returns>Task result.</returns>
        public async Task Edit(CreateModel createModel, string userId, bool isAdministrator, IEnumerable<string> photosUrls)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("User id is null");
            }
            else
            {
                var ads = await _rentalAdsRepository.Get(item => item.Id.Equals(createModel.Id));

                if (ads.Count() == default)
                {
                    throw new NullReferenceException("Ad not found");
                }
                else
                {
                    var date = DateTime.Now;

                    if (isAdministrator || ads.First().UserId.Equals(userId))
                    {
                        var rentalAd = ads.First();

                        var aditionalAdDatas = await _aditionalAdDatasRepository.Get(item => item.Id.Equals(rentalAd.Id));
                        aditionalAdDatas.First().UpdateDate = date;
                        await _aditionalAdDatasRepository.Update(aditionalAdDatas.First());

                        rentalAd.Region = createModel.Region;
                        rentalAd.District = createModel.District;
                        rentalAd.Locality = createModel.Locality;
                        rentalAd.Address = createModel.Address;
                        rentalAd.TotalCountOfRooms = createModel.TotalCountOfRooms;
                        rentalAd.RentCountOfRooms = createModel.RentCountOfRooms;
                        rentalAd.TotalArea = createModel.TotalArea;
                        rentalAd.LivingArea = createModel.LivingArea;
                        rentalAd.KitchenArea = createModel.KitchenArea;
                        rentalAd.TotalFloors = createModel.TotalFloors;
                        rentalAd.Floor = createModel.Floor;
                        rentalAd.XMapCoordinate = createModel.XMapCoordinate;
                        rentalAd.YMapCoordinate = createModel.YMapCoordinate;
                        rentalAd.Bathroom = createModel.Bathroom;
                        rentalAd.Notes = createModel.Notes;
                        rentalAd.Description = createModel.Description;
                        rentalAd.Facilities = createModel.Facilities;
                        rentalAd.RentalType = (int)createModel.RentalType;
                        rentalAd.LandArea = createModel.LandArea;
                        rentalAd.UpdateDate = date;
                        rentalAd.IsPublished = false;

                        if (createModel.RentalType == AdsTypeMenu.DayilyAds)
                        {
                            var dailyRentalAd = _mapper.Map<DailyRentalAdDTO>(rentalAd);
                            dailyRentalAd.BYNPricePerDay = createModel.BYNPricePerDay;
                            dailyRentalAd.BYNPricePerPerson = createModel.BYNPricePerPerson;
                            dailyRentalAd.USDPricePerDay = createModel.USDPricePerDay;
                            dailyRentalAd.USDPricePerPerson = createModel.USDPricePerPerson;

                            await _dailyRentalAdsRepository.Update(dailyRentalAd);
                        }
                        else
                        {
                            var longTermRentalAd = _mapper.Map<LongTermRentalAdDTO>(rentalAd);
                            longTermRentalAd.BYNPrice = createModel.BYNPrice;
                            longTermRentalAd.USDPrice = createModel.USDPrice;

                            await _longTermRentalAdsRepository.Update(longTermRentalAd);
                        }

                        // Remove old photos
                        await _housingPhotosRepository.RemoveRange(await _housingPhotosRepository.Get(item => item.RentalAdId.Equals(rentalAd.Id)));

                        // Add new photos
                        await _housingPhotosRepository.CreateRange(photosUrls.Select(item => new HousingPhotoDTO
                        {
                            PathToPhoto = item,
                            RentalAdId = rentalAd.Id,
                        }));
                    }
                    else
                    {
                        throw new Exception("You can't edit this ad");
                    }
                }
            }
        }

        /// <summary>
        /// Publish or unpublish ad.
        /// </summary>
        /// <param name="id">Rental ad unique key.</param>
        /// <returns>Publish result.</returns>
        public async Task Publish(string id)
        {
            var ads = await _rentalAdsRepository.Get(item => item.Id.Equals(id));

            if (ads.Count() == default)
            {
                throw new NullReferenceException("Ad not found");
            }
            else if (ads.First().UserId is null)
            {
                throw new ArgumentNullException("Can't publish or unpublish ad from other sites.");
            }
            else
            {
                var ad = ads.First();
                ad.IsPublished = ad.IsPublished ? false : true;
                await _rentalAdsRepository.Update(ad);
            }
        }
    }
}
