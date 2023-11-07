using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Models
{
    public partial record CourseListModel : BasePagedListModel<CourseModel>
    {
        public CourseListModel()
        {

        }
    }
}
