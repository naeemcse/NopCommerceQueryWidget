using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.CustomerQuery.Models.Admin;

/// <summary>
/// Represents a customer query search model
/// </summary>
public partial record CustomerQuerySearchModel : BaseSearchModel
{
    public string SearchName { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.CustomerQuery.List.SearchEmail")]
    [DataType(DataType.EmailAddress)]
    public string SearchEmail { get; set; }

    public string SearchSubject { get; set; }
    public string SearchMessage { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.CustomerQuery.List.SearchCreatedOnFrom")]
    [UIHint("DateNullable")] // This will show only date picker
    public DateTime? SearchCreatedOnFrom { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.CustomerQuery.List.SearchCreatedOnTo")]
    [UIHint("DateNullable")] // This will show only date picker
    public DateTime? SearchCreatedOnTo { get; set; }

    public bool SearchShowHidden { get; set; } = true;
}
// Custom validation attribute
public class DateGreaterThanOrEqualToAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public DateGreaterThanOrEqualToAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var currentValue = value as DateTime?;
        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

        if (property == null)
            throw new ArgumentException("Property with this name not found");

        var comparisonValue = property.GetValue(validationContext.ObjectInstance) as DateTime?;

        if (currentValue.HasValue && comparisonValue.HasValue)
        {
            if (currentValue.Value < comparisonValue.Value)
                return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}