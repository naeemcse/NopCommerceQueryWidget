using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.CustomerQuery.Models.Admin;
using Nop.Plugin.Widgets.CustomerQuery.Services;
using Nop.Services.Messages;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Models.Extensions;
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

    public AdminCustomerQueryController(
        ICustomerQueryService customerQueryService,
        INotificationService notificationService)
    {
        _customerQueryService = customerQueryService;
        _notificationService = notificationService;
    }

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    public virtual IActionResult List()
    {
        var model = new CustomerQuerySearchModel();
        return View("~/Plugins/Widgets.CustomerQuery/Views/Admin/List.cshtml", model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> List(CustomerQuerySearchModel searchModel)
    {
        var queries = await _customerQueryService.GetAllQueriesAsync(
            createdFromUtc: searchModel.SearchCreatedOnFrom,
            createdToUtc: searchModel.SearchCreatedOnTo,
            email: searchModel.SearchEmail,
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        var model = await new CustomerQueryListModel().PrepareToGridAsync(searchModel, queries, () =>
        {
            return queries.SelectAwait(async query =>
            {
                var queryModel = new CustomerQueryModel
                {
                    Id = query.Id,
                    Name = query.Name,
                    Email = query.Email,
                    Subject = query.Subject,
                    Message = query.Message,
                    CreatedOnUtc = query.CreatedOnUtc
                };

                return queryModel;
            });
        });

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
}