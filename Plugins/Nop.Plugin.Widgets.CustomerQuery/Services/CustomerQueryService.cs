using Nop.Core;
using Nop.Data;
using Nop.Plugin.Widgets.CustomerQuery.Domain;
using Nop.Services.Messages;

namespace Nop.Plugin.Widgets.CustomerQuery.Services;

/// <summary>
/// Customer query service
/// </summary>
public class CustomerQueryService : ICustomerQueryService
{
    #region Fields

    private readonly IRepository<CustomerQueryRecord> _customerQueryRepository;
    private readonly IEmailSender _emailSender;
    private readonly IStoreContext _storeContext;

    #endregion

    #region Ctor

    public CustomerQueryService(
        IRepository<CustomerQueryRecord> customerQueryRepository,
        IEmailSender emailSender,
        IStoreContext storeContext)
    {
        _customerQueryRepository = customerQueryRepository;
        _emailSender = emailSender;
        _storeContext = storeContext;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Inserts a customer query
    /// </summary>
    /// <param name="query">Customer query</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task InsertQueryAsync(CustomerQueryRecord query)
    {
        await _customerQueryRepository.InsertAsync(query);

        // Send notification email
        //var store = await _storeContext.GetCurrentStoreAsync();
        //if (!string.IsNullOrEmpty(store.Email))
        //{
        //    await _emailSender.SendEmailAsync(
        //        store.Email,
        //        "New Customer Query",
        //        $"Name: {query.Name}<br/>Email: {query.Email}<br/>Subject: {query.Subject}<br/>Message: {query.Message}",
        //        true);
        //}
    }

    /// <summary>
    /// Gets all customer queries
    /// </summary>
    /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
    /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
    /// <param name="email">Email to search for</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Customer queries</returns>
    public virtual async Task<IPagedList<CustomerQueryRecord>> GetAllQueriesAsync(
        DateTime? createdFromUtc = null,
        DateTime? createdToUtc = null,
        string email = null,
        int pageIndex = 0,
        int pageSize = int.MaxValue)
    {
        var query = _customerQueryRepository.Table;

        if (createdFromUtc.HasValue)
            query = query.Where(q => q.CreatedOnUtc >= createdFromUtc.Value);

        if (createdToUtc.HasValue)
            query = query.Where(q => q.CreatedOnUtc <= createdToUtc.Value);

        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(q => q.Email.Contains(email));

        query = query.OrderByDescending(q => q.CreatedOnUtc);

        return await query.ToPagedListAsync(pageIndex, pageSize);
    }

    /// <summary>
    /// Gets a customer query by identifier
    /// </summary>
    /// <param name="queryId">Query identifier</param>
    /// <returns>Customer query</returns>
    public virtual async Task<CustomerQueryRecord> GetQueryByIdAsync(int queryId)
    {
        return await _customerQueryRepository.GetByIdAsync(queryId);
    }

    /// <summary>
    /// Delete a customer query
    /// </summary>
    /// <param name="query">Customer query</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteQueryAsync(CustomerQueryRecord query)
    {
        await _customerQueryRepository.DeleteAsync(query);
    }

    #endregion
}
