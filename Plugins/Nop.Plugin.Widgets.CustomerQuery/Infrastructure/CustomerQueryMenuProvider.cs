using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Nop.Services.Plugins;
using Nop.Web.Framework;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Widgets.CustomerQuery.Infrastructure;

public class CustomerQueryMenuProvider : BasePlugin, IAdminMenuPlugin
{
    #region Fields
    private readonly IUrlHelperFactory _urlHelperFactory;
    #endregion

    #region Ctor
    public CustomerQueryMenuProvider(IUrlHelperFactory urlHelperFactory)
    {
        _urlHelperFactory = urlHelperFactory;
    }
    #endregion

    #region Methods
    public override string GetConfigurationPageUrl()
    {
        return null; // No configuration page for menu provider
    }

    public async Task ManageSiteMapAsync(AdminMenuItem rootNode)
    {
        var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
        if (pluginNode == null)
            return;

        var menuItem = new AdminMenuItem
        {
            SystemName = "CustomerQueries",
            Title = "Customer Queries",
            IconClass = "far fa-dot-circle",
            Visible = true
        };

        // Set the URL instead of controller/action names
        menuItem.Url = $"/Admin/AdminCustomerQuery/List";

        pluginNode.ChildNodes.Add(menuItem);

        await Task.CompletedTask;
    }
    #endregion
}