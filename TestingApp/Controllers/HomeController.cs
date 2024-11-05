using TestingApp.Areas.Authentication.Controllers;
using TestingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Diagnostics;

namespace TestingApp.Controllers
{
    public class HomeController : Controller
    {
        private ILogger<HomeController> _logger;
        private IEnumerable<EndpointDataSource> _endpointSources;

        public HomeController(ILogger<HomeController> logger, IEnumerable<EndpointDataSource> endpointSources)
        {
            _logger = logger;
            _endpointSources = endpointSources;
        }

        public IActionResult Index()
        {
            return Redirect($"/{nameof(Authentication)}/{nameof(Authentication.Login)}");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("/[action]")]
        public IActionResult GetRoutes()
        {
            var endpoints = _endpointSources.SelectMany(es => es.Endpoints).OfType<RouteEndpoint>();
            var output = endpoints.Select(e =>
                {
                    var controller = e.Metadata
                        .OfType<ControllerActionDescriptor>()
                        .FirstOrDefault();
                    var action = controller != null
                        ? $"{controller.ControllerName}.{controller.ActionName}"
                        : null;
                    var controllerMethod = controller != null
                        ? $"{controller.ControllerTypeInfo.FullName}:{controller.MethodInfo.Name}"
                        : null;
                    return new
                    {
                        Method = e.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault()?.HttpMethods?[0],
                        Route = $"/{e.RoutePattern.RawText.TrimStart('/')}",
                        Action = action,
                        ControllerMethod = controllerMethod
                    };
                }
            );

            return Json(output);
        }
    }
}
