using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Payments.Bkash.Components
{
    [ViewComponent(Name = "PaymentBkash")]
    public class PaymentBkashViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Plugins/Payments.Bkash/Views/PaymentInfo.cshtml");
        }
    }
}
