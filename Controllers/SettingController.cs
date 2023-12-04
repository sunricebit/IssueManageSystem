﻿using IMS.ViewModels.Validation;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    [Route("Setting")]
    public class SettingController : Controller
    {
        private readonly SettingDAO _settingDAO;

        public SettingController(SettingDAO settingDAO)
        {
            _settingDAO = settingDAO;
        }

        [Route("List")]
        public IActionResult SettingList(int? pageNumber)
        {
            int tempPageNumber = pageNumber ?? 1;
            int tempPageSize = 10;
            Paginate<Setting> paginate = new Paginate<Setting>(tempPageNumber, tempPageSize); 
            ViewBag.SettingList = paginate.GetListPaginate<Setting>();
            ViewBag.Action = "SettingList";
            ViewBag.Pagination = paginate.GetPagination();
            return View();
        }

        [Route("Add")]
        public IActionResult AddSetting() {

            return View();
        }


        [Route("Add"), HttpPost]
        public IActionResult AddSetting(SettingViewModel? settingView)
        {
            if (settingView == null)
            {
                return View();
            }

            if (!ModelState.IsValid) {
                return View(); 
            }
            Setting setting = new Setting() { 
                Type = settingView.Type,
                Value = settingView.Value,
            };
            
            if (_settingDAO.CheckSettingExist(setting))
            {
                ViewBag.ErrorMessage = "Setting List already has a setting with type: " + setting.Type + " and value: " + setting.Value;
                return View();
            }

            _settingDAO.AddSetting(setting);
            ViewBag.SuccessMessage = "Add setting success!";
            return View();
        }

        [Route("Details")]
        public IActionResult SettingDetail(int id)
        {
            Setting setting = _settingDAO.GetSettingById(id);
            SettingViewModel settingView = new SettingViewModel()
            {
                Id = id,
                Type = setting.Type,
                Value = string.IsNullOrEmpty(setting.Value) ? String.Empty : setting.Value.Trim(),
                Description = string.IsNullOrEmpty(setting.Description) ? String.Empty : setting.Description.Trim()
            };
            return View(settingView);
        }

        [Route("Update"), HttpPost]
        public IActionResult SettingUpdate(SettingViewModel? settingView)
        {
            if (settingView == null)
            {
                return View("SettingDetail");
            }

            if (!ModelState.IsValid) { return View("SettingDetail", settingView); }

            Setting setting = _settingDAO.GetSettingById(settingView.Id);
            if (setting.Type == settingView.Type 
                && setting.Value == (string.IsNullOrEmpty(settingView.Value) ? String.Empty : settingView.Value.Trim())
                && setting.Description == (string.IsNullOrEmpty(settingView.Description) ? String.Empty : settingView.Description.Trim()))
            {
                ViewBag.ErrorMessage = "Nothing change";
                return View("SettingDetail", settingView);
            }

            setting.Type = settingView.Type;
            setting.Value = string.IsNullOrEmpty(settingView.Value) ? String.Empty : settingView.Value.Trim();
            setting.Description = string.IsNullOrEmpty(settingView.Description) ? String.Empty : settingView.Description.Trim();

            if (!_settingDAO.CheckSettingCanUpdate(setting))
            {
                ViewBag.ErrorMessage = "Setting List already has a setting with type: " + setting.Type + " and value: " + setting.Value;
                return View("SettingDetail", settingView);
            }

            _settingDAO.SetSetting(setting);
            ViewBag.SuccessMessage = "Update setting success!";
            return View("SettingDetail", settingView);
        }
    }
}
