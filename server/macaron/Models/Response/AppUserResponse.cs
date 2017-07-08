using macaron.Data;
using macaron.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Macaron.Models.Response
{
    /// <summary>
    /// Response body(User)
    /// </summary>
    public class AppUserResponse
    {
        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// User full name
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Icon dat(base64)
        /// </summary>
        public string IconDat { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">AppUser model</param>
        public AppUserResponse(AppUser model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            UserName = model.UserName;
            FullName = model.FullName;
            Email = model.Email;
            IconDat = model.Icon;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="um"></param>
        /// <param name="userName"></param>
        public AppUserResponse(UserManager<AppUser> um, string userName): this(um.FindByNameAsync(userName).Result) { }
    }
}
