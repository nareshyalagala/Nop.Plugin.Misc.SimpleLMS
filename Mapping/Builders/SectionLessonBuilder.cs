using Nop.Data.Extensions;
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.SimpleLMS.Domains;

public class SectionLessonBuilder : NopEntityBuilder<SectionLesson>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(SectionLesson.Id))
                    .AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(SectionLesson.CourseId))
                   .AsInt32().NotNullable().ForeignKey<Course>(onDelete: System.Data.Rule.None)
               .WithColumn(nameof(SectionLesson.SectionId))
                   .AsInt32().NotNullable().ForeignKey<Section>(onDelete: System.Data.Rule.None)
                .WithColumn(nameof(SectionLesson.LessonId))
                   .AsInt32().NotNullable().ForeignKey<Lesson>(onDelete: System.Data.Rule.None)
               .WithColumn(nameof(SectionLesson.DisplayOrder))
                   .AsInt32().NotNullable()
               .WithColumn(nameof(SectionLesson.IsFreeLesson))
                   .AsBoolean().NotNullable()
               .WithColumn(nameof(SectionLesson.LimitedToStores))
                   .AsBoolean().NotNullable();
        ;
    }
}
