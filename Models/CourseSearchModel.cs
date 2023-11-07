using System;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.SimpleLMS.Models
{
    public partial record CourseSearchModel : BaseSearchModel
    {
        public CourseSearchModel()
        {
            CourseOverviewList = new CourseOverviewListModel();
            PageNumber = 1;
            Length = 6;
        }

        [NopResourceDisplayName("SimpleLMS.SearchCourseName")]
        public string SearchCourseName { get; set; }

        public CourseOverviewListModel CourseOverviewList { get; set; }

        public int PageNumber { get; set; }
         
    }
}

