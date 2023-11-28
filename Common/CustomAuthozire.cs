using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IMS.Common
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string[] _requiredRoles;

        public CustomAuthorizeAttribute(params string[] requiredRole)
        {
            _requiredRoles = requiredRole;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("role"); 

            if (_requiredRoles.Contains(role))
            {
                base.OnActionExecuting(context);
            }
            else
            {
                context.Result = new RedirectToActionResult("NotAccess", "Error", null);
            }
        }
    }
}