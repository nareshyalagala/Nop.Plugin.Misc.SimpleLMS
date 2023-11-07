using System;
using System.Collections.Generic;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Linq;

namespace Nop.Plugin.Misc.SimpleLMS.Models
{
    public record SectionDetail : BaseNopEntityModel
    {
        public SectionDetail()
        {
            Lessons = new List<LessonDetail>();
        }

        [NopResourceDisplayName("SimpleLMS.Name")]
        public string Title { get; set; }

        [NopResourceDisplayName("SimpleLMS.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("SimpleLMS.CourseName")]
        public string CourseName { get; set; }

        public int CourseId { get; set; }

        [NopResourceDisplayName("SimpleLMS.IsFree")]
        public bool IsFree { get; set; }

        public IList<LessonDetail> Lessons { get; set; }

        public int TotalLessons { get { return Lessons.Count; } }

        public int CompletedLessons { get { return Lessons.Where(p => p.IsCompleted).Count(); } }

        public int Duration { get
            {
                return
                    Lessons.Select(p=>p.Duration).Sum();
            } }
    }
}

