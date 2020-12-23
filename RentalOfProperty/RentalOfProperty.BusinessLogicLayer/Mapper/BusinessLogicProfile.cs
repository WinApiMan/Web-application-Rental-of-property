﻿// <copyright file="BusinessLogicProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Mapper
{
    using AutoMapper;
    using RentalOfProperty.BusinessLogicLayer.Models;
    using RentalOfProperty.DataAccessLayer.Models;

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
        }
    }
}