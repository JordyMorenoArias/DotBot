using DotBot.Models.DTOs.User;
using DotBot.Models.ViewModels;
using DotBot.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotBot.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            if (!ModelState.IsValid)
            {
                return View(userLogin);
            }

            try
            {
                var authResponse = await _authService.Login(userLogin);
                HttpContext.Session.SetString("Token", authResponse.Token);
                return RedirectToAction("Index", "Chat");
            }
            catch (UnauthorizedAccessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                var loginViewModel = new LoginViewModel
                {
                    Email = userLogin.Email,
                };

                return View(loginViewModel);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterDto userRegister)
        {
            if (!ModelState.IsValid)
            {
                return View(userRegister);
            }

            try
            {
                var result = _authService.Register(userRegister).Result;

                if (result)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                    return View(userRegister);
                }
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(userRegister);
            }
        }
    }
}
