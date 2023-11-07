using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Misc.SimpleLMS.Domains;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Models
{
    public partial record VideoModel : BaseNopEntityModel
    {
        public VideoModel()
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
    }
}
