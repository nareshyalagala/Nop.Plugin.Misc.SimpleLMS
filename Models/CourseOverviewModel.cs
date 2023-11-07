using Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Models;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.SimpleLMS.Models
{
    public partial record CourseOverviewModel : BaseNopEntityModel
    {

        public CourseOverviewModel()
        {
            CourseProgress = 0;
        }

        public string Name { get; set; }
        public string ParentProductName { get; set; }       
        public string ProductMainImage { get; set; } 
        public int CourseProgress { get; set; }

    }
}
