using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentalOfProperty.DataAccessLayer.Context;
using RentalOfProperty.DataAccessLayer.Models;

namespace RentalOfProperty.DataAccessLayer.Configuration
{
    public static class DataAccessLayerConfigurator
    {
        public static void ConfigureDataAccessLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            const int RequiredPasswordLength = 8;
            const string ConnectionString = "RentalOfPropertyConnection";

            //Db connection settings
            services.AddDbContext<RentalOfPropertyContext>(options => options
                .UseSqlServer(configuration.GetConnectionString(ConnectionString)), ServiceLifetime.Transient);

            //Identity settings
            services.AddIdentity<UserDTO, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = RequiredPasswordLength;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<RentalOfPropertyContext>();
        }
    }
}
