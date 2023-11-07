using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Models
{
    public partial record SortRecord : BaseNopEntityModel
    {
        public SortRecord()
        {

        }

        public int ExistingSortOrder { get; set; }
        public int NewSortOrder { get; set; }
        public string DisplayText { get; set; }
    }
}