using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.CustomerQuery.Components;
public class WidgetsCustomerQueryViewComponent: NopViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData = null)
    {
        // You can add logic here to handle the widget rendering based on the widgetZone or additionalData
        return View("~/Plugins/Widgets.CustomerQuery/Views/CustomerQuerey/Index.cshtml");
    }

}
