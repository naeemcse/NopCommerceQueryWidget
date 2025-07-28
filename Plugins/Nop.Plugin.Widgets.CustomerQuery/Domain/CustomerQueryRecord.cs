using Nop.Core;

namespace Nop.Plugin.Widgets.CustomerQuery.Domain;

/// <summary>
/// Represents a customer query record
/// </summary>
public class CustomerQueryRecord : BaseEntity
{
    /// <summary>
    /// Gets or sets the name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the subject
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Gets or sets the message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the date and time of instance creation
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }
}