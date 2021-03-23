// <copyright file="AdsManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using RentalOfProperty.BusinessLogicLayer.Enums;
    using RentalOfProperty.BusinessLogicLayer.Interfaces;
    using RentalOfProperty.BusinessLogicLayer.Models;
    using RentalOfProperty.DataAccessLayer.Enums;
    using RentalOfProperty.DataAccessLayer.Interfaces;

    /// <summary>
    /// Ads manager.
    /// </summary>
    public class AdsManager : IAdsManager
    {
        private readonly IDataLoader _goHomeByDataLoader;

        private readonly IDataLoader _realtByDataLoader;

        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdsManager"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service provider model.</param>
        /// <param name="mapper">Mapper model.</param>
        public AdsManager(Func<LoaderMenu, IDataLoader> serviceProvider, IMapper mapper)
        {
            _goHomeByDataLoader = serviceProvider(LoaderMenu.GoHomeBy);
            _realtByDataLoader = serviceProvider(LoaderMenu.RealtBY);
            _mapper = mapper;
        }

        /// <summary>
        /// Load long term ads from GoHome.by site.
        /// </summary>
        /// <param name="loadDataFromSourceMenuItem">Menu item.</param>
        /// <returns>Task result.</returns>
        public async Task LoadLongTermAdsFromGoHomeBy(LoadDataFromSourceMenu loadDataFromSourceMenuItem)
        {
            switch (loadDataFromSourceMenuItem)
            {
                case LoadDataFromSourceMenu.GoHomeByDailyAds:
                    _goHomeByDataLoader.LoadDataInAds(RentalAdMenu.RealtApartmentsByDailyRentalAd);
                    _goHomeByDataLoader.LoadDataInAds(RentalAdMenu.RealtHousesByDailyRentalAd);

                    var ads = _mapper.Map<IEnumerable<Ad>>(_goHomeByDataLoader.Ads);
                    break;

                case LoadDataFromSourceMenu.GoHomeByLongTermAds:
                    _goHomeByDataLoader.LoadDataInAds(RentalAdMenu.RealtApartmentsByLongTermRentalAd);
                    _goHomeByDataLoader.LoadDataInAds(RentalAdMenu.RealtHouseByLongTermRentalAd);

                    ads = _mapper.Map<IEnumerable<Ad>>(_goHomeByDataLoader.Ads);
                    break;

                default:
                    return;
            }
        }
    }
}
