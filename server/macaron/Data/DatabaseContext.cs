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
    }
}
