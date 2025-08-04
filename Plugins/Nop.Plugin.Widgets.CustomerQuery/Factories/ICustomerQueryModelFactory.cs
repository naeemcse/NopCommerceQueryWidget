using Nop.Plugin.Widgets.CustomerQuery.Domain;
using Nop.Plugin.Widgets.CustomerQuery.Models.Admin;
using Nop.Plugin.Widgets.CustomerQuery.Models.Public;

namespace Nop.Plugin.Widgets.CustomerQuery.Factories;

    public interface ICustomerQueryModelFactory
    {
        Task<CustomerQuerySearchModel> PrepareSearchModelAsync(CustomerQuerySearchModel searchModel);
        Task<CustomerQueryListModel> PrepareListModelAsync(CustomerQuerySearchModel searchModel);
        Task<CustomerQueryModel> PrepareQueryModelAsync(CustomerQueryModel model, CustomerQueryRecord query, bool excludeProperties = false);
        Task<SubmitQueryModel> PrepareSubmitQueryModelAsync(SubmitQueryModel model = null);
    //Task<ConfigurationModel> PrepareConfigurationModelAsync(ConfigurationModel model);
}
