// <copyright file="BusinessLogicProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Mapper
{
    using AutoMapper;
    using RentalOfProperty.BusinessLogicLayer.Models;
    using RentalOfProperty.DataAccessLayer.Models;
    using DALIdentityResult = Microsoft.AspNetCore.Identity.IdentityResult;
    using DALSignInResult = Microsoft.AspNetCore.Identity.SignInResult;

    /// <summary>
    /// Class for mapping settings.
    /// </summary>
    public class BusinessLogicProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessLogicProfile"/> class.
        /// </summary>
        public BusinessLogicProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<SignInUser, SignInUserDTO>().ReverseMap();
            CreateMap<SignInResult, DALSignInResult>().ReverseMap();

            CreateMap<AditionalAdDataDTO, AditionalAdData>().ReverseMap();
            CreateMap<ContactPersonDTO, ContactPerson>().ReverseMap();
            CreateMap<HousingPhotoDTO, HousingPhoto>().ReverseMap();
            CreateMap<RentalAdDTO, RentalAd>().ReverseMap();
            CreateMap<AdDTO, Ad>().ReverseMap();
            CreateMap<DailyRentalAdDTO, DailyRentalAd>().ReverseMap();
            CreateMap<LongTermRentalAdDTO, LongTermRentalAd>().ReverseMap();
            CreateMap<UserRentalAdDTO, UserRentalAd>().ReverseMap();
        }
    }
}
