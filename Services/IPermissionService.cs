using IMS.ViewModels.Permission;

namespace IMS.Services
{
    public interface IPermissionService
    {
        public bool CheckAccess(string page, string role);
        public void CreateNewPermission(string page);
        public bool IsExist(string page);
        public PermissionViewModel GetPermissionViewModel(int roleId);
    }
}
