using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Payments.Bkash.Infrastructure
{
    public partial class RouteProvider : IRouteProvider
    {
        public int Priority => -1;

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Plugin.Payments.Bkash.BkashCheckout", "Plugins/Bkash/BkashCheckout",
                 new { controller = "PaymentBkash", action = "BkashCheckout" });

            routeBuilder.MapRoute("Plugin.Payments.Bkash.BkashCreate", "Plugins/Bkash/BkashCreate",
                 new { controller = "PaymentBkash", action = "BkashCreate" });

            routeBuilder.MapRoute("Plugin.Payments.Bkash.BkashExecute", "Plugins/Bkash/BkashExecute",
                 new { controller = "PaymentBkash", action = "BkashExecute" });

            routeBuilder.MapRoute("Plugin.Payments.Bkash.BkashError", "Plugins/Bkash/BkashError",
                 new { controller = "PaymentBkash", action = "BkashError" });
        }
    }
}
