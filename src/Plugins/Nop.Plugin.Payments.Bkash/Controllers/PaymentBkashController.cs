using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Payments.Bkash.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Payments.Bkash.Controllers
{
    public class PaymentBkashController : BasePaymentController
    {
        #region Fields
        
        private readonly IPermissionService _permissionService;
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region ctor

        public PaymentBkashController(IPermissionService permissionService,
            IStoreContext storeContext,
            ISettingService settingService,
            INotificationService notificationService,
            ILocalizationService localizationService)
        {
            _permissionService = permissionService;
            _storeContext = storeContext;
            _settingService = settingService;
            _notificationService = notificationService;
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var bkashPaymentSettings = _settingService.LoadSetting<BkashPaymentSettings>(storeScope);

            var model = new ConfigurationModel
            {
                AppKey = bkashPaymentSettings.AppKey,
                AppSecret = bkashPaymentSettings.AppSecret,
                Username = bkashPaymentSettings.Username,
                Password = bkashPaymentSettings.Password,
                TestAppKey = bkashPaymentSettings.AppKey,
                TestAppSecret = bkashPaymentSettings.TestAppSecret,
                TestUsername = bkashPaymentSettings.TestUsername,
                TestPassword = bkashPaymentSettings.TestPassword,
                UseSandbox = bkashPaymentSettings.UseSandbox,
                SandBoxUrl = bkashPaymentSettings.SandBoxUrl,
                LiveUrl = bkashPaymentSettings.LiveUrl,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope <= 0)
                return View("~/Plugins/Payments.Bkash/Views/Configure.cshtml", model);

            model.AppKey_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.AppKey, storeScope);
            model.AppSecret_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.AppSecret, storeScope);
            model.Username_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.Username, storeScope);
            model.Password_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.Password, storeScope);
            model.TestAppKey_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.TestAppKey, storeScope);
            model.TestAppSecret_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.TestAppSecret, storeScope);
            model.TestUsername_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.TestUsername, storeScope);
            model.TestPassword_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.TestPassword, storeScope);
            model.UseSandbox_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.UseSandbox, storeScope);
            model.SandBoxUrl_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.SandBoxUrl, storeScope);
            model.LiveUrl_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.LiveUrl, storeScope);

            return View("~/Plugins/Payments.Bkash/Views/Configure.cshtml", model);
        }


        [HttpPost]
        [AuthorizeAdmin]
        [AdminAntiForgery]
        [Area(AreaNames.Admin)]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Configure();


            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var bkashPaymentSettings = _settingService.LoadSetting<BkashPaymentSettings>(storeScope);

            //Save settings
            bkashPaymentSettings.AppKey = model.AppKey;
            bkashPaymentSettings.AppSecret = model.AppSecret;
            bkashPaymentSettings.Username = model.Username;
            bkashPaymentSettings.Password = model.Password;
            bkashPaymentSettings.TestAppKey = model.TestAppKey;
            bkashPaymentSettings.TestAppSecret = model.TestAppSecret;
            bkashPaymentSettings.TestUsername = model.TestUsername;
            bkashPaymentSettings.TestPassword = model.TestPassword;
            bkashPaymentSettings.UseSandbox = model.UseSandbox;
            bkashPaymentSettings.SandBoxUrl = model.SandBoxUrl;
            bkashPaymentSettings.LiveUrl = model.LiveUrl;


            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.AppKey, model.AppKey_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.AppSecret, model.AppSecret_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.Username, model.Username_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.Password, model.Password_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.TestAppKey, model.TestAppKey_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.TestAppSecret, model.TestAppSecret_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.TestUsername, model.TestUsername_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.TestPassword, model.TestPassword_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.UseSandbox, model.UseSandbox_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.SandBoxUrl, model.SandBoxUrl_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.LiveUrl, model.LiveUrl_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }
        
        #endregion
    }
}
