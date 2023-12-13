using IMS.ViewModels.Permission;

namespace IMS.Services
{
    public interface IChkPgAcessService
    {
        public PageAccess GetPageAccess(HttpContext context);
    }
}
