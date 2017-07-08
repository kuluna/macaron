using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(user login)
    /// </summary>
    public class UserLoginRequest
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required, AppUserName]
        public string UserName { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [Required, MinLength(6)]
        public string Password { get; set; }
    }
}
