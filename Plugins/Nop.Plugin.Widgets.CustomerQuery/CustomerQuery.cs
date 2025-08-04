using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Topics;
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
using Nop.Services.Seo;
using Nop.Services.Topics;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Widgets.CustomerQuery
{
    public class CustomerQuery : BasePlugin, IWidgetPlugin
    {
        #region Fields
        private readonly ITopicService _topicService;
        protected readonly ILocalizationService _localizationService;
        protected readonly INopFileProvider _fileProvider;
        protected readonly IPictureService _pictureService;
        protected readonly ISettingService _settingService;
        protected readonly IWebHelper _webHelper;
        protected readonly WidgetSettings _widgetSettings;
        protected readonly IMigrationManager _migrationManager;
        protected readonly ICustomerQueryService _customerQueryService;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IUrlRecordService _urlRecordService;

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
             IMessageTemplateService messageTemplateService,
             ITopicService topicService,
              IUrlRecordService urlRecordService)
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
            _topicService = topicService;
            _urlRecordService = urlRecordService;
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
       /* public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.HeaderLinksAfter });
        }


        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(WidgetsCustomerQueryViewComponent);
        }*/

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string>
        {
            PublicWidgetZones.HeaderMenuBefore,
            PublicWidgetZones.Footer,
            PublicWidgetZones.AccountNavigationAfter
        });
        }

        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(WidgetsCustomerQueryViewComponent);
        }

      
        public override async Task InstallAsync()
        {
            // Create the table using migrations
            _migrationManager.ApplyUpMigrations(typeof(CustomerQueryRecordBuilder).Assembly);

            // Add topic for the menu item
            var topic = new Topic
            {
                SystemName = "CustomerQuery",
                IncludeInTopMenu = true,
                DisplayOrder = 99,
                Title = "Customer Query",
                Body = "",
                Published = true,
                IncludeInSitemap = true,
                AccessibleWhenStoreClosed = true,
                IncludeInFooterColumn1 = true,

            };

            await _topicService.InsertTopicAsync(topic);
            // Add URL record for the topic
            await _urlRecordService.SaveSlugAsync(topic, "customer-query", 0);
            // Localization
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Admin.CustomerQueries.Details"] = "Customer Query Details",
                ["Admin.CustomerQueries.BackToList"] = "back to customer query list",
                ["Plugins.Widgets.CustomerQuery.SubmitQuery"] = "Submit a Query",
                ["Plugins.Widgets.CustomerQuery.Fields.Name"] = "Name",
                ["Plugins.Widgets.CustomerQuery.Fields.Name.Required"] = "Name is required",
                ["Plugins.Widgets.CustomerQuery.Fields.Name.Hint"] = "Write your name",
                ["Plugins.Widgets.CustomerQuery.Fields.Email"] = "Email",
                ["Plugins.Widgets.CustomerQuery.Fields.Email.Required"] = "Email is required",
                ["Plugins.Widgets.CustomerQuery.Fields.Email.Hint"] = "Write your valid email",
                ["Plugins.Widgets.CustomerQuery.Fields.Email.Wrong"] = "Wrong email format",
                ["Plugins.Widgets.CustomerQuery.Fields.Subject"] = "Subject",
                ["Plugins.Widgets.CustomerQuery.Fields.Subject.Hint"] = "Write a subject of your query",
                ["Plugins.Widgets.CustomerQuery.Fields.Message"] = "Message",
                ["Plugins.Widgets.CustomerQuery.Fields.Message.Required"] = "Message is required",
                ["Plugins.Widgets.CustomerQuery.Fields.Message.Hint"] = "Write your query details",
                ["Plugins.Widgets.CustomerQuery.Submit"] = "Submit Query",
                ["Plugins.Widgets.CustomerQuery.Email.Customer.Subject"] = "Your query has been received",
                ["Plugins.Widgets.CustomerQuery.Email.Customer.Body"] = "<p>Dear %CustomerName%,</p><p>Thank you for submitting your query. We have received your message and will respond shortly.</p><p>Your message:</p><p>%QueryMessage%</p><p>Best regards,</p><p>%StoreName%</p>",
                ["Plugins.Widgets.CustomerQuery.Email.StoreOwner.Subject"] = "New customer query received",
                ["Plugins.Widgets.CustomerQuery.Email.StoreOwner.Body"] = "<p>A new customer query has been submitted:</p><p>From: %CustomerName% (%CustomerEmail%)</p><p>Subject: %QuerySubject%</p><p>Message:</p><p>%QueryMessage%</p>",
                // Display Names (Labels)
                ["Plugins.Widgets.CustomerQuery.List.SearchEmail"] = "Email",
                ["Plugins.Widgets.CustomerQuery.List.SearchCreatedOnFrom"] = "Start Date",
                ["Plugins.Widgets.CustomerQuery.List.SearchCreatedOnTo"] = "End Date",

                // Hints (tooltips)
                ["Plugins.Widgets.CustomerQuery.List.SearchEmail.Hint"] = "Search by customer email",
                ["Plugins.Widgets.CustomerQuery.List.SearchCreatedOnFrom.Hint"] = "Search for queries from this date",
                ["Plugins.Widgets.CustomerQuery.List.SearchCreatedOnTo.Hint"] = "Search for queries up to this date",

                // Validation messages
                ["Plugins.Widgets.CustomerQuery.List.SearchEmail.Wrong"] = "Please enter a valid email address",
                ["Plugins.Widgets.CustomerQuery.List.SearchDate.Wrong"] = "End date must be greater than or equal to start date",
                ["Plugins.Widgets.CustomerQuery.Success.Title"] = "Query Submitted Successfully",
                ["Plugins.Widgets.CustomerQuery.Success.Message"] = "Thank you for your query. We have received your message and will respond to you shortly.",


                // Configuration page related
                ["Plugins.Widgets.CustomerQuery.Configuration"] = "Customer Query Configuration",
                ["Plugins.Widgets.CustomerQuery.Settings.Enabled"] = "Enable customer query",
                ["Plugins.Widgets.CustomerQuery.Settings.Enabled.Hint"] = "Check to enable customer query functionality",
                ["Plugins.Widgets.CustomerQuery.Settings.DisplayInNavigation"] = "Display in navigation",
                ["Plugins.Widgets.CustomerQuery.Settings.DisplayInNavigation.Hint"] = "Check to display customer query link in navigation menu",
                ["Plugins.Widgets.CustomerQuery.Settings.DisplayInFooter"] = "Display in footer",
                ["Plugins.Widgets.CustomerQuery.Settings.DisplayInFooter.Hint"] = "Check to display customer query link in footer",
                ["Plugins.Widgets.CustomerQuery.Settings.DisplayInCustomerProfile"] = "Display in customer profile",
                ["Plugins.Widgets.CustomerQuery.Settings.DisplayInCustomerProfile.Hint"] = "Check to display customer query link in customer profile"


            });

            var emailAccount = _customerQueryService.GetEmailAccountAsync();

            var messageTemplates = new List<MessageTemplate>
    {
        new()
        {
            Name = "Customer.Query.Notification",
            Subject = "New query from %Customer.FullName%",
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
            Subject = "Your query has been received",
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

            // Any DB or setting setup
            await base.InstallAsync();          

        }

        public override async Task UninstallAsync()
        {
            // Remove the table
            _migrationManager.ApplyDownMigrations(typeof(CustomerQueryRecordBuilder).Assembly);

            // Delete locale resources
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.CustomerQuery");

            // Remove the topic and its URL record
            var topic = await _topicService.GetTopicBySystemNameAsync("CustomerQuery");
            if (topic != null)
            {
                // First get the URL records for this topic
                var urlRecords = (await _urlRecordService.GetAllUrlRecordsAsync())
                    .Where(ur => ur.EntityId == topic.Id && ur.EntityName == typeof(Topic).Name)
                    .ToList();

                // Delete URL records if any exist
                if (urlRecords.Any())
                    await _urlRecordService.DeleteUrlRecordsAsync(urlRecords);

                // Delete topic
                await _topicService.DeleteTopicAsync(topic);
            }


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
