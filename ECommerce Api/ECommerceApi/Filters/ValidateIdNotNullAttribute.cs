using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerce.Api.Filters
{
    public class ValidateIdNotNullAttribute : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var found = context.ActionArguments.ContainsKey("id");
            if (!found)
            {
                context.Result = new BadRequestObjectResult("ID is missing.");
                return;
            }

            var id = context.ActionArguments["id"];

            if (id == null || (int)id <= 0)
            {
                context.Result = new BadRequestObjectResult("Invalid ID.");
                return;
            }

            await next();
        }
    }
}
