using TestingApp.Areas.Authentication.Models;
using TestingApp.Areas.Authentication.Models.Configurations;
using TestingApp.Database;
using TestingApp.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestingApp.Areas.Authentication.Controllers
{
    [Area($"{nameof(Authentication)}")]
    [Route("[controller]")]
    public class Authentication : Controller
    {
        private AuthenticationConfiguration _authenticationConfiguration { get; }
        private DatabaseContext _databaseContext { get; }

        public Authentication(AuthenticationConfiguration authenticationConfiguration, DatabaseContext databaseContext)
        {
            _authenticationConfiguration = authenticationConfiguration;
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
                return RedirectToAction($"{nameof(Dashboard.Controllers.Dashboard.General)}", $"{nameof(Dashboard.Controllers.Dashboard)}", new { area = $"{nameof(Dashboard)}", user });
            }
            else
            {
                ModelState.AddModelError("Uncorrect", "Неправильный логин или пароль");
                return NotFound();
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
    }
}
