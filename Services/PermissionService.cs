using IMS.ViewModels.Permission;

namespace IMS.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IMSContext _context;

        public PermissionService(IMSContext context)
        {
            _context = context;
        }

        public bool CheckAccess(string page, string role)
        {
            Setting tempPage = _context.Settings.FirstOrDefault(s => s.Value == page);
            Setting tempRole = _context.Settings.FirstOrDefault(s => s.Value == role);
            return _context.Permissions.FirstOrDefault(p => p.RoleId == tempRole.Id
                                                                  && p.PageId == tempPage.Id).CanRead;
        }

        public void CreateNewPermission(string page)
        {
            List<Setting> roles = _context.Settings.Where(s => s.Type == "ROLE").ToList();
            Setting setting = new Setting
            {
                Type = "PAGE_LINK",
                Value = page,
            };
            _context.Settings.Add(setting);
            _context.SaveChanges();
            int pageId = _context.Settings.FirstOrDefault(item => item.Value == setting.Value && item.Type == setting.Type).Id;

            foreach (var role in roles)
            {
                Permission p = new Permission
                {
                    RoleId = role.Id,
                    PageId = pageId,
                    CanRead = true,
                    CanUpdate = role.Value.Equals(RoleUser.Admin) ? true : false,
                    CanCreate = role.Value.Equals(RoleUser.Admin) ? true : false,
                    CanExport = role.Value.Equals(RoleUser.Admin) ? true : false,
                    CanDelete = role.Value.Equals(RoleUser.Admin) ? true : false,
                };
                _context.Permissions.Add(p);
            }
            _context.SaveChanges();
        }

        public PermissionViewModel GetPermissionViewModel(int roleId)
        {
            Setting setting = _context.Settings.FirstOrDefault(s => s.Id == roleId);
            var query = _context.Permissions.Include(x => x.Page).Include(x => x.Role)
                .Where(x => x.RoleId == roleId).ToList();
            PermissionViewModel permissionVM = query
            .GroupBy(p => p.RoleId)
            .Select(group => new PermissionViewModel
            {
                Role = group.First().Role.Value,
                PagesAcess = group.Select(p => new PageAccess
                {
                    Page = p.Page.Value,
                    CanAccess = p.CanRead,
                    CanAdd = p.CanCreate,
                    CanUpdate = p.CanUpdate,
                    CanDelete = p.CanDelete,
                    CanExport = p.CanExport,
                }).ToList()
            })
            .First();
            return permissionVM;
        }

        public bool IsExist(string page)
        {
            Setting tempPage = _context.Settings.FirstOrDefault(s => s.Value.ToLower().Equals(page.ToLower()));
            if (tempPage == null)
            {
                return false;
            }
            return true;
        }
    }
}
