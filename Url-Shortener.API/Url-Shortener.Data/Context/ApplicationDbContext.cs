using Microsoft.EntityFrameworkCore;

namespace Url_Shortener.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options){ }
    }
}
