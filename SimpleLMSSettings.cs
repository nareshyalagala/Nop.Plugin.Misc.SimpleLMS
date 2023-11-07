using Nop.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.SimpleLMS
{
    public class SimpleLMSSettings : ISettings
    {
        //public string YouTubeApiKey { get; set; }
        //public string VimeoAppId { get; set; }
        public string VimeoClient { get; set; }
        public string VimeoSecret { get; set; }
        public string VimeoAccess { get; set; }
        //public string VdoCipherKey { get; set; }
    }
}
