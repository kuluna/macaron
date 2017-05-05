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
        public DbSet<Testcase> Testcases { get; set; }
        /// <summary>
        /// Test plans
        /// </summary>
        public DbSet<Testplan> Testplans { get; set; }
        /// <summary>
        /// Run test logs
        /// </summary>
        public DbSet<Testrun> Testruns { get; set; }
    }
}
