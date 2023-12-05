﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IMS.Common
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        //private readonly IPermissionService permissionService;

        //public CustomAuthorizeAttribute(params string[] requiredRole)
        //{
        //    _requiredRoles = requiredRole;
        //}

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

            var permissionService = (IPermissionService)context.HttpContext.RequestServices.GetService(typeof(IPermissionService));

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
                bool isPageExist = permissionService.IsExist(pageLink);
                if (!isPageExist)
                {
                    // Nếu chưa có thì add và cấp quyền cho admin, các user khác unable
                    permissionService.CreateNewPermission(pageLink);
                }

                // Có rồi thì check quyền
                if (!permissionService.CheckPermission(pageLink, user.Role.Value))
                {
                    context.Result = new RedirectToActionResult("NotAccess", "Error", null);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
