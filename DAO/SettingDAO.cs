﻿namespace IMS.DAO
{
    public class SettingDAO
    {

        private readonly IMSContext _context;

        public SettingDAO(IMSContext context)
        {
            _context = context;
        }

        public void AddSetting(Setting setting)
        {
            try
            {
                _context.Settings.Add(setting);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool CheckSettingExist(Setting setting)
        {
            try
            {
                Setting? checkSetting = _context.Settings.FirstOrDefault(s => setting.Type.Equals(s.Type)
                                                                                && setting.Value.Equals(s.Value));
                if (checkSetting == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool CheckSettingCanUpdate(Setting setting)
        {
            try
            {
                Setting? checkSetting = _context.Settings.FirstOrDefault(s => setting.Type.Equals(s.Type)
                                                                                && setting.Value.Equals(s.Value));
                if (checkSetting == null)
                {
                    return true;
                }
                if (checkSetting.Id == setting.Id)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Setting GetSettingById(int id)
        {
            try
            {
                Setting setting = _context.Settings.FirstOrDefault(s => s.Id == id);
                return setting;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Setting> GetSettings()
        {
            try
            {
                return _context.Settings;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Setting> Search(string value)
        {
            try
            {
                IEnumerable<Setting> settings = _context.Settings.Where(s => s.Value.Contains(value));
                return settings;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void SetSetting(Setting setting)
        {
            try
            {
                Setting tempSetting = _context.Settings.FirstOrDefault(s => s.Id == setting.Id);
                if (tempSetting != null)
                {
                    tempSetting.Value = setting.Value;
                    tempSetting.Type = setting.Type;
                    tempSetting.Status = setting.Status;
                    tempSetting.Order = setting.Order;
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
