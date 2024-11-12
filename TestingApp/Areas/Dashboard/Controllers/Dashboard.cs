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
        public User? CurrentUser { get; set; } 
        public IEnumerable<Source>? Sources { get; set; }

        private DatabaseContext _databaseContext { get; }

        public Dashboard(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult General(Guid userID)
        {
            CurrentUser = _databaseContext.Users.FirstOrDefault(p => p.ID == userID);
            Sources = _databaseContext.Sources.Where(p => p.OwnerID == userID);
            return View(this);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult GetCreateTaskPartialPage([FromBody] UserProperties userProperties)
        {
            return PartialView("_CreateTask");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateSource(Source source)
        {
            source.OwnerID = CurrentUser.ID;

            await _databaseContext.Sources.AddAsync(source);
            await _databaseContext.SaveChangesAsync();

            return PartialView();
        }
    }

    public class UserProperties
    {
        public Guid userID { get; set; }
    }
}
