using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DependencyResolver
{
    public class DependencyResolverService
    {
        public static void RegisterApplicationLayer(IServiceCollection services_)
        {
            services_.AddScoped<IDrinkService, DrinkService>();
        }
    }
}
