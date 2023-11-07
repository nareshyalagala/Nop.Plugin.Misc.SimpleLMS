using Nop.Data.Extensions;
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.SimpleLMS.Domains;

public class SectionBuilder : NopEntityBuilder<Section>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(Section.Id))
                    .AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(Section.CourseId))
                   .AsInt32().NotNullable().ForeignKey<Course>(onDelete: System.Data.Rule.None)
               .WithColumn(nameof(Section.Title))
                   .AsString(500).NotNullable()
               .WithColumn(nameof(Section.DisplayOrder))
                   .AsInt32().NotNullable()
               .WithColumn(nameof(Section.IsFree))
                   .AsBoolean().NotNullable()
               .WithColumn(nameof(Section.LimitedToStores))
                   .AsBoolean().NotNullable();
        ;
    }
}
