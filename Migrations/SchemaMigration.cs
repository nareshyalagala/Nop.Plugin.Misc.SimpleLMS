using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Mapping;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.SimpleLMS.Domains;

namespace Nop.Plugin.Misc.SimpleLMS.Migrations
{
    [NopMigration("2022/06/11 16:21:00:0000000","4.5",UpdateMigrationType.Data ,MigrationProcessType.Installation)] 
    public class SchemaMigration : Migration
    { 
        public SchemaMigration()
        {
             
        }

        public override void Down()
        {
             
        }

        /// <summary>
        /// Collect the UP migration expressions
        /// </summary>
        public override void Up()
        {

            if(!Schema.Table(NameCompatibilityManager.GetTableName(typeof(Course))).Exists())
            {
                Create.TableFor<Course>();
            }

            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(CourseProgress))).Exists())
            {
                Create.TableFor<CourseProgress>();
            }

            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(Attachment))).Exists())
            {
                Create.TableFor<Attachment>();
            }

            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(Video))).Exists())
            {
                Create.TableFor<Video>();
            }

            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(Lesson))).Exists())
            {
                Create.TableFor<Lesson>();
            }
            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(LessonAttachment))).Exists())
            {
                Create.TableFor<LessonAttachment>();
            }

            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(LessonProgress))).Exists())
            {
                Create.TableFor<LessonProgress>();
            }

            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(Section))).Exists())
            {
                Create.TableFor<Section>();
            }

            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(SectionLesson))).Exists())
            {
                Create.TableFor<SectionLesson>();
            }             
        }
    }
}
