using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Nop.Plugin.Payments.Bkash.Models
{
    public class Message
    {
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
    }
}
