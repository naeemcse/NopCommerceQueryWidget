using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.CustomerQuery.Factories;
using Nop.Plugin.Widgets.CustomerQuery.Models.Admin;
using Nop.Plugin.Widgets.CustomerQuery.Services;
using Nop.Services.Messages;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.CustomerQuery.Controllers;

[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class AdminCustomerQueryController : BasePluginController
{
    private readonly ICustomerQueryService _customerQueryService;
    private readonly INotificationService _notificationService;

    private readonly ICustomerQueryModelFactory _customerQueryModelFactory;

    public AdminCustomerQueryController(
        ICustomerQueryService customerQueryService,
        INotificationService notificationService,
          ICustomerQueryModelFactory customerQueryModelFactory)
    {
        _customerQueryService = customerQueryService;
        _notificationService = notificationService;
        _customerQueryModelFactory = customerQueryModelFactory;
    }

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    public virtual async Task<IActionResult> List()
    {
        var model = new CustomerQuerySearchModel();
        model = await _customerQueryModelFactory.PrepareSearchModelAsync(model);
        return View("~/Plugins/Widgets.CustomerQuery/Views/Admin/List.cshtml", model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> List(CustomerQuerySearchModel searchModel)
    {
        var model = await _customerQueryModelFactory.PrepareListModelAsync(searchModel);
        return Json(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var query = await _customerQueryService.GetQueryByIdAsync(id);
        if (query == null)
            return new NullJsonResult();

        await _customerQueryService.DeleteQueryAsync(query);

        _notificationService.SuccessNotification("The query has been deleted successfully.");

        return new NullJsonResult();
    }

    public virtual async Task<IActionResult> Details(int id)
    {
        var query = await _customerQueryService.GetQueryByIdAsync(id);
        if (query == null)
            return RedirectToAction("List");

        // Pass null as the model parameter to let the factory create a new one
        var model = await _customerQueryModelFactory.PrepareQueryModelAsync(
      model: null,
      query: query,
      excludeProperties: false
  );

        if (model == null)
            return RedirectToAction("List");

        return View("~/Plugins/Widgets.CustomerQuery/Views/Admin/Details.cshtml", model);
    }
}