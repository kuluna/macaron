using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace macaron.Models
{
    /// <summary>
    /// japaile user
    /// </summary>
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// Full name
        /// </summary>
        [Required, MinLength(1)]
        public string FullName { get; set; }
        /// <summary>
        /// Base64 icon
        /// </summary>
        public string Icon { get; set; }
    }
}
