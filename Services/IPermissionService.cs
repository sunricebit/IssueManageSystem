namespace IMS.Services
{
    public interface IPermissionService
    {
        public bool CheckPermission(string page, string role);
        public void CreateNewPermission(string page);
        public bool IsExist(string page);
    }
}
