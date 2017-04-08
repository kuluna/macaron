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
        /// Milestones
        /// </summary>
        public DbSet<Milestone> Milestones { get; set; }
        /// <summary>
        /// Platforms
        /// </summary>
        public DbSet<Platform> Platforms { get; set; }
        /// <summary>
        /// Testcases
        /// </summary>
        public DbSet<Testcase> Testcases { get; set; }

    }
}
