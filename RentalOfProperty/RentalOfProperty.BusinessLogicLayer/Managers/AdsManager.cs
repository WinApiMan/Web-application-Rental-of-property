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
        /// <param name="userRepository">User repository.</param>
        public AdsManager(Func<LoaderMenu, IDataLoader> serviceResolver, IMapper mapper, IRepository<AditionalAdDataDTO> aditionalAdDatasRepository, IRepository<ContactPersonDTO> contactPersonsRepository, IRepository<HousingPhotoDTO> housingPhotosRepository, IRepository<RentalAdDTO> rentalAdsRepository, IRepository<DailyRentalAdDTO> dailyRentalAdsRepository, IRepository<LongTermRentalAdDTO> longTermRentalAdsRepository, IRepository<UserRentalAdDTO> userRentalAdsRepository, IAdsFilter<RentalAdDTO> adsFilterRepository, IUserRepository userRepository)
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
            var adsDTO = await _adsFilterRepository.GetAdsForPage(pageNumber, pageSize);
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
                AdsTypeMenu.LongTermAds => await _adsFilterRepository.GetAdsForPage(ad => ad is LongTermRentalAdDTO, pageNumber, pageSize),
                AdsTypeMenu.DayilyAds => await _adsFilterRepository.GetAdsForPage(ad => ad is DailyRentalAdDTO, pageNumber, pageSize),
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
            return _adsFilterRepository.GetRentalAdsCount();
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
                AdsTypeMenu.LongTermAds => _adsFilterRepository.GetRentalAdsCount(ad => ad is LongTermRentalAdDTO),
                AdsTypeMenu.DayilyAds => _adsFilterRepository.GetRentalAdsCount(ad => ad is DailyRentalAdDTO),
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
        /// Search long term ads use parametrs.
        /// </summary>
        /// <param name="longTermSearch">Parametrs for searching.</param>
        /// <returns>Result ads.</returns>
        public async Task<IEnumerable<RentalAd>> LongTermSearch(LongTermSearch longTermSearch)
        {
            if (longTermSearch.Address is null && longTermSearch.District is null && longTermSearch.FinishBYNPrice is null
                && longTermSearch.FinishCurrentFloor is null && longTermSearch.FinishKitchenArea is null && longTermSearch.FinishLandArea is null
                && longTermSearch.FinishLivingArea is null && longTermSearch.FinishRoomsCount is null && longTermSearch.FinishTotalArea is null
                && longTermSearch.FinishTotalFloor is null && longTermSearch.FinishUSDPrice is null && !longTermSearch.IsFirstFloor
                && !longTermSearch.IsLastFloor && longTermSearch.Locality is null && longTermSearch.Region is null
                && longTermSearch.StartBYNPrice is null && longTermSearch.StartCurrentFloor is null && longTermSearch.StartKitchenArea is null
                && longTermSearch.StartLandArea is null && longTermSearch.StartLivingArea is null && longTermSearch.StartRoomsCount is null
                && longTermSearch.StartTotalArea is null && longTermSearch.StartTotalFloor is null && longTermSearch.StartUSDPrice is null)
            {
                return new List<RentalAd>();
            }
            else
            {
                var ads = _mapper.Map<IEnumerable<LongTermRentalAd>>(await _longTermRentalAdsRepository.Get());

                if (!longTermSearch.Region.Equals("Любая область"))
                {
                    ads = ads.Where(ad => ad.Region.ToLower().IndexOf(longTermSearch.Region.ToLower()) != -1);
                }

                if (!(longTermSearch.District is null))
                {
                    ads = ads.Where(ad => ad.District.ToLower().IndexOf(longTermSearch.District.ToLower()) != -1);
                }

                if (!(longTermSearch.Locality is null))
                {
                    ads = ads.Where(ad => ad.Locality.ToLower().IndexOf(longTermSearch.Locality.ToLower()) != -1);
                }

                if (!(longTermSearch.Address is null))
                {
                    ads = ads.Where(ad => ad.Address.ToLower().IndexOf(longTermSearch.Address.ToLower()) != -1);
                }

                if (!(longTermSearch.StartRoomsCount is null))
                {
                    ads = ads.Where(ad => ad.TotalCountOfRooms >= longTermSearch.StartRoomsCount);
                }

                if (!(longTermSearch.FinishRoomsCount is null))
                {
                    ads = ads.Where(ad => ad.TotalCountOfRooms <= longTermSearch.FinishRoomsCount);
                }

                if (!(longTermSearch.StartCurrentFloor is null))
                {
                    ads = ads.Where(ad => ad.Floor >= longTermSearch.StartCurrentFloor);
                }

                if (!(longTermSearch.FinishCurrentFloor is null))
                {
                    ads = ads.Where(ad => ad.Floor <= longTermSearch.FinishCurrentFloor);
                }

                if (!(longTermSearch.StartTotalFloor is null))
                {
                    ads = ads.Where(ad => ad.TotalFloors >= longTermSearch.StartTotalFloor);
                }

                if (!(longTermSearch.FinishTotalFloor is null))
                {
                    ads = ads.Where(ad => ad.TotalFloors <= longTermSearch.FinishTotalFloor);
                }

                if (longTermSearch.IsFirstFloor)
                {
                    ads = ads.Where(ad => ad.Floor > 1);
                }

                if (longTermSearch.IsLastFloor)
                {
                    ads = ads.Where(ad => ad.Floor < ad.TotalFloors);
                }

                if (!(longTermSearch.StartBYNPrice is null))
                {
                    ads = ads.Where(ad => ad.BYNPrice >= longTermSearch.StartBYNPrice);
                }

                if (!(longTermSearch.FinishBYNPrice is null))
                {
                    ads = ads.Where(ad => ad.BYNPrice <= longTermSearch.FinishBYNPrice);
                }

                if (!(longTermSearch.StartUSDPrice is null))
                {
                    ads = ads.Where(ad => ad.USDPrice >= longTermSearch.StartUSDPrice);
                }

                if (!(longTermSearch.FinishUSDPrice is null))
                {
                    ads = ads.Where(ad => ad.USDPrice <= longTermSearch.FinishUSDPrice);
                }

                if (!(longTermSearch.StartTotalArea is null))
                {
                    ads = ads.Where(ad => ad.TotalArea >= longTermSearch.StartTotalArea);
                }

                if (!(longTermSearch.FinishTotalArea is null))
                {
                    ads = ads.Where(ad => ad.TotalArea <= longTermSearch.FinishTotalArea);
                }

                if (!(longTermSearch.StartLivingArea is null))
                {
                    ads = ads.Where(ad => ad.LivingArea >= longTermSearch.StartLivingArea);
                }

                if (!(longTermSearch.FinishLivingArea is null))
                {
                    ads = ads.Where(ad => ad.LivingArea <= longTermSearch.FinishLivingArea);
                }

                if (!(longTermSearch.StartKitchenArea is null))
                {
                    ads = ads.Where(ad => ad.KitchenArea >= longTermSearch.StartKitchenArea);
                }

                if (!(longTermSearch.FinishKitchenArea is null))
                {
                    ads = ads.Where(ad => ad.KitchenArea <= longTermSearch.FinishKitchenArea);
                }

                if (!(longTermSearch.StartLandArea is null))
                {
                    ads = ads.Where(ad => ad.LandArea >= longTermSearch.StartLandArea);
                }

                if (!(longTermSearch.FinishLandArea is null))
                {
                    ads = ads.Where(ad => ad.LandArea <= longTermSearch.FinishLandArea);
                }

                return ads.ToList();
            }
        }

        /// <summary>
        /// Search daily ads use parametrs.
        /// </summary>
        /// <param name="dailySearch">Parametrs for searching.</param>
        /// <returns>Result ads.</returns>
        public async Task<IEnumerable<RentalAd>> DailySearch(DailySearch dailySearch)
        {
            if (dailySearch.Address is null && dailySearch.District is null && dailySearch.StartBYNPricePerPerson is null
                && dailySearch.FinishCurrentFloor is null && dailySearch.FinishBYNPricePerPerson is null && dailySearch.StartUSDPricePerPerson is null
                && dailySearch.FinishUSDPricePerPerson is null && dailySearch.FinishRoomsCount is null && dailySearch.StartUSDPricePerDay is null
                && dailySearch.FinishTotalFloor is null && dailySearch.FinishUSDPricePerDay is null && !dailySearch.IsFirstFloor
                && !dailySearch.IsLastFloor && dailySearch.Locality is null && dailySearch.Region is null
                && dailySearch.StartBYNPricePerDay is null && dailySearch.StartCurrentFloor is null && dailySearch.FinishBYNPricePerDay is null
                && dailySearch.StartRoomsCount is null && dailySearch.StartTotalFloor is null)
            {
                return new List<RentalAd>();
            }
            else
            {
                var ads = _mapper.Map<IEnumerable<DailyRentalAd>>(await _dailyRentalAdsRepository.Get());

                if (!dailySearch.Region.Equals("Любая область"))
                {
                    ads = ads.Where(ad => ad.Region.ToLower().IndexOf(dailySearch.Region.ToLower()) != -1);
                }

                if (!(dailySearch.District is null))
                {
                    ads = ads.Where(ad => ad.District.ToLower().IndexOf(dailySearch.District.ToLower()) != -1);
                }

                if (!(dailySearch.Locality is null))
                {
                    ads = ads.Where(ad => ad.Locality.ToLower().IndexOf(dailySearch.Locality.ToLower()) != -1);
                }

                if (!(dailySearch.Address is null))
                {
                    ads = ads.Where(ad => ad.Address.ToLower().IndexOf(dailySearch.Address.ToLower()) != -1);
                }

                if (!(dailySearch.StartRoomsCount is null))
                {
                    ads = ads.Where(ad => ad.TotalCountOfRooms >= dailySearch.StartRoomsCount);
                }

                if (!(dailySearch.FinishRoomsCount is null))
                {
                    ads = ads.Where(ad => ad.TotalCountOfRooms <= dailySearch.FinishRoomsCount);
                }

                if (!(dailySearch.StartCurrentFloor is null))
                {
                    ads = ads.Where(ad => ad.Floor >= dailySearch.StartCurrentFloor);
                }

                if (!(dailySearch.FinishCurrentFloor is null))
                {
                    ads = ads.Where(ad => ad.Floor <= dailySearch.FinishCurrentFloor);
                }

                if (!(dailySearch.StartTotalFloor is null))
                {
                    ads = ads.Where(ad => ad.TotalFloors >= dailySearch.StartTotalFloor);
                }

                if (!(dailySearch.FinishTotalFloor is null))
                {
                    ads = ads.Where(ad => ad.TotalFloors <= dailySearch.FinishTotalFloor);
                }

                if (dailySearch.IsFirstFloor)
                {
                    ads = ads.Where(ad => ad.Floor > 1);
                }

                if (dailySearch.IsLastFloor)
                {
                    ads = ads.Where(ad => ad.Floor < ad.TotalFloors);
                }

                if (!(dailySearch.StartBYNPricePerPerson is null))
                {
                    ads = ads.Where(ad => ad.BYNPricePerPerson >= dailySearch.StartBYNPricePerPerson);
                }

                if (!(dailySearch.FinishBYNPricePerPerson is null))
                {
                    ads = ads.Where(ad => ad.BYNPricePerPerson <= dailySearch.FinishBYNPricePerPerson);
                }

                if (!(dailySearch.StartUSDPricePerPerson is null))
                {
                    ads = ads.Where(ad => ad.USDPricePerPerson >= dailySearch.StartUSDPricePerPerson);
                }

                if (!(dailySearch.FinishUSDPricePerPerson is null))
                {
                    ads = ads.Where(ad => ad.USDPricePerPerson <= dailySearch.FinishUSDPricePerPerson);
                }

                if (!(dailySearch.StartUSDPricePerDay is null))
                {
                    ads = ads.Where(ad => ad.USDPricePerDay >= dailySearch.StartUSDPricePerDay);
                }

                if (!(dailySearch.FinishUSDPricePerDay is null))
                {
                    ads = ads.Where(ad => ad.USDPricePerDay <= dailySearch.FinishUSDPricePerDay);
                }

                if (!(dailySearch.StartBYNPricePerDay is null))
                {
                    ads = ads.Where(ad => ad.BYNPricePerDay >= dailySearch.StartBYNPricePerDay);
                }

                if (!(dailySearch.FinishBYNPricePerDay is null))
                {
                    ads = ads.Where(ad => ad.BYNPricePerDay <= dailySearch.FinishBYNPricePerDay);
                }

                return ads.ToList();
            }
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
                allRentalAdInfo.RentalAd = _mapper.Map<RentalAd>(rentalAds.First());

                var contactPersons = await _contactPersonsRepository.Get(person => person.Id.Equals(allRentalAdInfo.RentalAd.ContactPersonId));

                if (contactPersons.Count() != default)
                {
                    allRentalAdInfo.ContactPerson = _mapper.Map<ContactPerson>(contactPersons.First());
                    allRentalAdInfo.IsOriginal = false;
                }
                else
                {
                    var user = await _usersRepository.FindById(allRentalAdInfo.RentalAd.ContactPersonId);

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

                allRentalAdInfo.IsOriginal = true;
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
    }
}
