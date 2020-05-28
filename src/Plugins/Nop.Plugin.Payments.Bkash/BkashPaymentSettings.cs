using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Configuration;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Nop.Plugin.Payments.Bkash
{
    public class BkashPaymentSettings : ISettings
    {
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TestAppKey { get; set; }
        public string TestAppSecret { get; set; }
        public string TestUsername { get; set; }
        public string TestPassword { get; set; }
        public bool UseSandbox { get; set; }
        public string SandBoxUrl { get; set; }
        public string LiveUrl { get; set; }
    }
}
