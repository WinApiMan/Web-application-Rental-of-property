// <copyright file="DataAccessProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Mapper
{
    using AutoMapper;
    using RentalOfProperty.DataAccessLayer.Models;

    /// <summary>
    /// Class for mapping settings.
    /// </summary>
    public class DataAccessProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessProfile"/> class.
        /// </summary>
        public DataAccessProfile()
        {
            CreateMap<DailyRentalAdDTO, RentalAdDTO>().ReverseMap();
            CreateMap<LongTermRentalAdDTO, RentalAdDTO>().ReverseMap();
        }
    }
}
