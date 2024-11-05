using TestingApp.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace TestingApp.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }
    }
}
