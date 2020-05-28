using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Payments;
using Nop.Services.Plugins;

namespace Nop.Plugin.Payments.Bkash
{
    public class BkashPaymentProcess : BasePlugin, IPaymentMethod
    {

        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;

        public BkashPaymentProcess(IWebHelper webHelper,
            ILocalizationService localizationService,
            ISettingService settingService)
        {
            _webHelper = webHelper;
            _localizationService = localizationService;
            _settingService = settingService;
        }


        public bool SupportCapture => false;

        public bool SupportPartiallyRefund => false;

        public bool SupportRefund => false;

        public bool SupportVoid => false;

        public RecurringPaymentType RecurringPaymentType => throw new NotImplementedException();

        public PaymentMethodType PaymentMethodType => throw new NotImplementedException();

        public bool SkipPaymentInfo => throw new NotImplementedException();

        public string PaymentMethodDescription => throw new NotImplementedException();

        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public bool CanRePostProcessPayment(Order order)
        {
            throw new NotImplementedException();
        }

        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            throw new NotImplementedException();
        }

        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            throw new NotImplementedException();
        }

        public ProcessPaymentRequest GetPaymentInfo(IFormCollection form)
        {
            throw new NotImplementedException();
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/PaymentBkash/Configure";
        }

        public string GetPublicViewComponentName()
        {
            throw new NotImplementedException();
        }

        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            throw new NotImplementedException();
        }

        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public IList<string> ValidatePaymentForm(IFormCollection form)
        {
            throw new NotImplementedException();
        }

        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        public override void Install()
        {
            //settings
            _settingService.SaveSetting(new BkashPaymentSettings
            {
                UseSandbox = true
            });

            //locales
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.AppKey", "App key");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.AppKey.Hint", "Enter app key");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.AppSecret", "App Secret");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.AppSecret.Hint", "Enter app secret");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.Username", "Username");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.Username.Hint", "Enter Username");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.Password", "Password");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.Password.Hint", "Enter password");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.TestAppKey", "Test app key");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.TestAppKey.Hint", "Enter test app key");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.TestAppSecret", "Test app secret");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.TestAppSecret.Hint", "Enter test app secret");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.TestUsername", "Test Username");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.TestUsername.Hint", "Enter test Username");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.TestPassword", "Test Password");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.TestPassword.Hint", "Enter test Password");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.UseSandbox", "Use Sandbox");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.UseSandbox.Hint", "Check to enable Sandbox (testing environment).");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.SandBoxUrl", "SandBoxUrl");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.SandBoxUrl.Hint", "Enter SandBox Url");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.LiveUrl", "LiveUrl");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.LiveUrl.Hint", "Enter Live Url");


            base.Install();
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<BkashPaymentSettings>();

            //locales
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.Bkash.Fields.AppKey");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.Bkash.Fields.AppSecret");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.Bkash.Fields.Username");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.Bkash.Fields.Password");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.Bkash.Fields.TestAppKey");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.Bkash.Fields.TestAppSecret");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.Bkash.Fields.TestUsername");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.Bkash.Fields.TestPassword");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.Bkash.Fields.UseSandbox");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.Bkash.Fields.SandBoxUrl");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.Bkash.Fields.LiveUrl");

            base.Uninstall();
        }
    }
}
