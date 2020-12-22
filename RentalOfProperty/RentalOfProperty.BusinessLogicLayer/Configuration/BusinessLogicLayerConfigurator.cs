﻿// <copyright file="BusinessLogicLayerConfigurator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.BusinessLogicLayer.Configuration
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using RentalOfProperty.DataAccessLayer.Configuration;

    /// <summary>
    /// Configuration for business logic layer classes.
    /// </summary>
    public static class BusinessLogicLayerConfigurator
    {
        /// <summary>
        /// Method with dependency injection settings.
        /// </summary>
        /// <param name="services">Main service sollection.</param>
        /// <param name="configuration">Main configuration.</param>
        public static void ConfigureBusinessLogicLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Adding dal configuration
            services.ConfigureDataAccessLayerServices(configuration);
        }
    }
}