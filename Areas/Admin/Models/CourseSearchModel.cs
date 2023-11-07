using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Models
{
    public partial record CourseSearchModel : BaseSearchModel
    {
        public CourseSearchModel()
        {
        }

        [NopResourceDisplayName("SimpleLMS.SearchCourseName")]
        public string SearchCourseName { get; set; }

        [NopResourceDisplayName("SimpleLMS.InstructorGUID")]
        public string InstructorGUID { get; set; }
    }
}
