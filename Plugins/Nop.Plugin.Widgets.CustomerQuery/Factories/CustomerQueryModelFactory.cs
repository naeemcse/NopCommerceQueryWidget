using System.Diagnostics;
using Nop.Core;
using Nop.Plugin.Widgets.CustomerQuery.Domain;
using Nop.Plugin.Widgets.CustomerQuery.Models.Admin;
using Nop.Plugin.Widgets.CustomerQuery.Models.Public;
using Nop.Plugin.Widgets.CustomerQuery.Services;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Widgets.CustomerQuery.Factories;
public class CustomerQueryModelFactory : ICustomerQueryModelFactory
{

    private readonly ICustomerQueryService _customerQueryService;
    private readonly IBaseAdminModelFactory _baseAdminModelFactory;
    private readonly IWorkContext _workContext;

    public CustomerQueryModelFactory(
       ICustomerQueryService customerQueryService,
       IBaseAdminModelFactory baseAdminModelFactory,
       IWorkContext workContext)
    {
        _customerQueryService = customerQueryService;
        _baseAdminModelFactory = baseAdminModelFactory;
        _workContext = workContext;
    }


    /// <summary>
    /// Prepare customer query search model
    /// </summary>
    public virtual async Task<CustomerQuerySearchModel> PrepareSearchModelAsync(CustomerQuerySearchModel searchModel)
    {
        if (searchModel == null)
            throw new ArgumentNullException(nameof(searchModel));

        // Set grid page parameters
        searchModel.SetGridPageSize(10); // This is the correct method to use

        searchModel.SearchShowHidden = true;

        return searchModel;
    }

    /// <summary>
    /// Prepare paged customer query list model
    /// </summary>
    public virtual async Task<CustomerQueryListModel> PrepareListModelAsync(CustomerQuerySearchModel searchModel)
    {
        if (searchModel == null)
            throw new ArgumentNullException(nameof(searchModel));

        // Get queries
        var queries = await _customerQueryService.GetAllQueriesAsync(
            createdFromUtc: searchModel.SearchCreatedOnFrom,
            createdToUtc: searchModel.SearchCreatedOnTo,
            email: searchModel.SearchEmail,
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        // Prepare list model
        var model = await new CustomerQueryListModel().PrepareToGridAsync(searchModel, queries, () =>
        {
            return queries.SelectAwait(async query =>
            {
                // Prepare query model
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

        return model;
    }

    /// <summary>
    /// Prepare customer query model
    /// </summary>
    public virtual async Task<CustomerQueryModel> PrepareQueryModelAsync(CustomerQueryModel model, CustomerQueryRecord query, bool excludeProperties = false)
    {
        // Early return if query is null
        if (query == null)
            return null;

        // Create new model if not provided
        if (model == null)
            model = new CustomerQueryModel();

        // Map properties
        model.Id = query.Id;
        model.Name = query.Name ?? string.Empty;  // Ensure no null values
        model.Email = query.Email ?? string.Empty;
        model.Subject = query.Subject ?? string.Empty;
        model.Message = query.Message ?? string.Empty;
        model.CreatedOnUtc = query.CreatedOnUtc;

        return model;
    }
    /// <summary>
    /// Prepare submit query model
    /// </summary>
    public virtual async Task<SubmitQueryModel> PrepareSubmitQueryModelAsync(SubmitQueryModel model = null)
    {
        model ??= new SubmitQueryModel();

        // Pre-fill email if customer is logged in
        if (string.IsNullOrEmpty(model.Email))
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            model.Email = customer.Email;
        }

        return model;
    }
}
