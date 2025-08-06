using Nop.Services.Events;
using Nop.Services.Security;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Widgets.CustomerQuery.Infrastructure;
public class EventConsumer: IConsumer<AdminMenuCreatedEvent>
{
    private readonly IPermissionService _permissionService;

    public EventConsumer(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }
    public async Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
    {
        // Create main menu item
        var customerQueryMenu = new AdminMenuItem
        {
            SystemName = "CustomerQueryMenu",
            Title = "Customer Query",
            IconClass = "far fa-comment-dots",
            Visible = true
        };

        // Add submenu items
        customerQueryMenu.ChildNodes.Add(new AdminMenuItem
        {
            SystemName = "CustomerQuery.Configuration",
            Title = "Configuration",
            Url = eventMessage.GetMenuItemUrl("AdminCustomerQuery", "Configure"),
            IconClass = "fas fa-cog",
            Visible = true
        });

        customerQueryMenu.ChildNodes.Add(new AdminMenuItem
        {
            SystemName = "CustomerQuery.List",
            Title = "Query List",
            Url = eventMessage.GetMenuItemUrl("AdminCustomerQuery", "List"),
            IconClass = "fas fa-list",
            Visible = true
        });
        customerQueryMenu.ChildNodes.Add(new AdminMenuItem
        {
            SystemName = "CustomerQuery.Details",
            Title = "Query Details",            
            Visible = false
        });

        // Insert the menu after "Dashboard" menu item
        eventMessage.RootMenuItem.InsertAfter("Dashboard", customerQueryMenu);
    }
}
