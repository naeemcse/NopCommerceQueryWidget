using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.CustomerQuery.Models.Admin;

/// <summary>
/// Represents a customer query search model
/// </summary>
public partial record CustomerQuerySearchModel : BaseSearchModel
{
    public string SearchName { get; set; }
    public string SearchEmail { get; set; }
    public string SearchSubject { get; set; }
    public string SearchMessage { get; set; }
    public DateTime? SearchCreatedOnFrom { get; set; }
    public DateTime? SearchCreatedOnTo { get; set; }
    public bool SearchShowHidden { get; set; }
}