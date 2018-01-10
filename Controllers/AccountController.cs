using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using DotNetXPlat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using Omu.ValueInjecter;

namespace DotNetXPlat.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            RoleManager<IdentityRole> _roleManager,
            ILoggerFactory loggerFactory)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._roleManager = _roleManager;
            this._logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpGet]
        public ActionResult Login(string returnUrl = null)
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("LoginError", "Invalid login attempt.");
                return View(model);
            }

            _logger.LogInformation(1, "User logged in.");
            return Redirect(model.ReturnUrl ?? "/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(2, "User logged out.");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            var model = new RegisterViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            var user = new ApplicationUser();
            user.InjectFrom(model);
            user.UserName = user.Email;

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return View(model);
            }

            addPermissions(user);

            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation(3, "User created a new account with password.");
            return Redirect(returnUrl ?? "/");
        }

        private async void addPermissions(ApplicationUser user)
        {
            if (!await _roleManager.RoleExistsAsync(Roles.Admin))
            {
                var adminRole = new IdentityRole(Roles.Admin);
                await _roleManager.CreateAsync(adminRole);

                var claimTasks = new Task[] {
                        _roleManager.AddClaimAsync(adminRole, new Claim(Claims.Product.View, "")),
                        _roleManager.AddClaimAsync(adminRole, new Claim(Claims.Product.Edit, ""))
                    };
                Task.WaitAll(claimTasks);
            }

            if (!await _userManager.IsInRoleAsync(user, Roles.Admin))
            {
                await _userManager.AddToRoleAsync(user, Roles.Admin);
            }
        }
    }
}