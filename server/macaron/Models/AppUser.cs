using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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

    /// <summary>
    /// Specifies that a data field value is username. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class AppUserNameAttribute: ValidationAttribute
    {
        /// <summary>
        /// check the username.
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>True is username.</returns>
        public override bool IsValid(object value)
        {
            if (value is string)
            {
                return Regex.IsMatch(value as string, "^[a-zA-Z0-9]+$");
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Format the error message.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name) => $"{name} is not username.";
    }
}
