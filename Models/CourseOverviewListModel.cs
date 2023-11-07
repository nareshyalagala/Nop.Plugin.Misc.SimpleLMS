using System;
using System.Collections.Generic;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.UI.Paging;
using Nop.Web.Models.Common;

namespace Nop.Plugin.Misc.SimpleLMS.Models
{
    public partial record CourseOverviewListModel : BasePageableModel
    {
        public CourseOverviewListModel()
        {
            Courses = new List<CourseOverviewModel>();
        }

        public IList<CourseOverviewModel> Courses { get; set; }


    }


}

