namespace IMS.ViewModels.Permission
{
    public class PermissionViewModel
    {
        public string Page { get; set; }
        public Dictionary<string, bool> RolesAcess { get; set; }
    }
}
