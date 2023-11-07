using FluentMigrator.Builders.Create.Table; 
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.SimpleLMS.Domains;
using Nop.Data.Extensions;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;

namespace Nop.Plugin.Misc.SimpleLMS.Data.Mapping.Builders
{
    public class LessonBuilder : NopEntityBuilder<Lesson>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(Lesson.Id))
                    .AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(Lesson.Name))
                    .AsString(500).NotNullable()
                .WithColumn(nameof(Lesson.LessonTypeId))
                    .AsInt32().NotNullable()
                .WithColumn(nameof(Lesson.LessonContents))
                    .AsString(4000).Nullable()
                .WithColumn(nameof(Lesson.VideoId))
                    .AsInt32().Nullable().ForeignKey<Video>(onDelete: System.Data.Rule.None)
                .WithColumn(nameof(Lesson.PictureId))
                    .AsInt32().Nullable().ForeignKey<Picture>(onDelete: System.Data.Rule.None)
                .WithColumn(nameof(Lesson.DocumentId))
                    .AsInt32().Nullable().ForeignKey<Attachment>(onDelete: System.Data.Rule.None)               
                .WithColumn(nameof(Lesson.LimitedToStores))
                    .AsBoolean().NotNullable()  
                 .WithColumn(nameof(Lesson.IsFreeLesson))
                    .AsBoolean().NotNullable()
                .WithColumn(nameof(Lesson.InstructorId))
                    .AsInt32().NotNullable().ForeignKey<Customer>(onDelete: System.Data.Rule.None)
                .WithColumn(nameof(Lesson.InstructorGuid))
                    .AsGuid().NotNullable();
            ;
        }
    }
}