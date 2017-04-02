using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using macaron.Models;
using macaron.Models.Request;
using macaron.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace macaron.Controllers
{
    /// <summary>
    /// User API
    /// </summary>
    [Route("api/user")]
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
        [HttpGet("{username}"), Authorize]
        public async Task<IActionResult> GetMember(string username)
        {
            var member = await userManager.FindByNameAsync(username);
            if (member != null)
            {
                return Ok(member);
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
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserCreateRequest req)
        {
            // delayable
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
            var newMember = req.ToMember();
            var create = await userManager.CreateAsync(newMember, req.Password);
            if (create.Succeeded)
            {
                await signInManager.SignInAsync(newMember, true);
                return Created(Request.Host.ToUriComponent() + "/" + newMember.UserName, newMember);
            }
            else
            {
                return BadRequest(create.Errors);
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
            
            var member = await userManager.FindByEmailAsync(req.Email);
            if (member != null)
            {
                var signin = await signInManager.PasswordSignInAsync(member, req.Password, true, false);
                if (signin.Succeeded)
                {
                    return Ok(member);
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
