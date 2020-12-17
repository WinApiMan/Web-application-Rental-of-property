using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentalOfProperty.DataAccessLayer.Configuration;

namespace RentalOfProperty.BusinessLogicLayer.Configuration
{
    public static class BusinessLogicLayerConfigurator
    {
        public static void ConfigureBusinessLogicLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Adding dal configuration
            services.ConfigureDataAccessLayerServices(configuration);
        }
    }
}
