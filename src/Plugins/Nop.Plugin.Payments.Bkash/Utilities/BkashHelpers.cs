using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Nop.Plugin.Payments.Bkash.Utilities
{
    public static class BkashHelpers
    {
        public static string AbsoluteAction(this IUrlHelper url,string actionName,string controllerName,object routeValues = null)
        {
            string scheme = url.ActionContext.HttpContext.Request.Scheme;
            return url.Action(actionName, controllerName, routeValues, scheme);
        }
    }
}
