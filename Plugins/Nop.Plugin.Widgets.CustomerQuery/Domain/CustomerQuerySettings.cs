using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.CustomerQuery.Domain
{
    public class CustomerQuerySettings : ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to show in header navigation
        /// </summary>
        public bool DisplayInNavigation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show in footer
        /// </summary>
        public bool DisplayInFooter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show in customer profile
        /// </summary>
        public bool DisplayInCustomerProfile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the plugin is enabled
        /// </summary>
        public bool Enabled { get; set; }
    }
}
