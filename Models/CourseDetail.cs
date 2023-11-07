using System;
using System.Collections.Generic;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Linq;

namespace Nop.Plugin.Misc.SimpleLMS.Models
{
    public record CourseDetail : BaseNopEntityModel
    {
        public CourseDetail()
        {
            Sections = new List<SectionDetail>();
        }

        [NopResourceDisplayName("SimpleLMS.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("SimpleLMS.ProductName")]
        public string ParentProductName { get; set; }


        [NopResourceDisplayName("SimpleLMS.ProductName")]
        public int ProductId { get; set; }

        [NopResourceDisplayName("SimpleLMS.Sections")]
        public int SectionsTotal { get; set; }

        [NopResourceDisplayName("SimpleLMS.Lessons")]
        public int LessonsTotal { get; set; }

        public IList<SectionDetail> Sections { get; set; }

        public int TotalLessons { get { return Sections.SelectMany(p => p.Lessons).ToList().Count; } }

        public int CompletedLessons { get { return Sections.SelectMany(p => p.Lessons.Where(q => q.IsCompleted)).Count(); } }

        public int Progress { get { return (TotalLessons > 0 ? CompletedLessons * 100 / TotalLessons : 0); } }

        public int CurrentSection { get; set; }
        public int CurrentLesson { get; set; }
    }


}

