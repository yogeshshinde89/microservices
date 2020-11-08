using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTAuthentication_Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace JWTAuthentication_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpDelete]
        [Route("Deleteuser")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> DeleteUser(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (username == null || user == null)
            {
                return NotFound();
            }


            //List Logins associated with user
            //var logins = user.;
            //Gets list of Roles associated with current user

            var rolesForUser = await userManager.GetRolesAsync(user);
            var status = await userManager.DeleteAsync(user);
            if (status.Succeeded == true)
            {
                return Ok(new Response { Status = "Success", Message = "User Deleted successfully!" });
            }
            else
            {
                return Ok(new Response { Status = "Failure", Message = "Something went wrong while deleting the record!" });
            }

        }

        [HttpPut]
        [Route("/api/Updatepassword")]
        public async Task<ActionResult> UpdatePassword(string username, string currentpassword, string newpassword)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user != null && await userManager.CheckPasswordAsync(user, currentpassword))
            {
                var status = await userManager.ChangePasswordAsync(user, currentpassword, newpassword);
                if (status.Succeeded)
                {
                    return Ok(new Response { Status = "Success", Message = "Password Changed successfully!" });
                }
                else
                {
                    return Ok(new Response { Status = "Error", Message = "Something went wrong while updating password!" });
                }
            }
            else
            {
                return BadRequest(new Response { Status = "Unauthorised", Message = "Password is incorrect!" });
            }
        }

        //[HttpPost]
        //[Route("/api/ForgotPassword")]
        //public async Task<IActionResult> ForgotPassword(string username)
        //{
        //    var user = await userManager.FindByNameAsync(username);

        //    if (user != null)
        //    {
        //        await userManager.GeneratePasswordResetTokenAsync(user);
        //    }
        //}

    }
}
