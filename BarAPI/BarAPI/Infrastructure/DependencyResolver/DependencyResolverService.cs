using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.DependencyResolver
{
    public class DependencyResolverService
    {
        public static void RegisterInfrastructureLayer(IServiceCollection services_)
        {
            services_.AddScoped<IDrinkRepository, DrinkRepository>();
        }
    }
}
