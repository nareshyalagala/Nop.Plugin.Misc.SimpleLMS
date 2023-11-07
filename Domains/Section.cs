using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Stores;

namespace Nop.Plugin.Misc.SimpleLMS.Domains
{
    public partial class Section : BaseEntity, ILocalizedEntity, IStoreMappingSupported
    {
        public int CourseId { get; set; }

        public string Title { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsFree { get; set; }

        public bool LimitedToStores { get; set; }
    }
}
