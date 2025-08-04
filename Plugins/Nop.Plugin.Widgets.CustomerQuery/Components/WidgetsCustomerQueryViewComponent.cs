using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.CustomerQuery.Domain;
using Nop.Plugin.Widgets.CustomerQuery.Models;
using Nop.Web.Framework.Components;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.CustomerQuery.Components;
public class WidgetsCustomerQueryViewComponent: NopViewComponent
{

    private readonly CustomerQuerySettings _customerQuerySettings;

    public WidgetsCustomerQueryViewComponent(CustomerQuerySettings customerQuerySettings)
    {
        _customerQuerySettings = customerQuerySettings;
    }

    public async Task<IViewComponentResult> InvokeAsync1(string widgetZone, object additionalData = null)
    {
        // You can add logic here to handle the widget rendering based on the widgetZone or additionalData
        // return View("~/Plugins/Widgets.CustomerQuery/Views/CustomerQuerey/Index.cshtml");

        if (!_customerQuerySettings.Enabled)
            return Content("");

        
        bool shouldDisplay = false;
        if (widgetZone == PublicWidgetZones.HeaderLinksAfter)
            shouldDisplay = _customerQuerySettings.DisplayInNavigation;
        else if (widgetZone == PublicWidgetZones.Footer)
            shouldDisplay = _customerQuerySettings.DisplayInFooter;
        else if (widgetZone == PublicWidgetZones.CustomerInfoTop)
            shouldDisplay = _customerQuerySettings.DisplayInCustomerProfile;

        if (!shouldDisplay)
            return Content("");

        var displayType = "default";
        if (widgetZone == PublicWidgetZones.HeaderLinksAfter)
            displayType = "navigation";
        else if (widgetZone == PublicWidgetZones.Footer)
            displayType = "footer";
        else if (widgetZone == PublicWidgetZones.CustomerInfoTop)
            displayType = "profile";

        var model = new PublicInfoModel
        {
            DisplayType = displayType
        };

        return View("~/Plugins/Widgets.CustomerQuery/Views/PublicInfo.cshtml", model);

    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData = null)
    {
        if (!_customerQuerySettings.Enabled)
            return Content("");

        // Use traditional if-else for string comparison
        string displayType;
        if (widgetZone == "header-menu-before" || widgetZone == "header-links-after")
            displayType = "navigation";
        else if (widgetZone == "footer")
            displayType = "footer";
        else if (widgetZone == "account-navigation-after" || widgetZone == "customer-info-top")
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

        return View("~/Plugins/Widgets.CustomerQuery/Views/PublicInfo.cshtml", model);
    }

}
