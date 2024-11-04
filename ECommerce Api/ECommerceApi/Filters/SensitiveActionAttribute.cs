using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace ECommerce.Api.Filters
{
    public class SensitiveActionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Debug.WriteLine("Sensitive action excuted !!!!!!");
        }
    }
}
