using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Misc.SimpleLMS.Domains;

namespace Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Models
{
    public partial record SortableEntity
    {
        public int ParentId { get; set; }

        public SortRecordType SortRecordType { get; set; }

        public IList<SortRecord> SortRecords { get; set; }

        public List<string> NewSortOrderValues { get; set; }

    }
     
}
