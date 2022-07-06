using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration _config)  {
            services.AddScoped<ITokenService, TokenService>();

            // Adding the DbContext to Startup class so that we can inject the data context to other parts of our application
            services.AddDbContext<DataContext>(options => {
                options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}