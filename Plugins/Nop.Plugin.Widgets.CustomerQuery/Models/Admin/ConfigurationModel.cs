using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.CustomerQuery.Models.Admin;
public record ConfigurationModel : BaseNopModel, ISettingsModel
{
    public int ActiveStoreScopeConfiguration { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.CustomerQuery.Settings.Enabled")]
    public bool Enabled { get; set; }
    public bool Enabled_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.CustomerQuery.Settings.DisplayInNavigation")]
    public bool DisplayInNavigation { get; set; }
    public bool DisplayInNavigation_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.CustomerQuery.Settings.DisplayInFooter")]
    public bool DisplayInFooter { get; set; }
    public bool DisplayInFooter_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.CustomerQuery.Settings.DisplayInCustomerProfile")]
    public bool DisplayInCustomerProfile { get; set; }
    public bool DisplayInCustomerProfile_OverrideForStore { get; set; }
}
