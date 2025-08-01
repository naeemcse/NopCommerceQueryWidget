using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.CustomerQuery.Models.Admin;

/// <summary>
/// Represents a customer query model
/// </summary>
public partial record CustomerQueryModel : BaseNopEntityModel
{
    public CustomerQueryModel()
    {
        Name = string.Empty;
        Email = string.Empty;
        Subject = string.Empty;
        Message = string.Empty;
    }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public DateTime CreatedOnUtc { get; set; }
}