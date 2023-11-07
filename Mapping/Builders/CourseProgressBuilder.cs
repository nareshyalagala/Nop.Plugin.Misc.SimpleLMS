using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.SimpleLMS.Domains;
using Nop.Data.Extensions;
using Nop.Core.Domain.Customers;

namespace Nop.Plugin.Misc.SimpleLMS.Data.Mapping.Builders
{
    public class CourseProgressBuilder : NopEntityBuilder<CourseProgress>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(CourseProgress.Id))
                    .AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(CourseProgress.CourseId))
                    .AsInt32().NotNullable().ForeignKey<Course>(onDelete: System.Data.Rule.None)
                .WithColumn(nameof(CourseProgress.StudentId))
                    .AsInt32().NotNullable().ForeignKey<Customer>(onDelete: System.Data.Rule.None)
                .WithColumn(nameof(CourseProgress.LimitedToStores))
                    .AsBoolean().NotNullable();
        }
    }
}