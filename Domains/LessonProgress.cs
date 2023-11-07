using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Stores;

namespace Nop.Plugin.Misc.SimpleLMS.Domains
{
    public partial class LessonProgress : BaseEntity, ILocalizedEntity, IStoreMappingSupported
    {
        public int CourseProgressId { get; set; }

        public int LessonId { get; set; }

        public bool IsCompleted { get; set; }

        public int CurrentlyAt { get; set; }

        public bool LimitedToStores { get; set; }
    }
}
