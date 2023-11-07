using Nop.Data.Mapping;
using Nop.Plugin.Misc.SimpleLMS.Domains;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Misc.SimpleLMS.Mapping
{
    public partial class NameCompatibility : INameCompatibility
    {
        public Dictionary<Type, string> TableNames => new Dictionary<Type, string>()
        {
            {typeof(Course),"SimpleLMS_Course" },
            {typeof(Video),"SimpleLMS_Video" },
            {typeof(Attachment),"SimpleLMS_Attachment" },
            {typeof(SectionLesson),"SimpleLMS_SectionLesson" },
            {typeof(Section),"SimpleLMS_Section" },
            {typeof(LessonProgress),"SimpleLMS_LessonProgress" },
            {typeof(Lesson),"SimpleLMS_Lesson" },
            {typeof(CourseProgress),"SimpleLMS_CourseProgress" }
            //{typeof(LessonAttachment),"SimpleLMS_LessonAttachment" }
        };

        public Dictionary<(Type, string), string> ColumnName => new Dictionary<(Type, string), string>();
    }
}