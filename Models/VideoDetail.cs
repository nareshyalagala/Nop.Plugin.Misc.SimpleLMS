using Nop.Plugin.Misc.SimpleLMS.Domains;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.SimpleLMS.Models
{
    public record VideoDetail : BaseNopEntityModel
    {
        public VideoDetail()
        {
        }

        [NopResourceDisplayName("SimpleLMS.VideoType")]
        public VideoType VideoType { get; set; }

        [NopResourceDisplayName("SimpleLMS.VideoIdFromProvider")]
        public string VideoIdFromProvider { get; set; }

        [NopResourceDisplayName("SimpleLMS.Duration")]
        public int Duration { get; set; }

        [NopResourceDisplayName("SimpleLMS.VideoUrl")]
        public string VideoUrl { get; set; }

        [NopResourceDisplayName("SimpleLMS.VideoEmbedCode")]
        public string VideoEmbedCode { get; set; }


        public string TimeCode { get; set; }

    }
}