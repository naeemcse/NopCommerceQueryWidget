using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Data;
using Nop.Plugin.Widgets.CustomerQuery.Domain;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Stores;

namespace Nop.Plugin.Widgets.CustomerQuery.Services;

/// <summary>
/// Customer query service
/// </summary>
public class CustomerQueryService : ICustomerQueryService
{
    #region Fields

    private readonly IRepository<CustomerQueryRecord> _customerQueryRepository;
    private readonly IEmailAccountService _emailAccountService;
    private readonly IStoreService _storeService;
    private readonly IStoreContext _storeContext;
    private readonly IEmailSender _emailSender;
    private readonly EmailAccountSettings _emailAccountSettings;
    private readonly ILocalizationService _localizationService;
    private readonly ICustomerService _customerService;
    private readonly IQueuedEmailService _queuedEmailService;

    #endregion

    #region Ctor

    public CustomerQueryService(
       IRepository<CustomerQueryRecord> customerQueryRepository,
        IEmailAccountService emailAccountService,
         IQueuedEmailService queuedEmailService,
        IStoreService storeService,
        IStoreContext storeContext,
        IEmailSender emailSender,
        EmailAccountSettings emailAccountSettings,
        ILocalizationService localizationService,
           ICustomerService customerService)
    {
        _customerQueryRepository = customerQueryRepository;
        _emailAccountService = emailAccountService;
        _queuedEmailService = queuedEmailService;
        _storeService = storeService;
        _storeContext = storeContext;
        _emailSender = emailSender;
        _emailAccountSettings = emailAccountSettings;
        _localizationService = localizationService;
        _customerService = customerService;
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

        // Send notifications
        await SendCustomerNotificationAsync(query);
        await SendStoreOwnerNotificationAsync(query);
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

    /// <summary>
    /// Gets the email account to use for sending
    /// </summary>
    protected virtual async Task<EmailAccount> GetEmailAccountAsync()
    {
        var emailAccount = await _emailAccountService.GetEmailAccountByIdAsync(_emailAccountSettings.DefaultEmailAccountId)
            ?? (await _emailAccountService.GetAllEmailAccountsAsync()).FirstOrDefault();

        return emailAccount;
    }


    public virtual async Task SendCustomerNotificationAsync(CustomerQueryRecord query)
    {
        var emailAccount = await GetEmailAccountAsync();
        if (emailAccount == null)
            return;

        var store = await _storeContext.GetCurrentStoreAsync();

        // Build email message
        var subject = await _localizationService.GetResourceAsync("Plugins.Widgets.CustomerQuery.Email.Customer.Subject");
        var body = await _localizationService.GetResourceAsync("Plugins.Widgets.CustomerQuery.Email.Customer.Body");

        body = body.Replace("%CustomerName%", query.Name)
                  .Replace("%QueryMessage%", query.Message)
                  .Replace("%StoreName%", store.Name);

        // Send real email to customer
        /* await _emailSender.SendEmailAsync(
             emailAccount,
             subject,
             body,
             emailAccount.Email,
             emailAccount.DisplayName,
             query.Email,
             query.Name);*/


        //  queue email for later sending

        var email = new QueuedEmail
        {
            Priority = QueuedEmailPriority.High,
            From = emailAccount.Email,
            FromName = emailAccount.DisplayName,
            To = query.Email,
            ToName = query.Name,
            Subject = subject,
            Body = body,
            CreatedOnUtc = DateTime.UtcNow,
            EmailAccountId = emailAccount.Id,
            DontSendBeforeDateUtc = null
        };

        await _queuedEmailService.InsertQueuedEmailAsync(email);
    }


    public virtual async Task SendStoreOwnerNotificationAsync(CustomerQueryRecord query)
    {
        var emailAccount = await GetEmailAccountAsync();
        if (emailAccount == null)
            return;

        var store = await _storeContext.GetCurrentStoreAsync();

        // Get all administrators
        var adminRole = await _customerService.GetCustomerRoleBySystemNameAsync(NopCustomerDefaults.AdministratorsRoleName);
        if (adminRole == null)
            return;
        var admins = await _customerService.GetAllCustomersAsync(customerRoleIds: new[] { adminRole.Id });
        if (!admins.Any())
            return;
        var subject = await _localizationService.GetResourceAsync("Plugins.Widgets.CustomerQuery.Email.StoreOwner.Subject");
        var body = await _localizationService.GetResourceAsync("Plugins.Widgets.CustomerQuery.Email.StoreOwner.Body");

        body = body.Replace("%CustomerName%", query.Name)
                  .Replace("%CustomerEmail%", query.Email)
                  .Replace("%QuerySubject%", query.Subject ?? "N/A")
                  .Replace("%QueryMessage%", query.Message)
                  .Replace("%StoreName%", store.Name);

        // sample queue for data
        //  queue email for later sending

        var email = new QueuedEmail
        {
            Priority = QueuedEmailPriority.High,
            From = emailAccount.Email,
            FromName = emailAccount.DisplayName,
            To = emailAccount.Email,
            ToName = emailAccount.DisplayName,
            Subject = subject,
            Body = body,
            CreatedOnUtc = DateTime.UtcNow,
            EmailAccountId = emailAccount.Id,
            DontSendBeforeDateUtc = null
        };


        // Send email to each administrator
        foreach (var admin in admins)
        {
            if (string.IsNullOrEmpty(admin.Email))
                continue;
            // for sending real email
            /*await _emailSender.SendEmailAsync(
                emailAccount,
                subject,
                body,
                emailAccount.Email,
                emailAccount.DisplayName,
                admin.Email,
                admin.Username ?? admin.Email);*/
            email.To = admin.Email;

            await _queuedEmailService.InsertQueuedEmailAsync(email);

        }

    }


        #endregion
    }
