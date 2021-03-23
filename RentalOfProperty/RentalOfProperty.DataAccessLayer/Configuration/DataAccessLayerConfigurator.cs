// <copyright file="DataAccessLayerConfigurator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Configuration
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using RentalOfProperty.DataAccessLayer.Context;
    using RentalOfProperty.DataAccessLayer.Enums;
    using RentalOfProperty.DataAccessLayer.Interfaces;
    using RentalOfProperty.DataAccessLayer.Mapper;
    using RentalOfProperty.DataAccessLayer.Models;
    using RentalOfProperty.DataAccessLayer.Repositories;
    using RentalOfProperty.DataAccessLayer.Services;
    using System;

    /// <summary>
    /// Configuration for data access layer classes.
    /// </summary>
    public static class DataAccessLayerConfigurator
    {
        /// <summary>
        /// Method with dependency injection settings.
        /// </summary>
        /// <param name="services">Main service sollection.</param>
        /// <param name="configuration">Main configuration.</param>
        public static void ConfigureDataAccessLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            const int RequiredPasswordLength = 8, UniqueChars = 1;
            const string ConnectionString = "RentalOfPropertyConnection";

            // Db connection settings
            services.AddDbContext<RentalOfPropertyContext>(
                options => options
                .UseSqlServer(configuration.GetConnectionString(ConnectionString)), ServiceLifetime.Transient);

            // Identity settings
            services.AddIdentity<UserDTO, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = RequiredPasswordLength;
                options.Password.RequiredUniqueChars = UniqueChars;
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddErrorDescriber<MultilanguageIdentityErrorDescriber>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<RentalOfPropertyContext>();

            // Adding classes injections
            services.AddTransient<IUserRepository, UsersRepository>();
            services.AddScoped<GoHomeByDataLoader>();

            services.AddScoped<Func<LoaderMenu, IDataLoader>>(serviceProvider => loaderMenu =>
            {
                return loaderMenu switch
                {
                    LoaderMenu.GoHomeBy => serviceProvider.GetService<GoHomeByDataLoader>(),
                    LoaderMenu.RealtBY => null,
                    _ => null,
                };
            });

            // Adding automapper
            services.AddAutoMapper(typeof(DataAccessProfile));
        }
    }
}
