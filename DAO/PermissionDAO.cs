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

        public bool CheckPermission(string page, string role)
        {
            Setting tempPage = _context.Settings.FirstOrDefault(s => s.Value == page);
            Setting tempRole = _context.Settings.FirstOrDefault(s => s.Value == role);
            return _context.Permissions.FirstOrDefault(p => p.RoleId == tempRole.Id
                                                                  && p.PageId == tempPage.Id).CanRead;
        }

        public void CreateNewPermission(string page)
        {
            List<Setting> roles = _context.Settings.Where(s => s.Type == "ROLE").ToList();
            Setting setting = new Setting {
                Type = "PAGE_LINK",
                Value = page,
            };
            _context.Settings.Add(setting);
            _context.SaveChanges();
            int pageId = _context.Settings.FirstOrDefault(item => item.Value == setting.Value && item.Type == setting.Type).Id;

            foreach(var role in roles)
            {
                Permission p = new Permission
                {
                    RoleId = role.Id,
                    PageId = pageId,
                    CanRead = role.Value.Equals(RoleUser.Admin) ? true : false,
                };
                _context.Permissions.Add(p);
            }
            _context.SaveChanges();
        }

        public bool IsExist(string page)
        {
            Setting tempPage = _context.Settings.FirstOrDefault(s => s.Value == page);
            if (tempPage == null)
            {
                return false;
            }
            return true;
        }
    }
}
