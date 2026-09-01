using Ecommerce.Shared.CommonResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiBaseController : ControllerBase
    {
        // Handle result without value
        // if result is success return no content 204
        // if result is failure return problem details with its status code

        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
            {
                return NoContent(); // 204
            }
            else
            {
                return HandleProblem(result.Errors);
            }
        }

        protected ActionResult<T> HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value); // 200
            }
            else
            {
                return HandleProblem(result.Errors);
            }
        }

        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            // if no error is provided, return 500 internal server error
            if (errors == null || errors.Count == 0)
            {
                return Problem(detail: "An unexpected error occurred.", statusCode: 500);
            }

            // handle validation errors
            if (errors.All(e => e.ErrorType == ErrorType.Validation))
            {
                return HandleValidationProblem(errors);
            }

            // if there is only one error, handle it as single error problem
            return Problem(
                title: errors[0].Code,
                detail: errors[0].Description,
                statusCode: GetStatusCode(errors[0].ErrorType),
                type: errors[0].ErrorType.ToString()
                );

        }

        private int GetStatusCode(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Failure => 500,
                ErrorType.Forbidden => 403,
                ErrorType.InvalidCredintials => 401,
                ErrorType.NotFound => 404,
                ErrorType.Unauthorized => 401,
                ErrorType.Validation => 400,
                _ => 500
            };
        }

        private ActionResult HandleValidationProblem(IReadOnlyList<Error> errors)
        {
            var modelState = new ModelStateDictionary();
            foreach (var error in errors)
            {
                modelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(modelState);
        }
    }
}
