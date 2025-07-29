using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Services.Plugins;
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
        var systemNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "System");
        if (systemNode == null)
            return;

        systemNode.ChildNodes.Add(new AdminMenuItem
        {
            SystemName = "CustomerQueries",
            Title = "Customer Queries",
            IconClass = "far fa-dot-circle", // Changed to match System menu style
            Visible = true,
            PermissionNames = new List<string> { Nop.Services.Security.StandardPermission.System.MANAGE_SYSTEM_LOG }, // Add appropriate permission
            Url = "/Admin/AdminCustomerQuery/List"
        });

        await Task.CompletedTask;
    }

    #endregion
}