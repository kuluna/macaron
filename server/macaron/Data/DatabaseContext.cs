using macaron.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace macaron.Data
{
    /// <summary>
    /// Database access
    /// </summary>
    public class DatabaseContext : IdentityDbContext<AppUser>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        /// <summary>
        /// Projects
        /// </summary>
        public DbSet<Project> Projects { get; set; }
        /// <summary>
        /// Testcases
        /// </summary>
        public DbSet<Case> Cases { get; set; }
        /// <summary>
        /// Test plans
        /// </summary>
        public DbSet<Plan> Plans { get; set; }
        /// <summary>
        /// Run test logs
        /// </summary>
        public DbSet<Run> Runs { get; set; }
    }
}
