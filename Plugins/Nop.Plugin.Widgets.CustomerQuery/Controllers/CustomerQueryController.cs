using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Data;
using Nop.Plugin.Widgets.CustomerQuery.Domain;
using Nop.Plugin.Widgets.CustomerQuery.Models.Admin;
using Nop.Plugin.Widgets.CustomerQuery.Models.Public;
using Nop.Plugin.Widgets.CustomerQuery.Services;
using Nop.Services.Messages;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.CustomerQuery.Controller;
public class CustomerQueryController: BasePluginController
{
    private readonly ICustomerQueryService _customerQueryService;
    private readonly IWorkContext _workContext;

    public CustomerQueryController(
        ICustomerQueryService customerQueryService,
        IWorkContext workContext)
    {
        _customerQueryService = customerQueryService;
        _workContext = workContext;
    }

    public IActionResult SubmitQuery()
    {
        var model = new SubmitQueryModel();
        return View("~/Plugins/Widgets.CustomerQuery/Views/SubmitQuery.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitQuery(SubmitQueryModel model)
    {
        if (!ModelState.IsValid)
            return View("~/Plugins/Widgets.CustomerQuery/Views/SubmitQuery.cshtml", model);

        // Save to database
        var query = new CustomerQueryRecord
        {
            Name = model.Name,
            Email = model.Email,
            Subject = model.Subject ?? "N/A",
            Message = model.Message,
            CreatedOnUtc = DateTime.UtcNow
        };

        await _customerQueryService.InsertQueryAsync(query);

       // SuccessNotification("Your query has been submitted successfully.");
        return RedirectToRoute("HomePage");
    }
}
