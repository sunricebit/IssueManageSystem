using IMS.ViewModels.Permission;
using System.IO;

namespace IMS.DAO
{
    public class PermissionDAO
    {

        private readonly IMSContext _context;

        public PermissionDAO(IMSContext context)
        {
            _context = context;
        }

        public List<PermissionViewModel> GetAllPermission()
        {
            List<PermissionViewModel> permissionViewModels = new List<PermissionViewModel>();

            var query = _context.Permissions.Include(x => x.Page).Include(x => x.Role).ToList();
            permissionViewModels = query
            .GroupBy(p => p.PageId)
            .Select(group => new PermissionViewModel
            {
                Page = group.First().Page.Value,
                RolesAcess = group.ToDictionary(p => p.Role.Value, p => p.CanRead)
            })
            .ToList();

            return permissionViewModels;
        }

        public void UpdatePermission(List<PermissionViewModel> permissionViewModels)
        {
            foreach(var permissionViewModel in permissionViewModels)
            {
                foreach(var roleAccess in permissionViewModel.RolesAcess)
                {
                    Setting page = _context.Settings.FirstOrDefault(s => s.Value == permissionViewModel.Page);
                    Setting role = _context.Settings.FirstOrDefault(s => s.Value == roleAccess.Key);
                    Permission permission = _context.Permissions.FirstOrDefault(p => p.RoleId == role.Id 
                                                                          && p.PageId == page.Id);

                    permission.CanRead = roleAccess.Value;
                    _context.SaveChanges();
                }
            }
        }
    }
}
