using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Stores;

namespace Nop.Plugin.Misc.SimpleLMS.Domains
{
    public partial class LessonAttachment : BaseEntity, ILocalizedEntity, IStoreMappingSupported
    {
        public int LessonId { get; set; }

        public int AttachmentId { get; set; }

        public bool LimitedToStores { get; set; }
    }
}
