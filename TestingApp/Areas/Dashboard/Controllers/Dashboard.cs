using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestingApp.Database;

namespace TestingApp.Areas.Dashboard.Controllers
{
    [Area($"{nameof(Dashboard)}")]
    [Route("[controller]")]
    [Authorize]
    public class Dashboard : Controller
    { 
        private DatabaseContext _databaseContext { get; }

        public Dashboard(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult General()
        {
            return View(_databaseContext.Tasks);
        }
    }
}