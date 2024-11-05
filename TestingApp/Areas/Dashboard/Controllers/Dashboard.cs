using TestingApp.Areas.Authentication.Models;
using TestingApp.Database;
using TestingApp.Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace TestingApp.Areas.Dashboard.Controllers
{
    [Area($"{nameof(Dashboard)}")]
    [Route("[controller]")]
    public class Dashboard : Controller
    {
        private DatabaseContext _databaseContext { get; }

        public Dashboard(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult General(User user)
        {
            return View();
        }
    }
}
