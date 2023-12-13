using IMS.ViewModels.Permission;

namespace IMS.Services
{
    public class ChkPgAcessService : IChkPgAcessService
    {
        public PageAccess GetPageAccess(HttpContext context)
        {
            var controllerName = context.GetRouteData().Values["controller"] as string;
            var actionName = context.GetRouteData().Values["action"] as string;
            string page = "/" + controllerName + "/" + actionName;
            var permission = context.Session.GetPermission();
            var pageAccess = permission.PagesAcess.FirstOrDefault(item => item.Page == page);
            return pageAccess;
        }
    }
}
