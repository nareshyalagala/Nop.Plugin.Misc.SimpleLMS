using System;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Stores;

namespace Nop.Plugin.Misc.SimpleLMS.Domains
{
    public partial class Lesson : BaseEntity, ILocalizedEntity, IStoreMappingSupported
    {
        public string Name { get; set; }

        public int LessonTypeId { get; set; }

        public string LessonContents { get; set; }

        public int? VideoId { get; set; }

        public int? DocumentId { get; set; }

        public int? PictureId { get; set; }

        public bool LimitedToStores { get; set; }

        public int InstructorId { get; set; }

        public Guid InstructorGuid { get; set; }

        //public int ExamId { get; set; }

        public bool IsFreeLesson { get; set; } 

        public LessonType LessonType
        {
            get => (LessonType)LessonTypeId;
            set => LessonTypeId = (int)value;
        }

    }
}
