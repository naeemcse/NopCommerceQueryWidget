using FluentValidation;
using Nop.Plugin.Widgets.CustomerQuery.Models.Public;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.CustomerQuery.Validators;

public class SubmitQueryValidator : BaseNopValidator<SubmitQueryModel>
{
    public SubmitQueryValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Widgets.CustomerQuery.Fields.Name.Required"));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Widgets.CustomerQuery.Fields.Email.Required"))
            .EmailAddress()
            .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Widgets.CustomerQuery.Fields.Email.Wrong"));

        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Widgets.CustomerQuery.Fields.Message.Required"));
    }
}