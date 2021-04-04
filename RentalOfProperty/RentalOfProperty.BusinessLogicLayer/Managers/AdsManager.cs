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

        private readonly IAdsFilter<RentalAdDTO> _adsFilterRepository;

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
        /// <param name="adsFilterRepository">Ads filter repository.</param>
        public AdsManager(Func<LoaderMenu, IDataLoader> serviceResolver, IMapper mapper, IRepository<AditionalAdDataDTO> aditionalAdDatasRepository, IRepository<ContactPersonDTO> contactPersonsRepository, IRepository<HousingPhotoDTO> housingPhotosRepository, IRepository<RentalAdDTO> rentalAdsRepository, IRepository<DailyRentalAdDTO> dailyRentalAdsRepository, IRepository<LongTermRentalAdDTO> longTermRentalAdsRepository, IAdsFilter<RentalAdDTO> adsFilterRepository)
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
            _adsFilterRepository = adsFilterRepository;
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
                    var addingHousingPhotos = new List<HousingPhotoDTO>();

                    foreach (var ad in newAds)
                    {
                        var aditionalAdData = oldAditionalAdDatas.FirstOrDefault(adData => adData.RentalAdNumber == ad.AditionalAdData.RentalAdNumber);

                        if (aditionalAdData is null)
                        {
                            addingAditionalAdDatas.Add(ad.AditionalAdData);
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
                    addingHousingPhotos = new List<HousingPhotoDTO>();

                    foreach (var ad in newAds)
                    {
                        var aditionalAdData = oldAditionalAdDatas.FirstOrDefault(adData => adData.RentalAdNumber == ad.AditionalAdData.RentalAdNumber);

                        if (aditionalAdData is null)
                        {
                            addingAditionalAdDatas.Add(ad.AditionalAdData);
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
    }
}
