using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using TestingApp.Helpers;
using TestingApp.Core.Models.Identity;

namespace TestingApp.Filters
{
    public class UserSessionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is Controller controller)
            {
                controller.ViewBag.CurrentUser = context.HttpContext.Session.GetObject<User>("CurrentUser");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}
