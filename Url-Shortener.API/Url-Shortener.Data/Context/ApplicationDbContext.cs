using Microsoft.EntityFrameworkCore;
using Url_Shortener.Models.Entities;

namespace Url_Shortener.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options){ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Url> Urls { get; set; }
    }
}
