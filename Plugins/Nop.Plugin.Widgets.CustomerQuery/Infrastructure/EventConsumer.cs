using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
       /* // Add your custom menu item
        eventMessage.RootMenuItem.ChildItems.Add(new AdminMenuItem
        {
            SystemName = "CustomerQueryMenu",
            Title = "Customer Query",
            Url = eventMessage.GetMenuItemUrl("AdminCustomerQuery", "List"),
            IconClass = "far fa-comment-dots", // or any FontAwesome icon
            Visible = true
        });*/

        eventMessage.RootMenuItem.InsertBefore("Local plugins",
            new AdminMenuItem
            {
                SystemName = "CustomerQueryMenu",
                Title = "Customer Query",
                Url = eventMessage.GetMenuItemUrl("AdminCustomerQuery", "List"),
                IconClass = "far fa-comment-dots",
                Visible = true,
            });
    }
}
