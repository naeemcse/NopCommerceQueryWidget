using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.CustomerQuery.Models.Public;
public record SubmitQueryModel : BaseNopModel
{
    [NopResourceDisplayName("Plugins.Widgets.CustomerQuery.Fields.Name")]
    [Required(ErrorMessage = "Plugins.Widgets.CustomerQuery.Fields.Name.Required")]
    public string Name { get; set; }

    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Plugins.Widgets.CustomerQuery.Fields.Email")]
    [Required(ErrorMessage = "Plugins.Widgets.CustomerQuery.Fields.Email.Required")]
    public string Email { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.CustomerQuery.Fields.Subject")]
    public string Subject { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.CustomerQuery.Fields.Message")]
    [Required(ErrorMessage = "Plugins.Widgets.CustomerQuery.Fields.Message.Required")]
    public string Message { get; set; }
}
