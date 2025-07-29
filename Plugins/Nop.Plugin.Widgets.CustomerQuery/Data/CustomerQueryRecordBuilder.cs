using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Widgets.CustomerQuery.Domain;

namespace Nop.Plugin.Widgets.CustomerQuery.Data;

public class CustomerQueryRecordBuilder : NopEntityBuilder<CustomerQueryRecord>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(CustomerQueryRecord.Name)).AsString(400).NotNullable()
            .WithColumn(nameof(CustomerQueryRecord.Email)).AsString(400).NotNullable()
            .WithColumn(nameof(CustomerQueryRecord.Subject)).AsString(400).Nullable()
            .WithColumn(nameof(CustomerQueryRecord.Message)).AsString(int.MaxValue).NotNullable()
            .WithColumn(nameof(CustomerQueryRecord.CreatedOnUtc)).AsDateTime2().NotNullable();
    }
}