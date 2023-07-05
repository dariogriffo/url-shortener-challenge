using Microsoft.EntityFrameworkCore;
using Url_Shortener.Data.Context;
using Url_Shortener.Data.Repository.Implementations;
using Url_Shortener.Data.Repository.Interfaces;
using Url_Shortener.Services.Implementations;
using Url_Shortener.Services.Interfaces;

namespace Url_Shortener.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddDbConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbConnection")));
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();
            services.AddScoped<IUrlService, UrlService>();

            return services;
        }
    }
}
