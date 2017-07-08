using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using macaron.Models;
using macaron.Models.Request;
using macaron.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Macaron.Models.Response;

namespace macaron.Controllers
{
    /// <summary>
    /// User API
    /// </summary>
    [Route("api")]
    public class UserController : Controller
    {
        private readonly DatabaseContext db;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserController(DatabaseContext db, UserManager<AppUser> um, SignInManager<AppUser> sm)
        {
            this.db = db;
            userManager = um;
            signInManager = sm;
        }

        /// <summary>
        /// Get the user
        /// </summary>
        /// <param name="username">UserName</param>
        /// <returns>User</returns>
        /// <response code="401">Anauthorize</response>
        /// <response code="404">Not found</response>
        [HttpGet("user/{username}"), Authorize]
        public async Task<IActionResult> GetUser(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                return Ok(new AppUserResponse(user));
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create the new user
        /// </summary>
        /// <param name="req">request body</param>
        /// <returns>created user</returns>
        /// <response code="201">Creted user</response>
        /// <response code="400">Invalid request body (or duplicate)</response>
        [HttpPost("signup")]
        public async Task<IActionResult> Create([FromBody] UserCreateRequest req)
        {
            // Must delay
            await Task.Delay(1000);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // duplicate check
            var duplicate = await userManager.FindByNameAsync(req.UserName);
            if (duplicate != null)
            {
                return BadRequest($"{req.UserName} is already registed.");
            }

            // create user
            var newUser = req.ToMember();
            var create = await userManager.CreateAsync(newUser, req.Password);
            if (create.Succeeded)
            {
                await signInManager.SignInAsync(newUser, true);
                return Created(Request.Host.ToUriComponent() + "/" + newUser.UserName, new AppUserResponse(newUser));
            }
            else
            {
                return BadRequest(create.Errors);
            }
        }

        /// <summary>
        /// Delete the user
        /// </summary>
        /// <param name="username">username</param>
        /// <response code="204">Deleted</response>
        /// <response code="404">User not found</response>
        [HttpDelete("user/{username}"), Authorize]
        public async Task<IActionResult> Delete(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Sign in user
        /// </summary>
        /// <param name="req">Request body</param>
        /// <returns>Logined user</returns>
        /// <response code="200">Done Login</response>
        /// <response code="400">Invalid request body</response>
        /// <response code="401">Mistake username or password</response>
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserLoginRequest req)
        {
            // delayable
            await Task.Delay(1000);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // block double login
            await SignOut();
            
            var user = await userManager.FindByNameAsync(req.UserName);
            if (user != null)
            {
                var signin = await signInManager.PasswordSignInAsync(user, req.Password, true, false);
                if (signin.Succeeded)
                {
                    return Ok(new AppUserResponse(user));
                }
            }
            
            return Unauthorized();
        }

        /// <summary>
        /// Sign out user
        /// </summary>
        /// <response code="204">Done sign out</response>
        [HttpPost("signout")]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }
    }
}
