using System;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.SimpleLMS.Models
{
    public record LessonStatusModel : BaseNopEntityModel
    {
        public LessonStatusModel()
        {
        }

        public int CourseId { get; set; }
        public int SectionId { get; set; }
        public int LessonId { get; set; }
        public bool IsCompleted { get; set; }
        public int CurrentlyAt { get; set; }

    }
}

