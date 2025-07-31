using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.CustomerQuery.Models.Public;

public record SubmitQueryModel : BaseNopModel
{
    [NopResourceDisplayName("Plugins.Widgets.CustomerQuery.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.CustomerQuery.Fields.Email")]
    public string Email { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.CustomerQuery.Fields.Subject")]
    public string Subject { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.CustomerQuery.Fields.Message")]
    public string Message { get; set; }


    public bool SuccessfullySent { get; set; }
    public string Result { get; set; }

    public bool DisplayCaptcha { get; set; }

}
