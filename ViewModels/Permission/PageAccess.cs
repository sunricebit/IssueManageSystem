namespace IMS.ViewModels.Permission
{
    public class PageAccess
    {
        public string Page { get; set; }
        public bool CanAccess { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanAdd { get; set; }
        public bool CanExport { get; set; }
        public bool CanDelete { get; set; }
    }
}
