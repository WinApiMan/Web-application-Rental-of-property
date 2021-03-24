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
        public AdsManager(Func<LoaderMenu, IDataLoader> serviceResolver, IMapper mapper, IRepository<AditionalAdDataDTO> aditionalAdDatasRepository, IRepository<ContactPersonDTO> contactPersonsRepository, IRepository<HousingPhotoDTO> housingPhotosRepository, IRepository<RentalAdDTO> rentalAdsRepository, IRepository<DailyRentalAdDTO> dailyRentalAdsRepository, IRepository<LongTermRentalAdDTO> longTermRentalAdsRepository)
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

                    foreach (var id in contactPersonsId)
                    {
                        var tempContactPersons = await _contactPersonsRepository.Get(contactPerson => !string.IsNullOrEmpty(contactPerson.Id));
                        oldContactPersons.Add(tempContactPersons.First());
                    }

                    if (oldContactPersons.Count > newAds.Count)
                    {
                        var newContactPersons = newAds.Select(ad => ad.ContactPerson);
                        var removableContactPersons = oldContactPersons.Except(newContactPersons);

                        foreach (var contactPerson in removableContactPersons)
                        {
                            await _contactPersonsRepository.Remove(contactPerson);
                        }
                    }
                    else if (oldContactPersons.Count < newAds.Count)
                    {
                        var newContactPersons = newAds.Select(ad => ad.ContactPerson);
                        var addableContactPersons = newContactPersons.Except(oldContactPersons);

                        foreach (var contactPerson in addableContactPersons)
                        {
                            contactPerson.Id = Guid.NewGuid().ToString();
                            await _contactPersonsRepository.Create(contactPerson);
                            var ad = newAds.FirstOrDefault(ad => ad.ContactPerson.Equals(contactPerson));

                            ad.RentalAd.ContactPersonId = contactPerson.Id;
                            await _dailyRentalAdsRepository.Create(ad.RentalAd as DailyRentalAdDTO);
                            var tempAd = await _rentalAdsRepository.Get(tempAd => tempAd.RentalAdNumber == ad.RentalAd.RentalAdNumber);

                            foreach (var photo in ad.HousingPhotos)
                            {
                                photo.RentalAdId = tempAd.First().Id;
                                await _housingPhotosRepository.Create(photo);
                            }

                            ad.AditionalAdData.Id = tempAd.First().Id;

                            await _aditionalAdDatasRepository.Create(ad.AditionalAdData);
                        }
                    }

                    break;

                case LoadDataFromSourceMenu.GoHomeByLongTermAds:
                    _goHomeByDataLoader.LoadDataInAds(RentalAdMenu.RealtApartmentsByLongTermRentalAd);
                    _goHomeByDataLoader.LoadDataInAds(RentalAdMenu.RealtHousesByLongTermRentalAd);

                    newAds = _goHomeByDataLoader.Ads;
                    break;

                default:
                    return;
            }
        }
    }
}
