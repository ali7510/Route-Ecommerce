using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Factory
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationResponse(ActionContext actionContext)
        {
            var Errors = actionContext.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(ActionContext => ActionContext.Key, ActionContext => ActionContext.Value.Errors.Select(x => x.ErrorMessage).ToArray());
            var problemDetails = new ProblemDetails()
            {
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Instance = actionContext.HttpContext.Request.Path,
                Extensions = { ["errors"] = Errors }
            };
            return new BadRequestObjectResult(problemDetails);
        }
    }
}
