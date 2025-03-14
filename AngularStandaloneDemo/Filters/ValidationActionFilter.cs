using AngularStandaloneDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace AngularStandaloneDemo.Filters
{
    public class ValidationActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var errorResponse = new CustomErrorResponse
                {
                    StatusCode = 400,
                    Message = "Validation Failed",
                    Errors = errors // This should be a list of strings
                };

                // Return the ModelState directly to use ASP.NET Core's standard validation response
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No implementation needed for this example
        }
    }
}