using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreTools.Mvc
{
    /// <summary>
    /// Represents a model validation filter.
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateModelAttribute"/> class.
        /// </summary>
        public ValidateModelAttribute()
        {
        }

        /// <summary>
        /// Checks whether the specified <paramref name="context"/>'s model state is valid.
        /// </summary>
        /// <param name="context">The action executing context.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
