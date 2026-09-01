using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.CommonResult
{
    public class Error
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public ErrorType ErrorType { get; set; }
        private Error(string code, string description, ErrorType errorType)
        {
            Code = code;
            Description = description;
            ErrorType = errorType;
        }

        // static factory method to create an instance of Error class
        public static Error Failure(string code="General.Failure", string description = "General.Failure has occured")
        {
            return new Error(code, description, ErrorType.Failure);
        }

        public static Error Forbidden(string code = "General.Forbidden", string description = "General.Forbidden has occured")
        {
            return new Error(code, description, ErrorType.Forbidden);
        }

        public static Error InvalidCredintials(string code = "General.InvalidCredintials", string description = "General.InvalidCredintials has occured")
        {
            return new Error(code, description, ErrorType.InvalidCredintials);
        }

        public static Error NotFound(string code = "General.NotFound", string description = "General.NotFound has occured")
        {
            return new Error(code, description, ErrorType.NotFound);
        }

        public static Error Unauthorized(string code = "General.Unauthorized", string description = "General.Unauthorized has occured")
        {
            return new Error(code, description, ErrorType.Unauthorized);
        }

        public static Error Validation(string code = "General.Validation", string description = "General.Validation has occured")
        {
            return new Error(code, description, ErrorType.Validation);
        }
    }
}
