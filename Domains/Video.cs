using System;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Stores;

namespace Nop.Plugin.Misc.SimpleLMS.Domains
{
    public partial class Video : BaseEntity, ILocalizedEntity, IStoreMappingSupported
    {
        public int VideoTypeId { get; set; }

        public string VideoIdFromProvider { get; set; }

        public string VideoUrl { get; set; }

        public int Duration { get; set; }

        public string VideoEmbedCode { get; set; }

        public bool LimitedToStores { get; set; }

        public int InstructorId { get; set; }

        public Guid InstructorGuid { get; set; }

        public VideoType VideoType
        {
            get => (VideoType)VideoTypeId;
            set => VideoTypeId = (int)value;
        }

    }
}
