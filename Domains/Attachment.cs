using System;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Stores;

namespace Nop.Plugin.Misc.SimpleLMS.Domains
{
    public partial class Attachment : BaseEntity, ILocalizedEntity, IStoreMappingSupported
    {
        public int AtttachmentTypeId { get; set; }

        public string Name { get; set; }

        public string VirtualPath { get; set; }

        public bool LimitedToStores { get; set; }

        public int InstructorId { get; set; }

        public Guid InstructorGuid { get; set; }

        public AttachmentType AttachmentType
        {
            get => (AttachmentType)AtttachmentTypeId;
            set => AtttachmentTypeId = (int)value;
        }
    }
}