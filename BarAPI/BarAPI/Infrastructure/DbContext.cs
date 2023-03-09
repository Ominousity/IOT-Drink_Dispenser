using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext(DbContextOptions<DbContext> options_, ServiceLifetime service_) : base(options_)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder_)
        {

        }

        public DbSet<drink> _drinkEntries { get; set; }
    }
}
