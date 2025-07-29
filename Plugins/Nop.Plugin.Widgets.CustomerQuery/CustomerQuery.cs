using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Core.Infrastructure;
using Nop.Data.Migrations;
using Nop.Plugin.Widgets.CustomerQuery.Components;
using Nop.Plugin.Widgets.CustomerQuery.Data;
using Nop.Plugin.Widgets.CustomerQuery.Services;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.CustomerQuery
{
    public class CustomerQuery : BasePlugin, IWidgetPlugin
    {
        #region Fields

        protected readonly ILocalizationService _localizationService;
        protected readonly INopFileProvider _fileProvider;
        protected readonly IPictureService _pictureService;
        protected readonly ISettingService _settingService;
        protected readonly IWebHelper _webHelper;
        protected readonly WidgetSettings _widgetSettings;
        protected readonly IMigrationManager _migrationManager;

        #endregion

        #region Ctor

        public CustomerQuery(ILocalizationService localizationService,
            INopFileProvider fileProvider,
            IPictureService pictureService,
            ISettingService settingService,
            IWebHelper webHelper,
            WidgetSettings widgetSettings,
             IMigrationManager migrationManager)
        {
            _localizationService = localizationService;
            _fileProvider = fileProvider;
            _pictureService = pictureService;
            _settingService = settingService;
            _webHelper = webHelper;
            _widgetSettings = widgetSettings;
            _migrationManager = migrationManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the widget zones
        /// </returns>
        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.HomepageTop });
        }


        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(WidgetsCustomerQueryViewComponent);
        }


        public override async Task InstallAsync()
        {
            // Create the table using migrations
            _migrationManager.ApplyUpMigrations(typeof(CustomerQueryRecordBuilder).Assembly);

            // Localization
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Widgets.CustomerQuery.SubmitQuery"] = "Submit a Query",
                ["Plugins.Widgets.CustomerQuery.Fields.Name"] = "Name",
                ["Plugins.Widgets.CustomerQuery.Fields.Name.Required"] = "Name is required",
                ["Plugins.Widgets.CustomerQuery.Fields.Email"] = "Email",
                ["Plugins.Widgets.CustomerQuery.Fields.Email.Required"] = "Email is required",
                ["Plugins.Widgets.CustomerQuery.Fields.Email.Wrong"] = "Wrong email format",
                ["Plugins.Widgets.CustomerQuery.Fields.Subject"] = "Subject",
                ["Plugins.Widgets.CustomerQuery.Fields.Message"] = "Message",
                ["Plugins.Widgets.CustomerQuery.Fields.Message.Required"] = "Message is required",
                ["Plugins.Widgets.CustomerQuery.Submit"] = "Submit Query"

            });
            // Any DB or setting setup
            await base.InstallAsync();

           


        }


        public override async Task UninstallAsync()
        {
            // Remove the table
            _migrationManager.ApplyDownMigrations(typeof(CustomerQueryRecordBuilder).Assembly);

            // Delete locale resources
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.CustomerQuery");

            // Cleanup
            await base.UninstallAsync();
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
        /// </summary>
        public bool HideInWidgetList => false;

        #endregion
    }
}
