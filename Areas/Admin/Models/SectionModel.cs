using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Models
{
    public partial record SectionModel : BaseNopEntityModel
    {
        public SectionModel()
        {
            Lessons = new List<LessonModel>();
        }

        [NopResourceDisplayName("SimpleLMS.Name")]
        public string Title { get; set; }

        [NopResourceDisplayName("SimpleLMS.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("SimpleLMS.CourseName")]
        public string CourseName { get; set; }

        public int CourseId { get; set; }

        [NopResourceDisplayName("SimpleLMS.IsFree")]
        public bool IsFree { get; set; }

        public IList<LessonModel> Lessons { get; set; }
    }
}
