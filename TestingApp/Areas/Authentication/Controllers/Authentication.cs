using TestingApp.Areas.Authentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingApp.Core.Models.Identity;
using TestingApp.Database;
using TestingApp.Core.Models.Identity.Enums;
using TestingApp.Helpers;
using TestingApp.Security;

namespace TestingApp.Areas.Authentication.Controllers
{
    [Area($"{nameof(Authentication)}")]
    [Route("[controller]")]
    public class Authentication : Controller
    {
        private DatabaseContext _databaseContext { get; }

        public Authentication(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login(AuthenticationData authenticationData)
        {
            if(!ModelState.IsValid)
            {
                return View(authenticationData);
            }

            var user = await _databaseContext.Users.FirstOrDefaultAsync(p => p.Login == authenticationData.Login && p.Password == authenticationData.Password);
            if (user != null)
            {
                HttpContext.Session.SetObject("CurrentUser", user);
                var userToken = new JwtTokenSecurity().GenerateToken(user.Name);
                Response.Cookies.Append("AuthToken", userToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddHours(1)
                });

                return RedirectToAction($"{nameof(Dashboard.Controllers.Dashboard.General)}", $"{nameof(Dashboard.Controllers.Dashboard)}", new { area = $"{nameof(Dashboard)}" });
            }
            else
            {
                ModelState.AddModelError("Uncorrect", "Неправильный логин или пароль");
                return View(authenticationData);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Registration(RegistrationData data)
        {
            if(ModelState.IsValid)
            {
                if(_databaseContext.Users.Any(p => p.Login == data.Login))
                {
                    ModelState.AddModelError("Login", "Данный логин уже используется в системе");
                    return View(data);
                }

                var user = new User()
                {
                    Name = data.Name,
                    Login = data.Login,
                    Password = data.Password
                };

                if(_databaseContext.Users.Count() == 0)
                {
                    user.RoleType = RoleType.Admin;
                }
                else
                {
                    user.RoleType = RoleType.User;
                }

                await _databaseContext.Users.AddAsync(user);
                await _databaseContext.SaveChangesAsync();

                return RedirectToAction($"{nameof(Login)}");
            }
            else
            {
                return View(data);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CheckAvailabilityLogin(string Login)
        {
            if(_databaseContext.Users.Any(p => p.Login == Login))
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("CurrentUser");
            Response.Cookies.Delete("AuthToken");

            return View("Login");
        }
    }
}
