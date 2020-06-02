using System;
using System.Collections.Generic;
using System.Security.Policy;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BkashPaymentSettings _bkashPaymentSettings;

        public BkashPaymentProcess(IWebHelper webHelper,
            ILocalizationService localizationService,
            ISettingService settingService,
            IHttpContextAccessor httpContextAccessor,
            BkashPaymentSettings bkashPaymentSettings)
        {
            _webHelper = webHelper;
            _localizationService = localizationService;
            _settingService = settingService;
            _httpContextAccessor = httpContextAccessor;
            _bkashPaymentSettings = bkashPaymentSettings;
        }


        public bool SupportCapture => false;//Don't need
        public bool SupportPartiallyRefund => false; //Don't need
        public bool SupportRefund => false; //Don't need
        public bool SupportVoid => false;//Don't need
        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;//Don't need
        public PaymentMethodType PaymentMethodType => PaymentMethodType.Redirection;
        public bool SkipPaymentInfo => true;
        public string PaymentMethodDescription
        {
            //return description of this payment method to be display on "payment method" checkout step. good practice is to make it localizable
            //for example, for a redirection payment method, description may be like this: "You will be redirected to PayPal site to complete the payment"
            get { return _localizationService.GetResource("Plugins.Payments.Bkash.PaymentMethodDescription"); }
        }

        //Don't need
        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            return new CancelRecurringPaymentResult { Errors = new[] { "Recurring payment not supported" } };
        }

        public bool CanRePostProcessPayment(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return true;
        }

        //Don't need
        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            return new CapturePaymentResult { Errors = new[] { "Capture method not supported" } };
        }

        //Don't need
        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            return 0;
        }

        //Don't need
        public ProcessPaymentRequest GetPaymentInfo(IFormCollection form)
        {
            return new ProcessPaymentRequest();
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/PaymentBkash/Configure";
        }

        public string GetPublicViewComponentName()
        {
            return "PaymentBkash";
        }

        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            return false;
        }

        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            var baseUrl = _bkashPaymentSettings.BaseUrl;
            var orderNumber = postProcessPaymentRequest.Order.Id.ToString();
            var orderTotal = postProcessPaymentRequest.Order.OrderTotal;

            var url = $"{baseUrl}Plugins/Bkash/BkashCheckout?orderNumber={orderNumber}&orderTotal={orderTotal}";
            _httpContextAccessor.HttpContext.Response.Redirect(url);
        }

        //Don't need
        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            return new ProcessPaymentResult();
        }

        //Don't need
        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            return new ProcessPaymentResult { Errors = new[] { "Recurring payment not supported" } };
        }

        //Don't need
        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            return new RefundPaymentResult { Errors = new[] { "Recurring payment not supported" } };
        }

        //Don't need
        public IList<string> ValidatePaymentForm(IFormCollection form)
        {
            return new List<string>();
        }

        //Don't need
        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            return new VoidPaymentResult { Errors = new[] { "Void method not supported" } };
        }

        // Install the plugin
        public override void Install()
        {
            //settings
            _settingService.SaveSetting(new BkashPaymentSettings
            {
                UseSandbox = true,
                DoesCreateSuccessfulPayment = true
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

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.BaseUrl", "Base Url");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.BaseUrl.Hint", "Enter base url");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.UseSandbox", "Use Sandbox");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.UseSandbox.Hint", "Check to enable Sandbox (testing environment).");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.DoesCreateSuccessfulPayment", "Does Create Successful Payment");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.Fields.DoesCreateSuccessfulPayment.Hint", "Check to enable successful payment.");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.Bkash.PaymentMethodDescription", "Pay with bkash");

            base.Install();
        }

        // Uninstall the plugin
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
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.Bkash.Fields.BaseUrl");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.Bkash.Fields.UseSandbox");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.Bkash.Fields.DoesCreateSuccessfulPayment");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.Bkash.PaymentMethodDescription");

            base.Uninstall();
        }
    }
}
