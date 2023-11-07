using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.SimpleLMS.Domains;
using Nop.Data.Extensions;
using Nop.Core.Domain.Customers;

namespace Nop.Plugin.Misc.SimpleLMS.Data.Mapping.Builders
{
    public class AtttachmentBuilder : NopEntityBuilder<Attachment>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(Attachment.Id))
                .AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(Attachment.AtttachmentTypeId))
                    .AsInt32().NotNullable()
                .WithColumn(nameof(Attachment.Name))
                    .AsString(500).NotNullable()
                .WithColumn(nameof(Attachment.VirtualPath))
                    .AsString().NotNullable()
                .WithColumn(nameof(Attachment.InstructorId))
                    .AsInt32().NotNullable().ForeignKey<Customer>(onDelete: System.Data.Rule.None)
                .WithColumn(nameof(Attachment.InstructorGuid))
                    .AsGuid().NotNullable()
                .WithColumn(nameof(Attachment.LimitedToStores))
                   .AsBoolean().NotNullable();
            ;
        }
    }
}