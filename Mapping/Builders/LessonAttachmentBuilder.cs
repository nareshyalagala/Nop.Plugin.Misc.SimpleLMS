using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.SimpleLMS.Domains;
using Nop.Data.Extensions;

namespace Nop.Plugin.Misc.SimpleLMS.Mapping.Builders
{
    public class LessonAttachmentBuilder : NopEntityBuilder<LessonAttachment>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(LessonAttachment.Id))
                    .AsInt32().PrimaryKey().Identity()
                    .WithColumn(nameof(LessonAttachment.LessonId))
                    .AsInt32().Nullable().ForeignKey<Lesson>(onDelete: System.Data.Rule.None)
                    .WithColumn(nameof(LessonAttachment.AttachmentId))
                    .AsInt32().Nullable().ForeignKey<Attachment>(onDelete: System.Data.Rule.None);
        }
    }
}
