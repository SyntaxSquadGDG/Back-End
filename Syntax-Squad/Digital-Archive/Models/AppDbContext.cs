using System.Collections.Generic ;
using Microsoft.EntityFrameworkCore;
namespace Digital_Archive.Models
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Folder> Folders {  get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Sfile> Sfiles { get; set; }
    }
}
