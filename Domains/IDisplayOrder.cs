using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.SimpleLMS.Domains
{
    public partial interface IDisplayOrderRecord
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        public string DisplayText { get; set; }
    }

    public partial class DisplayOrderRecord : IDisplayOrderRecord
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        public string DisplayText { get; set; }
    }
}
