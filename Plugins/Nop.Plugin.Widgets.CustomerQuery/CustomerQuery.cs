using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Core.Domain.Messages;
using Nop.Core.Infrastructure;
using Nop.Data.Migrations;
using Nop.Plugin.Widgets.CustomerQuery.Components;
using Nop.Plugin.Widgets.CustomerQuery.Data;
using Nop.Plugin.Widgets.CustomerQuery.Services;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
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
        protected readonly ICustomerQueryService _customerQueryService;
        private readonly IMessageTemplateService _messageTemplateService;

        #endregion

        #region Ctor

        public CustomerQuery(ILocalizationService localizationService,
            INopFileProvider fileProvider,
            IPictureService pictureService,
            ISettingService settingService,
            IWebHelper webHelper,
            WidgetSettings widgetSettings,
             IMigrationManager migrationManager,
             ICustomerQueryService customerQueryService,
             IMessageTemplateService messageTemplateService
             )
        {
            _localizationService = localizationService;
            _fileProvider = fileProvider;
            _pictureService = pictureService;
            _settingService = settingService;
            _webHelper = webHelper;
            _widgetSettings = widgetSettings;
            _migrationManager = migrationManager;
            _customerQueryService = customerQueryService;
            _messageTemplateService = messageTemplateService;
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
                ["Plugins.Widgets.CustomerQuery.Submit"] = "Submit Query",
                ["Plugins.Widgets.CustomerQuery.Email.Customer.Subject"] = "Your query has been received",
                ["Plugins.Widgets.CustomerQuery.Email.Customer.Body"] = "<p>Dear %CustomerName%,</p><p>Thank you for submitting your query. We have received your message and will respond shortly.</p><p>Your message:</p><p>%QueryMessage%</p><p>Best regards,</p><p>%StoreName%</p>",
                ["Plugins.Widgets.CustomerQuery.Email.StoreOwner.Subject"] = "New customer query received",
                ["Plugins.Widgets.CustomerQuery.Email.StoreOwner.Body"] = "<p>A new customer query has been submitted:</p><p>From: %CustomerName% (%CustomerEmail%)</p><p>Subject: %QuerySubject%</p><p>Message:</p><p>%QueryMessage%</p>"


            });

            var emailAccount = _customerQueryService.GetEmailAccountAsync();

            var messageTemplates = new List<MessageTemplate>
    {
        new()
        {
            Name = "Customer.Query.Notification",
            Subject = "%Store.Name%. New query from %Customer.FullName%",
            Body = "<p>Hello,</p>" +
                   "<p>A new query has been submitted:</p>" +
                   "<p>Customer: %Customer.FullName% (%Customer.Email%)</p>" +
                   "<p>Subject: %CustomerQuery.Subject%</p>" +
                   "<p>Message: %CustomerQuery.Message%</p>",
            IsActive = true,
            EmailAccountId = emailAccount.Id
        },
        new()
        {
            Name = "Customer.Query.CustomerNotification",
            Subject = "%Store.Name%. Your query has been received",
            Body = "<p>Hello %Customer.FullName%,</p>" +
                   "<p>Thank you for contacting us. We have received your query and will respond shortly.</p>" +
                   "<p>Your message:</p>" +
                   "<p>%CustomerQuery.Message%</p>",
            IsActive = true,
            EmailAccountId = emailAccount.Id
        }
    };

            foreach (var template in messageTemplates)
            {
                await _messageTemplateService.InsertMessageTemplateAsync(template);
            }


            //  await _messageTemplateService.InsertMessageTemplateAsync(messageTemplates);
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
