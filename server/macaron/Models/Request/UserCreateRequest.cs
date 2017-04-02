using System.ComponentModel.DataAnnotations;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(User create)
    /// </summary>
    public class UserCreateRequest
    {
        /// <summary>
        /// User name
        /// </summary>
        [Required, MinLength(1)]
        [RegularExpression(@"^[a-z]+$", ErrorMessage = "The UserName field should be lowercase a-z.")]
        public string UserName { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [Required, MinLength(6)]
        public string Password { get; set; }

        /// <summary>
        /// Convert to User class
        /// </summary>
        /// <returns>User</returns>
        public AppUser ToMember()
        {
            return new AppUser()
            {
                UserName = UserName,
                FullName = UserName,
                Email = Email
            };
        }
    }
}
