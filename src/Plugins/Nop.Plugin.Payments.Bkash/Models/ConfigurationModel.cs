using System;
using System.Collections.Generic;
using System.Text;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Payments.Bkash.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Bkash.Fields.AppKey")]
        public string AppKey { get; set; }
        public bool AppKey_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Bkash.Fields.AppSecret")]
        public string AppSecret { get; set; }
        public bool AppSecret_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Bkash.Fields.Username")]
        public string Username { get; set; }
        public bool Username_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Bkash.Fields.Password")]
        public string Password { get; set; }
        public bool Password_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Bkash.Fields.TestAppKey")]
        public string TestAppKey { get; set; }
        public bool TestAppKey_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Bkash.Fields.TestAppSecret")]
        public string TestAppSecret { get; set; }
        public bool TestAppSecret_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Bkash.Fields.TestUsername")]
        public string TestUsername { get; set; }
        public bool TestUsername_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Bkash.Fields.TestPassword")]
        public string TestPassword { get; set; }
        public bool TestPassword_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Bkash.Fields.BaseUrl")]
        public string BaseUrl { get; set; }
        public bool BaseUrl_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Bkash.Fields.UseSandbox")]
        public bool UseSandbox { get; set; }
        public bool UseSandbox_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Bkash.Fields.DoesCreateSuccessfulPayment")]
        public bool DoesCreateSuccessfulPayment { get; set; }
        public bool DoesCreateSuccessfulPayment_OverrideForStore { get; set; }
    }
}
