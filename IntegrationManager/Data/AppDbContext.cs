using Microsoft.EntityFrameworkCore;
using IntegrationManager.Models;

namespace IntegrationManager.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<IntegrationMaster> IntegrationMasters { get; set; }
    }
}
