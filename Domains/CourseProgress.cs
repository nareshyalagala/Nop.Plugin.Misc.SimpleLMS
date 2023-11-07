using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Stores;

namespace Nop.Plugin.Misc.SimpleLMS.Domains
{
    public partial class CourseProgress : BaseEntity, ILocalizedEntity, IStoreMappingSupported
    {
        public int CourseId { get; set; }

        public int StudentId { get; set; }

        public bool LimitedToStores { get; set; }
    }
}
