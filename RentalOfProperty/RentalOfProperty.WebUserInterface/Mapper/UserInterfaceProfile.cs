﻿// <copyright file="UserInterfaceProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.WebUserInterface.Mapper
{
    using AutoMapper;
    using RentalOfProperty.BusinessLogicLayer.Models;
    using RentalOfProperty.WebUserInterface.Models.User;

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
        }
    }
}
