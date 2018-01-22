using System.Linq;
using System.Threading.Tasks;
using Business.Services;
using DotNetXPlat.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Omu.ValueInjecter;

namespace Web.Areas.API
{
    [Area("API")]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly IJwtFactory jwtFactory;

        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IJwtFactory jwtFactory)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.jwtFactory = jwtFactory;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(string username, string password)
        {
            var result = await signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);
            if (!result.Succeeded) return BadRequest();

            var user = userManager.Users.Single(u => u.UserName == username);
            return Ok(jwtFactory.GenerateJwtToken(user));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return Ok("User signed out.");
        }

        [HttpPut]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new ApplicationUser();
            user.InjectFrom(model);
            user.UserName = user.Email;

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return await SignIn(model.Email, model.Password);
        }

    }
}