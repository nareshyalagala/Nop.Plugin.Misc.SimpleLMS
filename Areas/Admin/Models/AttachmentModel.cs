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
    public partial record AttachmentModel : BaseNopEntityModel
    {
        public AttachmentModel()
        {

        }

        [NopResourceDisplayName("SimpleLMS.AttachmentType")]
        public AttachmentType AttachmentType { get; set; }

        [NopResourceDisplayName("SimpleLMS.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("SimpleLMS.VirtualPath")]
        public string VirtualPath { get; set; }
    }
}
