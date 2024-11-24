using Microsoft.EntityFrameworkCore;
using TestingApp.Areas.Tasking.Models;
using TestingApp.Core.Models.Identity;
using TestingApp.Core.Models.Tests;

namespace TestingApp.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Source> Sources { get; set; } = null!;
        public DbSet<TaskTesting> Tasks { get; set; } = null!;
        public DbSet<Test> Tests { get; set; } = null!;
        public DbSet<TestExecuteHistory> TestsExecuteHistories { get; set; } = null!;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }
    }
}
