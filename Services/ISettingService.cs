namespace IMS.Services
{
    public interface ISettingService
    {
        public IEnumerable<Setting> GetSettings();
        public Setting GetSettingById(int id);
        public IEnumerable<Setting> Search(string value);
        public void SetSetting(Setting setting);
        public bool CheckSettingExist(Setting setting);
        public void AddSetting(Setting setting);
        public IEnumerable<Setting> GetSettingPaginate(int pageNumber, int pageSize);
    }
}
