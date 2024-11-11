using Microsoft.AspNetCore.Mvc;
using TestingApp.Core.Models.Identity;
using TestingApp.Core.Models.Tests;
using TestingApp.Database;

namespace TestingApp.Areas.Dashboard.Controllers
{
    [Area($"{nameof(Dashboard)}")]
    [Route("[controller]")]
    public class Dashboard : Controller
    {
        public IEnumerable<Source>? Sources { get; set; }

        private DatabaseContext _databaseContext { get; }

        public Dashboard(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult General(User user)
        {
            Sources = _databaseContext.Sources.Where(p => p.Owner == user);
            return View(this);
        }
    }
}
