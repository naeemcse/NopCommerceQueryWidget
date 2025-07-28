using Nop.Core;
using Nop.Plugin.Widgets.CustomerQuery.Domain;

namespace Nop.Plugin.Widgets.CustomerQuery.Services;

/// <summary>
/// Customer query service interface
/// </summary>
public interface ICustomerQueryService
{
    /// <summary>
    /// Inserts a customer query
    /// </summary>
    /// <param name="query">Customer query</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertQueryAsync(CustomerQueryRecord query);

    /// <summary>
    /// Gets all customer queries
    /// </summary>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Customer queries</returns>
    Task<IPagedList<CustomerQueryRecord>> GetAllQueriesAsync(int pageIndex = 0, int pageSize = int.MaxValue);

    /// <summary>
    /// Gets a customer query by identifier
    /// </summary>
    /// <param name="queryId">Query identifier</param>
    /// <returns>Customer query</returns>
    Task<CustomerQueryRecord> GetQueryByIdAsync(int queryId);

    /// <summary>
    /// Delete a customer query
    /// </summary>
    /// <param name="query">Customer query</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteQueryAsync(CustomerQueryRecord query);
}
