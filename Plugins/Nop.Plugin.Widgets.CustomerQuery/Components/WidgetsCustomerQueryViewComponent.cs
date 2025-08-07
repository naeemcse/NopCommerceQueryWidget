using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.CustomerQuery.Domain;
using Nop.Plugin.Widgets.CustomerQuery.Models;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.CustomerQuery.Components;
public class WidgetsCustomerQueryViewComponent: NopViewComponent
{

    private readonly CustomerQuerySettings _customerQuerySettings;

    public WidgetsCustomerQueryViewComponent(CustomerQuerySettings customerQuerySettings)
    {
        _customerQuerySettings = customerQuerySettings;
    }
    
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData = null)
    {
        if (!_customerQuerySettings.Enabled)
            return Content("");

        // Use traditional if-else for string comparison
        string displayType;
        if (widgetZone == "header_menu_after")
            displayType = "navigation";
        else if (widgetZone == "footer")
            displayType = "footer";
        else if (widgetZone == "account_navigation_after")
            displayType = "profile";
        else
            displayType = string.Empty;

        if (string.IsNullOrEmpty(displayType))
            return Content("");

        // Check if display is enabled for the current zone
        bool shouldDisplay = displayType switch
        {
            "navigation" => _customerQuerySettings.DisplayInNavigation,
            "footer" => _customerQuerySettings.DisplayInFooter,
            "profile" => _customerQuerySettings.DisplayInCustomerProfile,
            _ => false
        };

        if (!shouldDisplay)
            return Content("");

        var model = new PublicInfoModel
        {
            DisplayType = displayType
        };

        if(displayType=="footer")
              return View("~/Plugins/Widgets.CustomerQuery/Views/Footer.cshtml");

        return View("~/Plugins/Widgets.CustomerQuery/Views/PublicInfo.cshtml", model);
    }

}
