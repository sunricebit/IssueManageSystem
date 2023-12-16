using IMS.ViewModels.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

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
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Auth",
                    action = "SignIn"
                }));
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
                    // Nếu chưa có thì add và cấp quyền cho admin, các user khác sẽ có quyền read
                    permissionService.CreateNewPermission(pageLink);
                }

                PermissionViewModel permissionVM = permissionService.GetPermissionViewModel(user.RoleId);
                string permissionString = JsonSerializer.Serialize(permissionVM);
                context.HttpContext.Session.SetString("Permission", permissionString);

                // Có rồi thì check quyền Access -> các quyền khác ngoài read cập nhật trong màn hình permission
                if (!permissionService.CheckAccess(pageLink, user.Role.Value))
                {
                    context.Result = new RedirectToActionResult("NotAccess", "Error", null);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
