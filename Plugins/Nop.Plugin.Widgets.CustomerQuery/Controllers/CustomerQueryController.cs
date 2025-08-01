using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.CustomerQuery.Domain;
using Nop.Plugin.Widgets.CustomerQuery.Models.Public;
using Nop.Plugin.Widgets.CustomerQuery.Services;
using Nop.Web.Controllers;

namespace Nop.Plugin.Widgets.CustomerQuery.Controller;
public class CustomerQueryController: BasePublicController
{
    private readonly ICustomerQueryService _customerQueryService;
   

    public CustomerQueryController(
        ICustomerQueryService customerQueryService,
        IWorkContext workContext)
    {
        _customerQueryService = customerQueryService;       
    }

    public IActionResult Index()
    {
        var model = new SubmitQueryModel();
        // return View(  model);
        return View("~/Plugins/Widgets.CustomerQuery/Views/CustomerQuery/Index.cshtml", model);

    }

    [HttpPost]
    public async Task<IActionResult> Index(SubmitQueryModel model)
    {
        if (!ModelState.IsValid)
            return View("Index", model);
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
