using System;
using Nop.Plugin.Misc.SimpleLMS.Domains;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.SimpleLMS.Models
{
    public record LessonDetail : BaseNopEntityModel
    {
        public LessonDetail()
        {
        }

        [NopResourceDisplayName("SimpleLMS.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("SimpleLMS.LessonType")]
        public LessonType LessonType { get; set; }

        [NopResourceDisplayName("SimpleLMS.LessonContents")]
        public string LessonContents { get; set; }

        [NopResourceDisplayName("SimpleLMS.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("SimpleLMS.Instructor")]
        public int InstructorName { get; set; }

        [NopResourceDisplayName("SimpleLMS.SectionName")]
        public int SectionId { get; set; }

        [NopResourceDisplayName("SimpleLMS.IsFree")]
        public bool IsFreeLesson { get; set; }

        public VideoDetail Video { get; set; }

        [NopResourceDisplayName("SimpleLMS.VideoType")]
        public VideoType VideoType { get; set; }

        [NopResourceDisplayName("SimpleLMS.VideoIdFromProvider")]
        public string VideoIdFromProvider { get; set; }

        [NopResourceDisplayName("SimpleLMS.Duration")]
        public int Duration { get; set; }

        [NopResourceDisplayName("SimpleLMS.VideoUrl")]
        public string VideoUrl { get; set; }

        [NopResourceDisplayName("SimpleLMS.VideoEmbedCode")]
        public string VideoEmbedCode { get; set; }


        [NopResourceDisplayName("SimpleLMS.CourseName")]
        public int CourseId { get; internal set; }

        public bool IsCompleted { get; set; }
    }
}

