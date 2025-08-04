using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.CustomerQuery.Domain;
using Nop.Plugin.Widgets.CustomerQuery.Factories;
using Nop.Plugin.Widgets.CustomerQuery.Models.Admin;
using Nop.Plugin.Widgets.CustomerQuery.Services;
using Nop.Services.Configuration;
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
    private readonly ISettingService _settingService;
    private readonly IStoreContext _storeContext;
    private readonly CustomerQuerySettings _customerQuerySettings;

    public AdminCustomerQueryController(
        ICustomerQueryService customerQueryService,
        INotificationService notificationService,
        ICustomerQueryModelFactory customerQueryModelFactory,
        ISettingService settingService,
        IStoreContext storeContext,
        CustomerQuerySettings customerQuerySettings)
    {
        _customerQueryService = customerQueryService;
        _notificationService = notificationService;
        _customerQueryModelFactory = customerQueryModelFactory;
        _settingService = settingService;
        _storeContext = storeContext;
        _customerQuerySettings = customerQuerySettings;
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


    public async Task<IActionResult> Configure()
    {
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var settings = _customerQuerySettings;

        var model = new ConfigurationModel
        {
            Enabled = settings.Enabled,
            DisplayInNavigation = settings.DisplayInNavigation,
            DisplayInFooter = settings.DisplayInFooter,
            DisplayInCustomerProfile = settings.DisplayInCustomerProfile,
            ActiveStoreScopeConfiguration = storeScope
        };

        if (storeScope > 0)
        {
            model.Enabled_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.Enabled, storeScope);
            model.DisplayInNavigation_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.DisplayInNavigation, storeScope);
            model.DisplayInFooter_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.DisplayInFooter, storeScope);
            model.DisplayInCustomerProfile_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.DisplayInCustomerProfile, storeScope);
        }

        return View("~/Plugins/Widgets.CustomerQuery/Views/Admin/Configure.cshtml", model);
    }
    [HttpPost]
    public async Task<IActionResult> Configure2(ConfigurationModel model)
    {
        if (!ModelState.IsValid)
            return await Configure();

        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var settings = _customerQuerySettings;

        // Save settings for each store scope
        settings.Enabled = model.Enabled;
        settings.DisplayInNavigation = model.DisplayInNavigation;
        settings.DisplayInFooter = model.DisplayInFooter;
        settings.DisplayInCustomerProfile = model.DisplayInCustomerProfile;

        /* We do not clear cache after each setting update.
         * This behavior can increase performance because cached settings will not be cleared 
         * and loaded from database after each update */
        await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.Enabled, model.Enabled_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.DisplayInNavigation, model.DisplayInNavigation_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.DisplayInFooter, model.DisplayInFooter_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.DisplayInCustomerProfile, model.DisplayInCustomerProfile_OverrideForStore, storeScope, false);

        // Clear cache
        await _settingService.ClearCacheAsync();

       // _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

        return await Configure();
    }

    [HttpPost]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        if (!ModelState.IsValid)
            return await Configure();

        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();

        // Save settings for each store scope using the correct method
        await _settingService.SaveSettingOverridablePerStoreAsync(_customerQuerySettings, x => x.Enabled, model.Enabled_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(_customerQuerySettings, x => x.DisplayInNavigation, model.DisplayInNavigation_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(_customerQuerySettings, x => x.DisplayInFooter, model.DisplayInFooter_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(_customerQuerySettings, x => x.DisplayInCustomerProfile, model.DisplayInCustomerProfile_OverrideForStore, storeScope, false);

        // Update the settings values
        _customerQuerySettings.Enabled = model.Enabled;
        _customerQuerySettings.DisplayInNavigation = model.DisplayInNavigation;
        _customerQuerySettings.DisplayInFooter = model.DisplayInFooter;
        _customerQuerySettings.DisplayInCustomerProfile = model.DisplayInCustomerProfile;

        // Save the settings
        await _settingService.SaveSettingAsync(_customerQuerySettings);

        // Clear cache
        await _settingService.ClearCacheAsync();

       // _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

        return RedirectToAction("Configure");
    }
}