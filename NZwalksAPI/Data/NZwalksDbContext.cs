using Microsoft.EntityFrameworkCore;
using NZwalksAPI.Models.Domain;

namespace NZwalksAPI.Data

{
    public class NZwalksDbContext : DbContext
    {
        public NZwalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; } 

    }
}
