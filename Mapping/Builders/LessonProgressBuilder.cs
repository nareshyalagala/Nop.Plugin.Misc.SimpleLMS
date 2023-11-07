using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.SimpleLMS.Domains;
using Nop.Data.Extensions;
using Nop.Core.Domain.Customers;

namespace Nop.Plugin.Misc.SimpleLMS.Data.Mapping.Builders
{
    public class LessonProgressBuilder : NopEntityBuilder<LessonProgress>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(LessonProgress.Id))
                    .AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(LessonProgress.CourseProgressId))
                    .AsInt32().NotNullable().ForeignKey<CourseProgress>(onDelete: System.Data.Rule.None)
                .WithColumn(nameof(LessonProgress.LessonId))
                    .AsInt32().NotNullable().ForeignKey<Lesson>(onDelete: System.Data.Rule.None)
                .WithColumn(nameof(LessonProgress.IsCompleted))
                    .AsBoolean().Nullable()
                .WithColumn(nameof(LessonProgress.CurrentlyAt))
                    .AsInt32().Nullable()
                .WithColumn(nameof(LessonProgress.LimitedToStores))
                    .AsBoolean().NotNullable();
            ;
        }
    }
}