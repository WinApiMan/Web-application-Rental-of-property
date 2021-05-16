// <copyright file="UserInterfaceProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Mapper
{
    using AutoMapper;
    using RentalOfProperty.BusinessLogicLayer.Models;
    using RentalOfProperty.WebUserInterface.Enums;
    using RentalOfProperty.WebUserInterface.Models.Ad;
    using RentalOfProperty.WebUserInterface.Models.User;
    using BLLLoadDataFromSourceMenu = BusinessLogicLayer.Enums.LoadDataFromSourceMenu;
    using BLLAdsTypeMenu = BusinessLogicLayer.Enums.AdsTypeMenu;
    using RentalOfProperty.WebUserInterface.Models;

    /// <summary>
    /// Class for mapping settings.
    /// </summary>
    public class UserInterfaceProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceProfile"/> class. Фdding mappers for entities.
        /// </summary>
        public UserInterfaceProfile()
        {
            CreateMap<RegisterView, User>().ReverseMap();
            CreateMap<AuthorizeView, SignInUser>().ReverseMap();
            CreateMap<EditView, User>().ReverseMap();

            CreateMap<RentalAdView, RentalAd>().ReverseMap();
            CreateMap<DailyRentalAdView, DailyRentalAd>().ReverseMap();
            CreateMap<LongTermRentalAdView, LongTermRentalAd>().ReverseMap();
            CreateMap<HousingPhotoView, HousingPhoto>().ReverseMap();
            CreateMap<ContactPersonView, ContactPerson>().ReverseMap();
            CreateMap<AditionalAdDataView, AditionalAdData>().ReverseMap();
            CreateMap<AllRentalAdInfoView, AllRentalAdInfo>().ReverseMap();

            CreateMap<LoadDataFromSourceMenu, BLLLoadDataFromSourceMenu>().ReverseMap();
            CreateMap<AdsTypeMenu, BLLAdsTypeMenu>().ReverseMap();

            CreateMap<SearchView, Search>().ReverseMap();
            CreateMap<DailySearchView, DailySearch>().ReverseMap();
            CreateMap<LongTermSearchView, LongTermSearch>().ReverseMap();
            CreateMap<CreateView, CreateModel>().ReverseMap();
            CreateMap<GeneralSearchView, GeneralSearch>().ReverseMap();
            CreateMap<CityViewsModel, CityViews>().ReverseMap();
        }
    }
}
