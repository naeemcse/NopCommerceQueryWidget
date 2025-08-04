using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.CustomerQuery.Models;
public record PublicInfoModel : BaseNopModel
{
    public PublicInfoModel()
    {
        DisplayType = "default";
    }

    /// <summary>
    /// Gets or sets the display type (navigation, footer, profile)
    /// </summary>
    public string DisplayType { get; set; }
}
