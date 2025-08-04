using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Security;
using Nop.Plugin.Widgets.CustomerQuery.Domain;
using Nop.Plugin.Widgets.CustomerQuery.Models.Public;
using Nop.Plugin.Widgets.CustomerQuery.Services;
using Nop.Services.Localization;
using Nop.Web.Controllers;

namespace Nop.Plugin.Widgets.CustomerQuery.Controller;
public class CustomerQueryController: BasePublicController
{
    private readonly ICustomerQueryService _customerQueryService;

    protected readonly CaptchaSettings _captchaSettings;
    protected readonly ILocalizationService _localizationService;


    public CustomerQueryController(
        ICustomerQueryService customerQueryService,
        IWorkContext workContext , CaptchaSettings captchaSettings,
        ILocalizationService localizationService)
    {
        _captchaSettings = captchaSettings;
        _customerQueryService = customerQueryService;
        _localizationService = localizationService;
    }

    public IActionResult Index()
    {
        var model = new SubmitQueryModel();
        model.DisplayCaptcha = true;
        // return View(  model);
        return View("~/Plugins/Widgets.CustomerQuery/Views/CustomerQuery/Index.cshtml", model);

    }

    [HttpPost]
    public async Task<IActionResult> Index(SubmitQueryModel model, bool captchaValid)
    {
        if (!ModelState.IsValid)
            return View("Index", model);

        //validate CAPTCHA
        if (_captchaSettings.Enabled && _captchaSettings.ShowOnContactUsPage && !captchaValid)
        {
            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
        }
        // Binding to Save to database
        var query = new CustomerQueryRecord
        { 
            Name = model.Name,
            Email = model.Email,
            Subject = model.Subject ?? "N/A",
            Message = model.Message,
            CreatedOnUtc = DateTime.UtcNow
        };

        await _customerQueryService.InsertQueryAsync(query);

        
        return RedirectToAction("Success");
    }

    public IActionResult Success()
    {
        return View("~/Plugins/Widgets.CustomerQuery/Views/CustomerQuery/Success.cshtml");

    }
}
