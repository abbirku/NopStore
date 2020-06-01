using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Payments.Bkash.Models
{
    public class PaymentModel
    {
        public bool UseSandBox { get; set; }
        public string Currency { get; set; }
        public decimal OrderTotal { get; set; }
        public string OrderNumber { get; set; }
        public string ReturnUrl { get; set; }
        public string SuccessUrl { get; set; }
        public string CreateUrl { get; set; }
        public string ExecuteUrl { get; set; }
    }
}
