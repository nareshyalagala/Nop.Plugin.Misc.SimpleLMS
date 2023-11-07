using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Models
{
  public record ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.SimpleLMS.Configuration.YouTubeApiKey.Text")]
        public string YouTubeApiKey { get; set; }
        [NopResourceDisplayName("Plugins.SimpleLMS.Configuration.VimeoAppId.Text")]
        public string VimeoAppId { get; set; }
        [NopResourceDisplayName("Plugins.SimpleLMS.Configuration.VimeoClient.Text")]
        public string VimeoClient { get; set; }
        [NopResourceDisplayName("Plugins.SimpleLMS.Configuration.VimeoSecret.Text")]
        public string VimeoSecret { get; set; }
        [NopResourceDisplayName("Plugins.SimpleLMS.Configuration.VimeoAccess.Text")]
        public string VimeoAccess { get; set; }
        [NopResourceDisplayName("Plugins.SimpleLMS.Configuration.VdoCipherKey.Text")]
        public string VdoCipherKey { get; set; }
    }
}
