using DocumentFormat.OpenXml.Drawing.Charts;
using FluentMigrator;
using Nop.Core;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Widgets.CustomerQuery.Domain;

namespace Nop.Plugin.Widgets.CustomerQuery.Data;

[NopMigration("2025/07/28 12:00:00", "Customer Query - Create Table", MigrationProcessType.Installation)]
public class SchemaMigration : AutoReversingMigration
{
    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        Create.TableFor<CustomerQueryRecord>();
    }

    /// <summary>
    /// No need for table didn't not delete, it will be automatically deleted by NopCommerce
    /// </summary>
   /* public override void Down()
    {
        Delete.Table(nameof(CustomerQueryRecord));
    }*/
}