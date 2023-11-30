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
            User? user = context.HttpContext.Session.GetUser();
            if (user == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Auth",
                    action = "SignIn"
                }));
                return;
            }

            if (_requiredRoles.Length != 0 &&!_requiredRoles.Contains(user!.Role.Value))
            {
                context.Result = new RedirectToActionResult("NotAccess", "Error", null);
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
