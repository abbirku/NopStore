using System;
using System.Collections.Generic;
using System.Text;
using A4A.BKASH.Configuration;
using Nop.Services.Configuration;

namespace Nop.Plugin.Payments.Bkash.Utilities
{
    public class BkashProvider : IBkashCheckoutConfigProvider
    {
        private readonly BkashPaymentSettings _bkashPaymentSettings;

        public BkashProvider(BkashPaymentSettings bkashPaymentSettings)
        {
            _bkashPaymentSettings = bkashPaymentSettings;
        }

        public bool IsLive
        {
            get
            {
                bool isLive;
                isLive = !_bkashPaymentSettings.UseSandbox;
                return isLive;
            }
        }

        public string AppKey
        {
            get
            {
                string str;
                str = _bkashPaymentSettings.UseSandbox ? _bkashPaymentSettings.TestAppKey : _bkashPaymentSettings.AppKey;
                return str;
            }
        }

        public string AppSecret
        {
            get
            {
                string str;
                str = _bkashPaymentSettings.UseSandbox ? _bkashPaymentSettings.TestAppSecret : _bkashPaymentSettings.AppSecret;
                return str;
            }
        }

        public string UserName
        {
            get
            {
                string str;
                str = _bkashPaymentSettings.UseSandbox ? _bkashPaymentSettings.TestUsername : _bkashPaymentSettings.Username;
                return str;
            }
        }

        public string Password
        {
            get
            {
                string str;
                str = _bkashPaymentSettings.UseSandbox ? _bkashPaymentSettings.TestPassword : _bkashPaymentSettings.Password;
                return str;
            }
        }
    }
}
