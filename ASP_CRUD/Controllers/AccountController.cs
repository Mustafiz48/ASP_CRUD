using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ASP_CRUD.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ASP_CRUD.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
                              SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }



        //Registration-Login-Logout

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

                if (result.Succeeded)
                {
                    var current_user = await _userManager.FindByNameAsync(user.Email);
                    var role = await _userManager.GetRolesAsync(current_user);

                    string role_val = role[0];
                    //return RedirectToAction("Index", "Home");

                    var token = Authenticate(user, role_val);
                    return Ok(token);

                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(user);
        }

        private String Authenticate(LoginViewModel user, string role)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.ASCII.GetBytes("This is mustak's secret key for Jwt");

            //var current_user = await _userManager.FindByNameAsync(user.Email);
            //var role = await _userManager.GetRolesAsync(current_user);

            var tokendescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, role),
                }),
                Expires = DateTime.Now.AddDays(5),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenkey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenhandler.CreateToken(tokendescriptor);
            return tokenhandler.WriteToken(token);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
