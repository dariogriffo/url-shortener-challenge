using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using Url_Shortener.Data.Context;
using Url_Shortener.Data.Repository.Implementations;
using Url_Shortener.Data.Repository.Interfaces;
using Url_Shortener.Models.Entities;
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

        public static void AddRedirect(this WebApplication app)
        {
            app.MapFallback(async context =>
            {
                string url = context.Request.GetDisplayUrl();
                
                var db = app.Services.CreateScope().ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

                var urlService = app.Services.CreateScope().ServiceProvider
                    .GetRequiredService<IUrlService>();

                var urlId = urlService.Decode(url);

                if (urlId == 0)
                {
                    context.Response.Redirect(url);
                    return;
                }

                Url result = db.Urls.First(x => x.Id.Equals(urlId));
                if (result == null)
                    throw new ArgumentNullException();

                context.Response.Redirect(result!.LongUrl);
            });
        }
    }
}
