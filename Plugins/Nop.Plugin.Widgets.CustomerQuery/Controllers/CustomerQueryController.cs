using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Data;
using Nop.Plugin.Widgets.CustomerQuery.Domain;
using Nop.Plugin.Widgets.CustomerQuery.Models.Public;
using Nop.Services.Messages;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.CustomerQuery.Controller;
public class CustomerQueryController: BasePluginController
{
    private readonly IWorkContext _workContext;
    private readonly IRepository<CustomerQueryRecord> _customerQueryRepository;
    private readonly IEmailSender _emailSender;
    private readonly IStoreContext _storeContext;

    public CustomerQueryController(
        IWorkContext workContext,
        IRepository<CustomerQueryRecord> customerQueryRepository,
        IEmailSender emailSender,
        IStoreContext storeContext)
    {
        _workContext = workContext;
        _customerQueryRepository = customerQueryRepository;
        _emailSender = emailSender;
        _storeContext = storeContext;
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

        await _customerQueryRepository.InsertAsync(query);

        // Send email to store owner
        
       /* var store = await _storeContext.GetCurrentStoreAsync();
        if (!string.IsNullOrEmpty(store.Email))
        {
            await _emailSender.SendEmailAsync(
                store.Email,
                "New Customer Query",
                $"Name: {model.Name}<br/>Email: {model.Email}<br/>Subject: {model.Subject}<br/>Message: {model.Message}",
                true);
        }
        SuccessNotification("Your query has been submitted successfully.");
*/
        return RedirectToRoute("HomePage");
    }



}
