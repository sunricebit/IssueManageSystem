using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IMS.Common
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly PermissionDAO _permissionDAO;

        //public CustomAuthorizeAttribute(params string[] requiredRole)
        //{
        //    _requiredRoles = requiredRole;
        //}

        public CustomAuthorizeAttribute(PermissionDAO permissionDAO)
        {
            _permissionDAO = permissionDAO;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //User? user = context.HttpContext.Session.GetUser();
            //if (user == null)
            //{
            //    context.Result = new RedirectToRouteResult("Auth", "SignIn", null);
            //    return;
            //}

            //if (_requiredRoles.Length != 0 &&!_requiredRoles.Contains(user!.Role.Value))
            //{
            //    context.Result = new RedirectToActionResult("NotAccess", "Error", null);
            //    return;
            //}
            //base.OnActionExecuting(context);

            User? user = context.HttpContext.Session.GetUser();
            if (user == null)
            {
                context.Result = new RedirectToRouteResult("Auth", "SignIn", null);
                return;
            }

            var controller = context.Controller as Controller;
            if (controller != null)
            {
                var routeValues = context.RouteData.Values;

                // Kiểm tra đường dẫn
                string pageLink = "/" + routeValues["controller"]?.ToString() + "/" + routeValues["action"]?.ToString();

                // Check đường dẫn trong permission
                bool isPageExist = _permissionDAO.IsExist(pageLink);
                if (isPageExist)
                {
                    // Nếu chưa có thì add và cấp quyền cho admin, các user khác unable
                    _permissionDAO.CreateNewPermission(pageLink);
                }

                // Có rồi thì check quyền
                if (!_permissionDAO.CheckPermission(pageLink, user.Role.Value))
                {
                    context.Result = new RedirectToRouteResult("Auth", "SignIn", null);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
