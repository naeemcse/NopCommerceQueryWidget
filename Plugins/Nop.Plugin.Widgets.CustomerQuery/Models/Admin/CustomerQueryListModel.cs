using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.CustomerQuery.Models.Admin;

/// <summary>
/// Represents a customer query list model
/// </summary>
public partial record CustomerQueryListModel : BasePagedListModel<CustomerQueryModel>
{
}