// <copyright file="Startup.cs" company="RentalOfPropertyCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty
{
    using System.Globalization;
    using AutoMapper;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using RentalOfProperty.BusinessLogicLayer.Configuration;
    using RentalOfProperty.WebUserInterface;
    using RentalOfProperty.WebUserInterface.Mapper;

    /// <summary>
    /// Class with start settings.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Main configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets main configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Method with services settings.
        /// </summary>
        /// <param name="services">Main services collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add localization
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddControllersWithViews()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                const string English = "en", Russian = "ru";

                var supportedCultures = new[]
                {
                    new CultureInfo(Russian),
                    new CultureInfo(English),
                };

                options.DefaultRequestCulture = new RequestCulture(Russian);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddDistributedMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Adding session
            services.AddSession();

            // Adding logger
            services.AddLogging(loggingBuilder => loggingBuilder.AddFile("Logs/RentalOfProperty-{Date}.txt"));

            services.AddAutoMapper(typeof(UserInterfaceProfile));

            // Adding bll configuration
            services.ConfigureBusinessLogicLayerServices(Configuration);
        }

        /// <summary>
        /// Method with application settings.
        /// </summary>
        /// <param name="app">Application builder object.</param>
        /// <param name="env">Web host enviroment object.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRequestLocalization();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Installation default route
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
