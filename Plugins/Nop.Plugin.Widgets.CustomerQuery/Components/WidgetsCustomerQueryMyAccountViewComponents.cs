using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.CustomerQuery.Components;
public class WidgetsCustomerQueryMyAccountViewComponents : NopViewComponent
{    
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData = null)
    {
        return View("~/Plugins/Widgets.CustomerQuery/Views/MyAccount.cshtml");
    }
}
