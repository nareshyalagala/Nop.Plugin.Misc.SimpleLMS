using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Stores;

namespace Nop.Plugin.Misc.SimpleLMS.Domains
{
    public class SectionLesson : BaseEntity, ILocalizedEntity, IStoreMappingSupported
    {
        public int CourseId { get; set; }

        public int SectionId { get; set; }

        public int LessonId { get; set; }

        public bool IsFreeLesson { get; set; }

        public int DisplayOrder { get; set; }

        public bool LimitedToStores { get; set; }
    }
}
