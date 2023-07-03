using Microsoft.EntityFrameworkCore;
using Url_Shortener.Data.Context;

namespace Url_Shortener.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddDbConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbConnection")));
        }
    }
}
