using DocumentFormat.OpenXml.Drawing.Spreadsheet;
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
                }).OrderBy(p => p.Page).ToList()
            })
            .OrderBy(p => p.Role).ToList();

            return permissionViewModels;
        }

        public List<PermissionViewModel> GetPermissionByKeyword(string page)
        {
            List<PermissionViewModel> permissionViewModels = new List<PermissionViewModel>();

            var query = _context.Permissions.Include(x => x.Page).Include(x => x.Role).ToList();
            permissionViewModels = query
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
                }).Where(p => p.Page.ToLower().Contains(page.ToLower())).OrderBy(p => p.Page).ToList()
            })
            .OrderBy(p => p.Role).ToList();

            return permissionViewModels;
        }

        public void UpdatePermission(List<PermissionViewModel> permissionViewModels)
        {
            foreach(var permissionViewModel in permissionViewModels)
            {
                foreach(var pagesAccess in permissionViewModel.PagesAcess)
                {
                    Setting role = _context.Settings.FirstOrDefault(s => s.Value == permissionViewModel.Role);
                    Setting page = _context.Settings.FirstOrDefault(s => s.Value == pagesAccess.Page);
                    Permission permission = _context.Permissions.FirstOrDefault(p => p.RoleId == role.Id 
                                                                          && p.PageId == page.Id);

                    permission.CanRead = pagesAccess.CanAccess;
                    permission.CanCreate = pagesAccess.CanAdd;
                    permission.CanUpdate = pagesAccess.CanUpdate;
                    permission.CanExport = pagesAccess.CanExport;
                    permission.CanDelete = pagesAccess.CanDelete;
                    _context.SaveChanges();
                }
            }
        }
    }
}
