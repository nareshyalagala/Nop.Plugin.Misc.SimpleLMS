using Nop.Data.Extensions;
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.SimpleLMS.Domains;
using Nop.Core.Domain.Customers;

public class VideoBuilder : NopEntityBuilder<Video>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(Video.Id))
                .AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(Video.VideoTypeId))
                   .AsInt32().NotNullable()
               .WithColumn(nameof(Video.VideoIdFromProvider))
                   .AsString(4000).NotNullable()
                .WithColumn(nameof(Video.Duration))
                    .AsInt32().NotNullable()
                .WithColumn(nameof(Video.VideoUrl))
                   .AsString(4000).Nullable()
               .WithColumn(nameof(Video.VideoEmbedCode))
                   .AsString(4000).Nullable()
               .WithColumn(nameof(Video.InstructorGuid))
                   .AsGuid().NotNullable()
               .WithColumn(nameof(Video.InstructorId))
                .AsInt32().NotNullable().ForeignKey<Customer>(onDelete: System.Data.Rule.None)
               .WithColumn(nameof(Video.LimitedToStores))
                   .AsBoolean().NotNullable();
        ;
    }
}
